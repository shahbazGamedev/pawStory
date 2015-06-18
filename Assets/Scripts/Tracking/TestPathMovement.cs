/**
Script Author : Vaikash 
Description   : Dog Movement on Path
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestPathMovement : MonoBehaviour {
	public GameObject dogRef;
	List<Vector3> pathData;
	bool followPath;
	
	Vector3 currentposition;
	public Vector3 target;

	public bool reachedPathEnd;

	
	private Animator dogAnim;
	
	public int nodeCount;
	public int currentNode;

	public float speedFactor;
	// Use this for initialization
	void Start () 
	{
		dogAnim = dogRef.GetComponent<Animator>();
		pathData=new List<Vector3>();
		reachedPathEnd=false;
		nodeCount=0;
		currentNode=0;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		currentposition=transform.position;
		IterateWaypoints();
	}

	public void SetPathData(List<Vector3> data)
	{
		pathData=data;
		nodeCount=data.Count;
		reachedPathEnd=false;
		currentNode=0;
	}
	
	public void EnableDogPathMovement(bool enable)
	{
		followPath=enable;
	}

	void IterateWaypoints()
	{
		if(followPath)
		{
			currentNode+=1;
			if(currentNode>(nodeCount-1))
				currentNode=nodeCount-1;
			target=new Vector3(pathData[currentNode].x,currentposition.y,pathData[currentNode].z);
			GetComponent<Rigidbody>().MovePosition(target);

		}
		if(!(Vector3.Distance(transform.position,target)<1f))
		{
			transform.LookAt(target);
		}

	}
}
