/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DogMovementFrisbee : MonoBehaviour
{
    public Transform[] spawnPoint;
    public float jumpForce = 200f;
    public Transform Dog;
    public GameObject dog;
    public Transform target;
    public GameObject Frisbee;
    public int Score;
    public bool isMoving;
    public bool isGameover;
    public Text chance;
    public int chances;
    public Text ChanceUi;
    private Transform targetMove;
    public int spawnValue = 0;
    public bool isSpawn;
    public GameObject FrisbeeAttached;
    public GameObject EndPanel;
    public int MaxChances;
    public int Life;
    public Text score;
    public Text life;
    public GameObject startPanel;
    public GameObject flagRed;
    public GameObject flagGreen;
    public GameObject flagYellow;
    private Animator dogAnim;
    private Vector3 frisbeedirection;
    private Vector3 dogPos;
    private float distance;
    public float speed;
    private Vector3 jumpHeight;
    private Vector3 direction;
    bool isCatching;
    Rigidbody rb;
    Vector3 position;
    Vector3 pos;
    bool isRestart;
    int lastValue;
    int currentValue;
    
    

    void OnEnable()
    {
        EventMgr.GameRestart += OnRestartGame;
    }


    void OnDisable()
    {
        EventMgr.GameRestart -= OnRestartGame;
    }


    void Awake()
    {
        jumpHeight = new Vector3(0, jumpForce, 0);
        dogAnim = GetComponent<Animator>();
        dogPos = new Vector3(-0.2f, 0.035f, 9.1f);
    }


    void Start()
    {
        EndPanel.SetActive(false);
        flagYellow.SetActive(false);
        flagGreen.SetActive(false);
        flagRed.SetActive(false);
        startPanel.SetActive(true);
        FrisbeeAttached.SetActive(false);
        SpawnValueReset();
        isMoving = true;
        rb = GetComponent<Rigidbody>();
        isGameover = false;
        Movement();
        Time.timeScale = 1;
        
        
    }


    void Update()
    {
        TempFlag();
        if (isGameover == true)
        {
            GameOver();
        }
        if (isMoving)
        {
            Movement();
        }

        if (isCatching)
        {
            
            FrisebeeCatch();
        }
        if (isSpawn)
        {
            SpawnValueReset();
            isSpawn = false;
        }
        ChanceUi.text = "No Of Chances:" + chances + " / " + MaxChances;
        life.text = "Life : " + Life;
        score.text = "Score: " + Score;
        
          
    }


    public void jumpingRight(Vector3 force)
    {
        dogAnim.SetTrigger("RightJump");
        rb.velocity = (force);
    }


    public void jumpingLeft(Vector3 force)
    {
        dogAnim.SetTrigger("LeftJump");
        rb.velocity = (force);
    }


    public void Movement()
    {
        
        distance = Vector3.Distance(targetMove.position, transform.position);
        if (distance > 1f)
        {
            transform.LookAt(direction);
        }
        direction = new Vector3(targetMove.position.x, 0, targetMove.position.z);

        dogAnim.SetFloat("Walk", 1f);
        float step = speed * Time.deltaTime;

        position = targetMove.position;
        position.y = transform.position.y;
        rb.MovePosition(Vector3.MoveTowards(transform.position, position, step));

        if (distance < 1f)
        {
            Debug.Log("Hi");
            dogAnim.SetFloat("Walk", 0f);
            transform.LookAt(frisbeedirection);
            isMoving = false;
        }
    }


    void FrisebeeCatch()
    {
        frisbeedirection = new Vector3(target.position.x, 0f, target.position.z);
        if ((transform.position - target.position).magnitude > 2f)
            transform.LookAt(frisbeedirection);
        ChanceUi.text = "No Of Chances:" + chances;
    }


    public void GameOver()
    {
        startPanel.SetActive(false);
        dog.SetActive(false);
        Frisbee.SetActive(false);
        chance.text = "SCORE: " + Score;
        EndPanel.SetActive(true);
        Time.timeScale = 0;
    }


    public void Menu()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }


    public void SpawnValueReset()
    {
        spawnValue = Random.Range(0, 3);
        while (lastValue == spawnValue)
        {
            spawnValue = Random.Range(0, 3);
        }
        targetMove = spawnPoint[spawnValue];
        lastValue = spawnValue;
    }


    void TempFlag()
    {
        if (targetMove == spawnPoint[1])
        {
            flagRed.SetActive(true);
            flagYellow.SetActive(false);
            flagGreen.SetActive(false);
        }
        if (targetMove == spawnPoint[2])
        {
            flagGreen.SetActive(true);
            flagRed.SetActive(false);
            flagYellow.SetActive(false);
        }
        if (targetMove == spawnPoint[0])
        {
            flagGreen.SetActive(false);
            flagRed.SetActive(false);
            flagYellow.SetActive(true);
        }
    }


    public void OnRestartGame()
    {
        isMoving = true;
        transform.position = dogPos;
        Score = 0;
        chances = 0;
        MaxChances = 10;
        Life = 3;
        isGameover = false;
        startPanel.SetActive(true);
        dog.SetActive(true);
        Frisbee.SetActive(true);
        FrisbeeAttached.SetActive(false);
        EndPanel.SetActive(false);
        Time.timeScale = 1;
        isRestart = false;
        if (isGameover == true)
        {
            GameOver();
        }
        Frisbee.GetComponent<FrisbeeMovement>().dummyFrisbee.SetActive(true);
        Frisbee.transform.position = new Vector3(-0.143f, .156f, -1.96f);



    }
    public void FoulCollect()
    {
        distance = Vector3.Distance(target.position, transform.position);
       
    
           // isMoving= true;
            
            transform.LookAt(target);
        dogAnim.SetTrigger("Foul");
            float step = speed * Time.deltaTime;
            pos = target.position;
            rb.MovePosition(Vector3.MoveTowards(transform.position, pos, step));
        
        if (distance < 2f)
        {
            isMoving = false;

            dogAnim.SetFloat("Walk", 0f);

        }
        Frisbee.GetComponent<FrisbeeMovement>().canCollect = false;
    }
}

	



		
	



