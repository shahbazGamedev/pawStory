/**
Script Author : Vaikash 
Description   : Dog Movement using Waypoint System
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitMovement : MonoBehaviour {
	
	public List<Transform> waypoints;
	public List<Transform> circlePivot;

	public Transform target;
	Vector3 targetPosition;
	Vector3 circleCenter;
	float xComponent;
	float zComponent;
	public float angle;

	public bool gameStart;

	bool reachedTarget;
	int checkPointTracker;
	Rigidbody dogRigidBody;

	public float dogSpeed;
	public float dogAngularSpeed;

	public bool circularPath;

	public float radius;

	// Use this for initialization
	void Start () 
	{
		findObjectReference ();
		checkPointTracker = 0;
		gameStart = true;
		reachedTarget = true;
		dogRigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(gameStart)
		{
			if(reachedTarget)
			{
				target=waypoints[checkPointTracker];


				if (checkPointTracker == 2)
				{
					//checkPointTracker += 1;
					target=waypoints[checkPointTracker+1];
					angle = 0f;
					circularPath = true;
					circleCenter = circlePivot [1].position;
					//circleCenter = (waypoints [checkPointTracker - 1] - waypoints [checkPointTracker + 1] )* 0.5f;
					radius = transform.position.z - circleCenter.z;
				}
				else if(checkPointTracker == 5)
				{
					//checkPointTracker += 1;
					target=waypoints[0];
					angle = 0f;
					circularPath = true;
					circleCenter = circlePivot [0].position;
					radius = transform.position.z - circleCenter.z;
				}
				else
					circularPath = false;
				
				if(circularPath)
					checkPointTracker += 2;
				else
					checkPointTracker += 1;
				if (checkPointTracker > waypoints.Count - 1)
					checkPointTracker = 0;
				
				reachedTarget = false;
			}


			target.position = new Vector3(target.position.x,transform.position.y,target.position.z);

			// Update position using rigidbody
			if(!circularPath)
			{
				dogRigidBody.MovePosition (Vector3.MoveTowards (transform.position, target.position, dogSpeed * Time.deltaTime));

				// Update dog rotation based on target
				if(!(Vector3.Distance(transform.position, target.position)<2f))
				{
					transform.LookAt(target);
				}
			}

			else
			{
				angle -= Time.deltaTime * dogAngularSpeed;
				xComponent = circleCenter.x + Mathf.Sin (angle*0.0174f) * radius;
				zComponent = circleCenter.z + Mathf.Cos (angle*0.0174f) * radius;

				targetPosition = new Vector3 (xComponent, transform.position.y, zComponent);

				dogRigidBody.MovePosition(Vector3.MoveTowards(transform.position, targetPosition , dogSpeed* Time.deltaTime));
				transform.LookAt(targetPosition);
			}



			// Check if dog reached path end
			if(Vector3.Distance(target.position, transform.position)<0.2f)
			{
				reachedTarget = true;
			}
		}
	}

	void findObjectReference()
	{
		foreach(Transform child in GameObject.FindGameObjectWithTag ("Waypoint").transform)
		{
			waypoints.Add (child);
		}
		foreach(Transform child in GameObject.FindGameObjectWithTag ("CircleCenter").transform)
		{
			circlePivot.Add (child);
		}
	}


}
