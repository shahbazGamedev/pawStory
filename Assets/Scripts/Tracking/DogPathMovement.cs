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
	bool followPath;

	Vector3 currentposition;
	public Vector3 Target;
	Vector3 forwardTarget;
	public bool reachedPathEnd;
	public float lerpIncrement;

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
	
	// Update is called once per frame
	void FixedUpdate () {
		lerpIncrement+=Time.deltaTime;
		if(followPath)
		{
			currentposition=transform.position;
			FollowPath();
		}
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

	void FollowPath()
	{
		if(currentNode<(nodeCount-1) )
		{
			if(nodeCount==0)
			{
				Debug.Log("Path Data is Empty");
			}
			else
			{
				if((transform.position-Target).sqrMagnitude<1f)
				{
					lerpIncrement=0;
					currentNode+=1;
					Target=pathData[currentNode];
					Target.y=currentposition.y;
					if(currentNode<nodeCount-2)
					{
					forwardTarget=pathData[currentNode+1];
					forwardTarget.y=currentposition.y;
					}
					else
					{
						forwardTarget=(pathData[nodeCount-2]-pathData[nodeCount-1]) * 2;
						forwardTarget.y=currentposition.y;
					}
				}
				else if(currentNode==0)// yet to start the movement
				{
					Target=pathData[currentNode];
					Target.y=currentposition.y;
				}

			}
		
		}
		lerpIncrement=Mathf.Clamp(lerpIncrement, 0f, 2f);

		GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, Target, lerpIncrement/2));

		if(!(Vector3.Distance(transform.position,Target)<1f))
		{
			transform.LookAt(forwardTarget);
		}

		if(reachedPathEnd)
		{
			dogAnim.SetFloat ("Speed",0f);
			followPath=false;
		}
		if(reachedPathEnd==false && followPath==true)
			dogAnim.SetFloat ("Speed",1f);
		if(Vector3.Distance(pathData[nodeCount-1], transform.position)<0.5f)
		{
			reachedPathEnd=true;
		}
	}
}
