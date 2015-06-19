/**
Script Author : Vaikash 
Description   : Dog Movement on Path
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DogPathMovement : MonoBehaviour {
	public GameObject dogRef;
	List<Vector3> pathData;
	public bool followPath;

	Vector3 currentposition;
	public Vector3 target;

	public bool reachedPathEnd;
	Vector3 pathEnd;
	public float dogSpeed;


	private Animator dogAnim;

	public int nodeCount;
	public int currentNode;

	// Use this for initialization
	void Start () 
	{
		dogAnim = dogRef.GetComponent<Animator>();
		pathData=new List<Vector3>();
		reachedPathEnd=false;
		nodeCount=0;
		currentNode=0;
	}

	void Update()
	{
		// Animation state update
		if(reachedPathEnd)
		{
			dogAnim.SetFloat("Speed", 0f);
			followPath=false;
		}
		else if(reachedPathEnd==false && followPath==true)
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
		pathData=data;
		nodeCount=data.Count;
		reachedPathEnd=false;
		currentNode=0;
		pathEnd=pathData[nodeCount-1];
	}


	// Set followPath flag
	public void EnableDogPathMovement(bool enable)
	{
		followPath=enable;
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
				if((transform.position-target).sqrMagnitude<=0.5f)
				{
					target=pathData[currentNode];
					target.y=currentposition.y;
					pathEnd.y=currentposition.y;
					currentNode+=1;
				}
			}
		}

		// Update position using rigidbody
		GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, target, dogSpeed* Time.deltaTime));

		// Update dog rotation based on target
		if(!(Vector3.Distance(transform.position,target)<1f))
		{
			transform.LookAt(target);
		}

		// Check if dog reached path end
		if(Vector3.Distance(pathEnd, transform.position)<0.1f)
		{
			reachedPathEnd=true;
		}
	}

}
