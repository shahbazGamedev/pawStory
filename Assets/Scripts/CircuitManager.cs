/**
Script Author : Vaikash 
Description   : Dog circuit movement
**/

using UnityEngine;
using System.Collections;

public class CircuitManager : MonoBehaviour 
{

	public float a;
	public float b;
	public float alpha;
	public float centerX;
	public float centerY;
	public float moveSpeed;

	public Vector3 focalPoint1;
	public Vector3 focalPoint2;

	public float forwardDist;
	public Vector3 forwardDirection;

	public Transform target;
	Vector3 currentPosition;
	DogManager dgManager;

	float x;
	float y;

	float xForward;
	float yForward;
	float forwardAlpha;

	// Use this for initialization
	void Start () 
	{
		target=this.gameObject.transform;
		dgManager=GetComponent<DogManager>();
		dgManager.isCircuitRun=true; // Added tp override dog idle animation
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update ellipse angle
		if(alpha>=360)
			alpha=0;

		alpha+=Time.deltaTime*moveSpeed;

		// Calculate current position on ellipse
		x = centerX + a * Mathf.Cos(alpha*0.0174f);
		y = centerY + b * Mathf.Sin(alpha*0.0174f);

		// Calculate LookAt Position
		forwardAlpha=alpha+forwardDist;

		if(forwardAlpha>=360)
			forwardAlpha-=360;

		xForward = centerX + a * Mathf.Cos(forwardAlpha*0.0174f);
		yForward = centerY + b * Mathf.Sin(forwardAlpha*0.0174f);

		currentPosition=transform.position;

		if(!dgManager.jump)
		{
		
			GetComponent<Rigidbody>().MovePosition(new Vector3(x,currentPosition.y,y));
			transform.LookAt(new Vector3(xForward, currentPosition.y, yForward));

		}

		// Jump implementation
		else if(dgManager.jump)
		{

			GetComponent<Rigidbody>().MovePosition(new Vector3(x,currentPosition.y,y));
			transform.LookAt(new Vector3(xForward, currentPosition.y, yForward)); // Added to make the dog look the circuit while jumping..

		}

	}

}
