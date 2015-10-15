using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewFollowTraningMgr : MonoBehaviour
{
    public List<GameObject> Nodes;
    public GameObject startPos;
    public GameObject dog;
    int curNode = -1;
    public bool isMoving;
    bool canTap;
    public float dogSpeed;
    Animator dogAnim;
    bool gotoStart;
   


    void Start()
    {
        dogAnim = GetComponent<Animator>();
        canTap = true;
          
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
            gotoStart = true;
            
            
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

   
    
   
}


