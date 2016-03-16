/**
Script Author : Vaikash 
Description   : Combo - Dog Movement Controller
**/

using UnityEngine;
using System.Collections;

public class DogRunner : MonoBehaviour
{
    #region Variables

    public static DogRunner instRef;

    public bool runStart;
    public static int Life;
    public Vector3 runDirection;
    string layerName;         
   // public GameObject BtnVideo;

    public static bool updateAnim;
    Vector3 startPos;
    float dogVelocity;
    bool jump;
    public static bool gameOver;
    bool isCoroutineON;
    float time;

    CharacterController dogCC;
    public Animator dogAnim;

    public float jumpSpeed;
  //  public float camFactor;
    float verticalVelocity = 0f;

    // Camera 
   // public float smoothDampTime;
   // public Vector3 cameraOffset;
   // Vector3 smoothDampVelocity = Vector3.zero;
    Vector3 dist;
    ///Vector3 pos;

    public float springForce;

    #endregion Variables

    public void Awake()
    {
        instRef = this;
        dogCC = GetComponent<CharacterController>();
        dogAnim = GetComponent<Animator>();
    }

    void Start()
    {
        // BtnVideo.SetActive(false);
        ComboManager.StartGame += StartGame;
        gameOver = true;
        startPos = transform.position;
    }

    public void OnDisable()
    {
        ComboManager.StartGame -= StartGame;
    }

    void LateUpdate()
    {
       // CamUp();
    }

    // New method using character controller for smooth movement
    public void Update()
    {
        if (runStart)
        {
            // Handle Movement
           // dist = runDirection * Time.deltaTime;
            if (dogCC.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;

            }
            else
            {
                verticalVelocity += 2.75f * Physics.gravity.y * Time.deltaTime;
            }

            // Handle Jump
            if (dogCC.isGrounded && jump)
            {
                StartCoroutine(JumpForce(jumpSpeed));
                jump = false;
            }

            dist.y = verticalVelocity * Time.deltaTime;
            dogCC.Move(dist);

            dogVelocity = 1;
            SyncAnim();
            if (Life == 5 || GlobalVariables.distanceCovered > 150f)
            {
                //BtnVideo.SetActive(true);

            }
        }


    }

    // Camera Movement
    //public void CamUp()
    //{
    //    if (runStart)
    //    {
    //        pos = Camera.main.transform.parent.transform.position;
    //        pos.z = transform.position.z;
    //        Camera.main.transform.parent.transform.position = Vector3.SmoothDamp(Camera.main.transform.parent.transform.position, pos, ref smoothDampVelocity, smoothDampTime);
    //    }
    //}

    // Reset GameOver Flag
    void ResetGameOverFlag()
    {
        gameOver = false;
    }

    // Sync Animation
    public void SyncAnim()
    {
        if (updateAnim)
        {
            dogAnim.SetFloat("Speed", dogVelocity);
            updateAnim = false;
        }
    }

    // GameOver
    public void GameOver()
    {
        verticalVelocity = 0;
        gameOver = true;
        ComboManager.instRef.GameOver();
        runStart = false;
        dogVelocity = 0;
        updateAnim = true;
        SyncAnim();
        dogAnim.SetBool("GameOver", gameOver);

    }

    // Reset Dog Pos
    public void ResetPos()
    {
        transform.position = startPos;
    }

    #region EventHandlers

    // Handle Jump Event
    public void HandleDogJump()
    {

        if (dogCC.isGrounded && runStart && !isCoroutineON)
        {
            jump = true;
            dogAnim.SetTrigger("Jump");
           // SoundManager.instance.PlaySfx(SFXVAL.jump);
        }
    }

    // Game start event handler
    void StartGame()
    {
        runStart = true;
        updateAnim = true;
        Invoke("ResetGameOverFlag", 2f);
    }
    #endregion EventHandlers

    #region Coroutines

    IEnumerator JumpForce(float force)
    {
        isCoroutineON = true;
        while (time < 0.2f)
        {
            verticalVelocity += force * Time.deltaTime;
            time += Time.deltaTime;
            force -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        time = 0;
        isCoroutineON = false;
        yield return null;
    }



    #endregion Coroutines


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("JumpMat") && !isCoroutineON)
        {
            dogAnim.SetTrigger("Fly");
            //springForce = 100;
           // SoundManager.instance.PlaySfx(SFXVAL.doubleJump);
            StartCoroutine(JumpForce(springForce));
        }
        //if (hit.gameObject.CompareTag("target") && !isCoroutineON)
        //{
        //    dogAnim.SetTrigger("Fly");
        //    //SoundManager.instance.PlaySfx(SFXVAL.doubleJump);
        //    StartCoroutine(JumpForce(springForce));
        //}
        //else if (hit.gameObject.CompareTag("LoseLine"))
        //{
        //    GameOver();
        //    if (ComboManager.LifeCalc)
        //    {
        //        Life += 1;
        //        ComboManager.LifeCalc = false;
        //    }
        //    Debug.Log(Life);


        //}
    }

    void OnTriggerEnter(Collider hit)
    {
        layerName = LayerMask.LayerToName(hit.gameObject.layer);

        switch (layerName)
        {
            case "Obstacle":
                GameOver();
                if (ComboManager.LifeCalc)
                {
                    Life += 1;
                    ComboManager.LifeCalc = false;
                }

                break;
        }
    }
}
