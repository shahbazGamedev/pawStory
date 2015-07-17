/**
Script Author : Vaikash 
Description   : Dog Movement on Path
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DogPathMovement : MonoBehaviour {
	public GameObject dogRef;
	List<Vector3> pathData;
	public bool followPath;

	Vector3 currentposition;
	public Vector3 target;

	public bool reachedPathEnd;
	Vector3 pathEnd;
	public float dogSpeed;
	public bool reachedTarget;

	Animator dogAnim;

	public int nodeCount;
	public int currentNode;

	public float minDistToReach;
	TrackingManager trackingManagerRef;

	// Use this for initialization
	void Start () 
	{
		dogAnim = dogRef.GetComponent<Animator>();
		pathData=new List<Vector3>();
		reachedPathEnd=false;
		nodeCount=0;
		currentNode=0;
		trackingManagerRef = FindObjectOfType<TrackingManager> ();
	}

	void Update()
	{
		// Animation state update
		if(reachedPathEnd)
		{
			dogAnim.SetFloat("Speed", 0f);
			followPath=false;
		}
		else if(!reachedPathEnd && followPath)
		{
			dogAnim.SetFloat("Speed", 1f);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		// Update position if flag set true
		if(followPath)
		{
			currentposition=transform.position;
			FollowPath();
		}
	}

	// Set path data and reset parameter for each swipe
	public void SetPathData(List<Vector3> data)
	{
		pathData = data;
		nodeCount = data.Count;
		reachedPathEnd = false;
		currentNode = 0;
		pathEnd = pathData [nodeCount - 1];
	}


	// Set followPath flag
	public void EnableDogPathMovement(bool enable)
	{
		followPath=enable;
		target = pathData [0];
		target.y = transform.position.y;
	}

	// Move dog on the set path
	void FollowPath()
	{
		if(currentNode<(nodeCount) )
		{
			if(nodeCount==0)
			{
				Debug.Log("Path Data is Empty");
			}
			else
			{
				// Update target once target is reached
				if((transform.position-target).sqrMagnitude<=minDistToReach)
				{
					target = pathData [currentNode];
					target.y = currentposition.y;
					pathEnd.y = currentposition.y;
					currentNode += 1;
				}
			}
		}

		// Update position using rigidbody
		GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, target, dogSpeed* Time.deltaTime));

		// Update dog rotation based on target
		if(!(Vector3.Distance(transform.position,target)<0.01f))
		{
			transform.LookAt(target);
		}

		// Check if dog reached path end
		if(Vector3.Distance(pathEnd, transform.position)<0.001f)
		{
			reachedPathEnd=true;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.transform.tag=="Finish") 
		{
			followPath = false;
			reachedTarget = true;
			trackingManagerRef.points += 1;
			trackingManagerRef.roundComplete = true;
			Debug.Log ("Reached Target");
		}
	}

}
