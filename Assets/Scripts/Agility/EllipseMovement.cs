/**
Script Author : Vaikash 
Description   : Dog circuit movement - Elliptical Path
**/

using UnityEngine;
using System.Collections;

public class EllipseMovement : MonoBehaviour 
{
	#region Variables

	[Tooltip("Ellipse MajorAxis - position.x")] 
	public Transform a;
	[Tooltip("Ellipse MinorAxis - position.y")]
	public Transform b;
	public bool updatePos;
	public float alpha;
	public float centerX;
	public float centerY;
	public float moveSpeed;
	public float alphaFactor;

	public float forwardDist;
	public Vector3 forwardDirection;

	Vector3 target;
	Vector3 currentPosition;
	DogManager dgManager;
	Animator dogAnim;

	float x;
	float y;

	float xForward;
	float yForward;
	float forwardAlpha;

	#endregion

	// Use this for initialization
	void Start () 
	{
		target=gameObject.transform.position;
		dgManager = GetComponent<DogManager> ();
		dogAnim = GetComponent<Animator> ();
//		dgManager.isCircuitRun=true; // Added tp override dog idle animation
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (updatePos) 
		{
			dogAnim.SetFloat ("Speed", 1);
			DogTrackMove ();
		}
		else
		{
			dogAnim.SetFloat ("Speed", 0);
		}
	}

	// Update dog position in a elliptical path
	void DogTrackMove()
	{
		// Update ellipse angle
		alpha += Time.deltaTime * alphaFactor;

//		if(alpha>=360)
//			alpha=0;

		// Calculate current position on ellipse based on angle(radians)
		x = centerX + a.transform.position.x * Mathf.Cos (alpha * 0.0174f);
		y = centerY + b.transform.position.z * Mathf.Sin (alpha * 0.0174f);

		// Calculate LookAt Position
		forwardAlpha=alpha+forwardDist;

//		if(forwardAlpha>=360)
//			forwardAlpha-=360;

		xForward = centerX + a.transform.position.x * Mathf.Cos (forwardAlpha * 0.0174f);
		yForward = centerY + b.transform.position.z * Mathf.Sin (forwardAlpha * 0.0174f);

		currentPosition=transform.position;
		target=new Vector3(x, currentPosition.y, y);
		forwardDirection=new Vector3(xForward, currentPosition.y, yForward);

		// Dog Movement
		GetComponent<Rigidbody>().MovePosition (Vector3.MoveTowards (transform.position, target, moveSpeed * Time.deltaTime));
		transform.LookAt(forwardDirection);
		//Debug.Log (transform.rotation.eulerAngles);
	}
}
