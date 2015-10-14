using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewFollowTraningMgr : MonoBehaviour
{
    public List<GameObject> Nodes1;
    public List<GameObject> Nodes2;
    public List<GameObject> Nodes3;
    public GameObject startPos;
    public GameObject dog;
    int curNode1 = -1;
    int curNode2 =-1;
    int curNode3 = -1;
    public bool isMovingB;
    public bool isMovingG;
    public bool isMovingR;
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
        if(isMovingB)
        {
            MovementB();
        }
        if (isMovingG)
        {
            MovementG();
        }
        if (isMovingR)
        {
            MovementR();
        }
      
    }

    public void MovementB()
    {

        transform.LookAt(Nodes1[curNode1].transform, new Vector3(0, 1, 0));
        transform.position = Vector3.MoveTowards(transform.position, Nodes1[curNode1].transform.position, Time.deltaTime * dogSpeed);
        dogAnim.SetFloat("Walk", 1f);
        if (Vector3.Distance(transform.position, Nodes1[curNode1].transform.position) < 1f)
        {
            dogAnim.SetFloat("Walk", 0f);
            isMovingB = false;
            canTap = true;

        }
    }

    public void MovementG()
    {

        transform.LookAt(Nodes2[curNode2].transform, new Vector3(0, 1, 0));
        transform.position = Vector3.MoveTowards(transform.position, Nodes2[curNode2].transform.position, Time.deltaTime * dogSpeed);
        dogAnim.SetFloat("Walk", 1f);
        if (Vector3.Distance(transform.position, Nodes2[curNode2].transform.position) < 1f)
        {
            dogAnim.SetFloat("Walk", 0f);
            isMovingG = false;
            canTap = true;

        }
    }

    public void MovementR()
    {

        transform.LookAt(Nodes3[curNode3].transform, new Vector3(0, 1, 0));
        transform.position = Vector3.MoveTowards(transform.position, Nodes3[curNode3].transform.position, Time.deltaTime * dogSpeed);
        dogAnim.SetFloat("Walk", 1f);
        if (Vector3.Distance(transform.position, Nodes3[curNode3].transform.position) < 1f)
        {
            dogAnim.SetFloat("Walk", 0f);
            isMovingR = false;
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
            curNode1++;
            isMovingB = true;
            Debug.Log(curNode1);
            canTap = false;


        }
    }

    public void GreenMove()
    {
        if (canTap)
        {
            curNode2++;
            isMovingG = true;
            canTap = false;


        }
    }

    public void RedMove()
    {
        if (canTap)
        {
            curNode3++;
            isMovingR = true;
            canTap = false;


        }
    }
    void BackToStart()
    {
        transform.LookAt(startPos.transform, new Vector3(0, 1, 0));
        transform.position = Vector3.MoveTowards(transform.position, startPos.transform.position, Time.deltaTime * dogSpeed);
    }
   
}


