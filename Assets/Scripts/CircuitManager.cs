/**
Script Author : Vaikash 
Description   : Dog circuit movement
**/

using UnityEngine;
using System.Collections;

public class CircuitManager : MonoBehaviour 
{
	[Tooltip("Ellipse MajorAxis - position.x")] 
	public Transform a;
	[Tooltip("Ellipse MinorAxis - position.y")]
	public Transform b;
	public float alpha;
	public float centerX;
	public float centerY;
	public float moveSpeed;

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
		x = centerX + a.transform.position.x * Mathf.Cos(alpha*0.0174f);
		y = centerY + b.transform.position.z * Mathf.Sin(alpha*0.0174f);

		// Calculate LookAt Position
		forwardAlpha=alpha+forwardDist;

		if(forwardAlpha>=360)
			forwardAlpha-=360;

		xForward = centerX + a.transform.position.x * Mathf.Cos(forwardAlpha*0.0174f);
		yForward = centerY + b.transform.position.z * Mathf.Sin(forwardAlpha*0.0174f);

		currentPosition=transform.position;

		// Dog Movement
		GetComponent<Rigidbody>().MovePosition(new Vector3(x,currentPosition.y,y));
		transform.LookAt(new Vector3(xForward, currentPosition.y, yForward));
	}

}
