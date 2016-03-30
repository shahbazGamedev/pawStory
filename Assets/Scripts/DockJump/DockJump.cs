using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DogStates
{
    Idle = 0,
    Sit = 1,
    Walk = 2,
    Jog = 3,

    RunStart = 10,
    Run = 11,
    RunEnd = 12,

    JumpStart = 15,
    Jump = 16,
    JumpEnd = 17,

    Win = 30,
    Lost = 31
};

public class DockJump : MonoBehaviour
{
    // Dog
    public GameObject DogObj;

    public Animator DogAnim;
    public GameObject TaptoPlay;
    private float runSpeed;
    private float jumpForce;
    private float jumpspeed;
    public Transform DogStartTrans;
    private Rigidbody rb;
    private bool isInJumpZone;
    public DogStates dogState; // 0: sit, 1: Idle, 2: jog, 3: Run, 5: Jump,
    private int TotalIdleStates = 3;
    private int IdleRandom;
    private List<float> jumpDistList;
    private Vector3 dir;// = new Vector3 (0, 0, 1);

    // Camera
    public GameObject CamObj;

    public Transform CamStartTrans;
    public Animator CamAnim;
    public bool isCamMove;

    // HUD
    public GameObject GameOverPanel;

    public GameObject TouchMat;
    public GameObject gameScreen;

    public Text TapToPlayTxt;
    public Text ScoreTxt;
    public Text JumpCountTxt;
    public Text HighScoreTxt;
    private int highScore;
    private int score;

    private bool waitForTap;
    public bool play;
    private int jumpCount;
    private int maxJumpCount;
    public Transform target;
    private float speedDampTime = 0.1f;
    private float dragRatio;
    private float distance;
    private bool isGameOver;

    private void ChangeDogState(DogStates newDogState)
    {
        dogState = newDogState;
        switch (newDogState)
        {
            case DogStates.Idle:
                IdleRandom = Random.Range(0, TotalIdleStates);
                DogAnim.SetInteger("IdleRandom", IdleRandom);
                break;

            case DogStates.Sit:
                break;

            case DogStates.Walk:
                break;

            case DogStates.Jog:
                break;

            case DogStates.RunStart:
            case DogStates.Run:
            case DogStates.RunEnd:
                break;

            case DogStates.JumpStart:
            case DogStates.Jump:
            case DogStates.JumpEnd:
                break;

            case DogStates.Win:
                break;
        };
        DogAnim.SetInteger("DogState", (int)newDogState);
    }

    public void OnRestartGame()
    {
        Debug.Log("OnRestartGame");
        rb = GetComponent<Rigidbody>();
        StartCoroutine(PlayGame());

        play = false;
        waitForTap = true;
        isGameOver = false;
        isInJumpZone = false;
        TaptoPlay.SetActive(false);

        // dog
        ChangeDogState(DogStates.Idle);
        rb.isKinematic = true;
        transform.position = DogStartTrans.position;
        runSpeed = 5;
        jumpForce = 10;
        jumpCount = 0;
        maxJumpCount = 3;
        jumpDistList = new List<float>();
        dir = new Vector3(0, 0, 1);

        // Cam
        CamObj.transform.position = CamStartTrans.position;
        isCamMove = false;
        CamAnim.enabled = true;

        // HUD
        TapToPlayTxt.text = "Tap to Play";
        JumpCountTxt.text = "Chances: " + jumpCount + " / " + maxJumpCount;
        ScoreTxt.text = "Distance: ";
        GameOverPanel.SetActive(false);
        gameScreen.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.GameRestart += OnRestartGame;
    }

    private void OnDisable()
    {
        EventManager.GameRestart -= OnRestartGame;
    }

    private void Start()
    {
        OnRestartGame();
        StartCoroutine(PlayGame());
        TaptoPlay.SetActive(false);
    }

    private void Update()
    {
        if (GameMgr.Inst.IsGamePaused())
        {
            return;
        }

        // Detect Tap
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
            && !isGameOver && play)
        {
            if (waitForTap)
            {
                waitForTap = false;
                TapToPlayTxt.text = "Tap to Jump";
                ChangeDogState(DogStates.Run);
                CamAnim.enabled = false;
            }
            else if (isInJumpZone && dogState == DogStates.Run)
            {
                ChangeDogState(DogStates.Jump);
                FinalJump();
            }
        }
    }

    private void FixedUpdate()
    {
        if (dogState == DogStates.Run && !isGameOver)
        {
            DogObj.transform.position += Time.deltaTime * dir * runSpeed;
        }
    }

    private void LateUpdate()
    {
        if (dogState == DogStates.Run && !isGameOver)
            CamObj.transform.position += new Vector3(0, 0, 1) * Time.deltaTime * 5;
    }

    private void FinalJump()
    {
        rb.isKinematic = false;
        ChangeDogState(DogStates.Jump);
        rb.AddForce(new Vector3(0, 0.5f, 0.8f) * jumpForce, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        isInJumpZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");
        isInJumpZone = false;
        rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // On dog landing to pool
        if (collision.gameObject.tag == "floor")
        {
            AnalyzeLanding();
        }
        if (jumpCount >= maxJumpCount)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        waitForTap = true;
        GameOverPanel.SetActive(true);

        TouchMat.SetActive(false);
        gameScreen.SetActive(false);

        float longDist = 0;
        for (int i = 0; i < jumpDistList.Count; i++)
        {
            if (longDist < jumpDistList[i])
            {
                longDist = jumpDistList[i];
            }
        }
        HighScoreTxt.text = "Longest Jump : " + (int)longDist + " ft"; ;
    }

    private void AnalyzeLanding()
    {
        rb.isKinematic = true;

        if (dogState == DogStates.Run)// not a jump so foul
        {
            distance = -1;
        }
        else
        {
            distance = Vector3.Distance(target.position, transform.position);
        }

        jumpDistList.Add(distance);
        StartCoroutine(NextRound());
    }

    private IEnumerator NextRound()
    {
        //ChangeDogState(DogStates.Run);
        //yield return new WaitForSeconds (0.5f);
        ChangeDogState(DogStates.RunEnd);
        yield return new WaitForSeconds(1f);
        string distStr = "Distance: Foul";
        if (distance == -1)
        {
            TapToPlayTxt.text = "";
            ChangeDogState(DogStates.Lost);
        }
        else
        {
            distStr = "Distance: " + (int)distance + " ft";
            TapToPlayTxt.text = "Cool";
            ChangeDogState(DogStates.Win);
        }
        // Update UI
        ScoreTxt.text = distStr;
        yield return new WaitForSeconds(2f);

        ChangeDogState(DogStates.Idle);
        yield return new WaitForSeconds(0.25f);

        jumpCount += 1;
        if (jumpCount >= maxJumpCount)
        {
            GameOver();
        }
        else
        {
            waitForTap = true;
            isCamMove = false;
            isGameOver = false;
            isInJumpZone = false;

            // dog
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            runSpeed = 5;
            jumpForce = 10;
            transform.position = DogStartTrans.position;

            // Cam
            CamObj.transform.position = CamStartTrans.position;

            // HUD
            TapToPlayTxt.text = "Tap to Play";
            JumpCountTxt.text = "Chances: " + jumpCount + " / " + maxJumpCount;
            GameOverPanel.SetActive(false);
        }
    }

    public void OnMainMenuBtn()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }

    public void Transition()
    {
        float transitionDuration = 2f;
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            transform.position = Vector3.MoveTowards(startingPos, target.position, t);
        }
    }

    public void CamMove()
    {
        if (isCamMove)
        {
            isCamMove = false;
            Transition();
        }
    }

    private IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(5.0f);
        play = true;
        TaptoPlay.SetActive(true);
    }
}