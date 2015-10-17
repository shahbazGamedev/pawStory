using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewFollowTraningMgr : MonoBehaviour
{
    public List<GameObject> Nodes;
    public GameObject dog;
    public GameObject gameOverPanel;
    int curNode = -1;
    public bool isMoving;
    bool canTap;
    public float dogSpeed;
    Animator dogAnim;
    bool gotoStart;
    public Vector3 StartPos;
   


    void Start()
    {
        dogAnim = GetComponent<Animator>();
        canTap = true;
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
            canTap = true;

        }
    }

   

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Finish")
        {
            
            Debug.Log("collided");
            gameOverPanel.SetActive(true);
            
            
        }
    }

    public void OnPointerDown()
    {
        if (canTap)
        {
            curNode++;
            isMoving = true;
            Debug.Log(curNode);
            canTap = false;


        }
    }

    void OnRestartGame()
    {
        dog.transform.position = StartPos;
        gameOverPanel.SetActive(false);
        transform.LookAt(new Vector3(0, 0, 0));
    }

   
    
   
}


