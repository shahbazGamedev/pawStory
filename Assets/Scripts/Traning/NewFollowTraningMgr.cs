using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NewFollowTraningMgr : MonoBehaviour
{
   //UI Elements
    public GameObject gameOverPanel;
    public GameObject failPanel;
    public Text TxtTimer;
    public Text TxtGameOver;

    //GamePlay
    int curNode = -1;
    public bool isMoving;
    public bool canTap;
    public float dogSpeed;
    public GameObject Traget;
    public List<GameObject> Nodes;
    public GameObject dog;
    public float timer;
    public Vector3 StartPos;
    public GameObject movingTarget;

    //Defaults
    Animator dogAnim;







    void Start()
    {
        dogAnim = GetComponent<Animator>();
        StartPos = transform.position;
        OnRestartGame();
       
    }
    
       
    void OnEnable()
    {
        EventMgr.GameRestart += OnRestartGame;
    }


    void OnDisable()
    {
        EventMgr.GameRestart -= OnRestartGame;
    }


    void Update()
    {
        if (isMoving)
        {
            Movement();
        }
        movingTarget.transform.Translate(350 * Time.deltaTime, 0, 0);
        
        if (movingTarget.transform.position.x > Screen.width)
        {
            movingTarget.transform.position = new Vector2(0, 0);
        }
        if (movingTarget.transform.position.x > Traget.transform.position.x && movingTarget.transform.position.x < Screen.width / 2)
        {
            Debug.Log("yes");

            canTap = true;
        }
        else
        {
            canTap = false;
        }

    }


    void FixedUpdate()
    {
        timer += Time.deltaTime;
        TxtTimer.text = "Time :" +(int)timer;
        GameOver();
    }


    public void Movement()
    {

        transform.LookAt(Nodes[curNode].transform, new Vector3(0, 1, 0));
        transform.position = Vector3.MoveTowards(transform.position, Nodes[curNode].transform.position, Time.deltaTime * dogSpeed);
        dogAnim.SetFloat("Walk", 1f);
        if (Vector3.Distance(transform.position, Nodes[curNode].transform.position) < 1f)
        {
            dogAnim.SetFloat("Walk", 0f);
            isMoving = false;
        }
    }
   

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Finish")
        {
            
            Debug.Log("collided");
            gameOverPanel.SetActive(true);
            TxtGameOver.text = "Follow Traning Sucessful!!";
            Time.timeScale = 0;
        }
    }


    public void OnPointerDown()
    {
        if (canTap)
        {
            curNode++;
            isMoving = true;
            Debug.Log(curNode);
            


        }
    }


    void OnRestartGame()
    {
       
        dog.transform.position = StartPos;
        transform.rotation = Quaternion.identity;
        gameOverPanel.SetActive(false);
        isMoving = false;
        dogAnim.SetFloat("Walk", 0f);
        curNode = -1;
        movingTarget.SetActive(true);
        movingTarget.transform.position = new Vector2(0, 0);
        timer = 0f;
        Time.timeScale = 1;
    }
    

    void GameOver()
    {
        if(timer >= 45)
        {
            gameOverPanel.SetActive(true);
            TxtGameOver.text = "Traning Session Failed!!!";
            movingTarget.SetActive(false);
            Time.timeScale = 0;

        }
    }



}


