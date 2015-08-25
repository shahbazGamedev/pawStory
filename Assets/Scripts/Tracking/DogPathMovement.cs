/**
Script Author : Vaikash
Description   : Dog Movement on Path
**/

using UnityEngine;
using System.Collections.Generic;

public class DogPathMovement : MonoBehaviour
{
    #region Variables

    public GameObject dogRef;
    public bool followPath;
    public Vector3 target;
    public bool reachedPathEnd;
    public float dogSpeed;
    public bool reachedTarget;
    public int nodeCount;
    public int currentNode;
    public float minDistToReach;

    List<Vector3> pathData;
    Vector3 currentposition;
    Vector3 pathEnd;
    Animator dogAnim;
    Rigidbody dogRB;

    // Event System
    public delegate void DogPathMove();
    public static event DogPathMove PathEnd;
    public static event DogPathMove TargetReached;

    #endregion Variables

    public void Awake()
    {
        dogAnim = dogRef.GetComponent<Animator>();
        dogRB = dogRef.GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
        pathData = new List<Vector3>();
        reachedPathEnd = false;
        nodeCount = 0;
        currentNode = 0;
    }

    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update position if flag set true
        if (followPath)
        {
            currentposition = transform.position;
            FollowPath();
        }
    }

    // Set path data and reset parameter for each swipe
    public void SetPathData(List<Vector3> data)
    {
        pathData = data;
        nodeCount = data.Count;
        reachedPathEnd = false;
        reachedTarget = false;
        currentNode = 0;
        pathEnd = pathData[nodeCount - 1];
    }

    // Set followPath flag
    public void EnableDogPathMovement(bool enable)
    {
        followPath = enable;
        target = pathData[0];
        target.y = transform.position.y;
        dogAnim.SetBool("Sniff", true);
    }

    // Move dog on the set path
    void FollowPath()
    {
        if (currentNode < (nodeCount))
        {
            if (nodeCount == 0)
            {
                Debug.Log("Path Data is Empty");
            }
            else
            {
                // Update target once target is reached
                if ((transform.position - target).sqrMagnitude <= minDistToReach)
                {
                    target = pathData[currentNode];
                    target.y = currentposition.y;
                    pathEnd.y = currentposition.y;
                    currentNode += 1;
                }
            }
        }

        // Update position using rigidbody
        dogRB.MovePosition(Vector3.MoveTowards(transform.position, target, dogSpeed * Time.deltaTime));

        // Update dog rotation based on target
        if (!(Vector3.Distance(transform.position, target) < 0.015f))
        {
            transform.LookAt(target);
        }

        // Check if dog reached path end
        if (currentNode >= nodeCount-2)
        {
            if (Vector3.Distance(pathEnd, transform.position) < 0.5f)
            {
                reachedPathEnd = true;
                followPath = false;
                dogAnim.SetBool("Sniff", false);
                if (PathEnd != null)
                    PathEnd();
            }
        }
    }

    // Update score if dog collides with the target and stop
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Finish")
        {
            followPath = false;
            reachedTarget = true;
            dogAnim.SetBool("Sniff", false);
            if (TargetReached != null)
                TargetReached();
        }
    }
}