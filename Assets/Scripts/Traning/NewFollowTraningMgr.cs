using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NewFollowTraningMgr : MonoBehaviour
{
    public List<GameObject> Nodes;
    public GameObject dog;
    public GameObject gameOverPanel;
    public GameObject failPanel;
    int curNode = -1;
    public bool isMoving;
    public bool canTap;
    public float dogSpeed;
    Animator dogAnim;
    public Vector3 StartPos;
    public GameObject movingTarget;
    public float timer;
    public Text TxtTimer;
    public Text TxtGameOver;







    void Start()
    {
        dogAnim = GetComponent<Animator>();
        gameOverPanel.SetActive(false);
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
        if(isMoving)
        {
            Movement();
        }
        movingTarget.transform.Translate(250 * Time.deltaTime, 0, 0);
        Debug.Log(movingTarget.transform.position.x);
        if(movingTarget.transform.position.x >= 1000)
        {
            movingTarget.transform.position = new Vector2(0, 0);
        }
        if(movingTarget.transform.position.x>485 && movingTarget.transform.position.x<500)
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
    }
    

    void GameOver()
    {
        if(timer >= 45)
        {
            gameOverPanel.SetActive(true);
            TxtGameOver.text = "Traning Session Failed!!!";
            movingTarget.SetActive(false);

        }
    }



}


