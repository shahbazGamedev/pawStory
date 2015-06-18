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
	bool reachedPathEnd;
	public float moveSpeed;

	private Animator dogAnim;

	int nodeCount;
	int currentNode;

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
	void Update () {
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
					currentNode+=1;
					Target=pathData[currentNode];
					Target.y=currentposition.y;
					if(currentNode<nodeCount-2)
					{
					forwardTarget=pathData[currentNode+2];
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
				GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, Target, moveSpeed));
//				if(currentNode>nodeCount-2)
//				{
//
//				}
//				else
					transform.LookAt(forwardTarget);
			}

		}
		if(reachedPathEnd)
		{
			dogAnim.SetFloat ("Speed",0f);
			followPath=false;
		}
		if(reachedPathEnd==false && followPath==true)
			dogAnim.SetFloat ("Speed",1f);
	}
}
