/**
Script Author : Vaikash 
Description   : Dog circuit movement - Elliptical Path
**/

using UnityEngine;
using System.Collections;

public class EllipseMovement : MonoBehaviour 
{
	#region Variables

	public CircuitLaneData[] circuitLaneData;

	public bool updatePos;
	public float alpha;
	public float centerX;
	public float centerY;
	public float moveSpeed;
	public float alphaFactor;
	public bool isGrounded;
	public float forwardDist;
	public Vector3 forwardDirection;
	public float laneChangeSpeed;
	public float damping;

	Vector3 target;
	Vector3 currentPosition;

	Vector3 majorAxis;
	Vector3 minorAxis;

	Animator dogAnim;

	float x;
	float y;
	float xForward;
	float yForward;
	float forwardAlpha;
	int currentLane; // starts from 0

	[System.Serializable]
	public struct CircuitLaneData {
		public string name;
		public int LaneID;
		[Tooltip("Ellipse MajorAxis - position.x")] 
		public Transform majorAxis;
		[Tooltip("Ellipse MinorAxis - position.y")]
		public Transform minorAxis;
	}

	// Event to broadcast lane change complete to listeners
	public delegate void DogMovement();
	public static event DogMovement LaneChangeComplete;
	public static event DogMovement DogJustMoved;
	public static event DogMovement LapTriggered;

	#endregion

	// Use this for initialization
	void Start () 
	{
		target=gameObject.transform.position;
		dogAnim = GetComponent<Animator> ();
		ResetLane ();
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

		// Calculate current position on ellipse based on angle(radians)
		x = centerX + majorAxis.x * Mathf.Cos (alpha * 0.0174f);
		y = centerY + minorAxis.z * Mathf.Sin (alpha * 0.0174f);

		// Calculate LookAt Position
		forwardAlpha=alpha+forwardDist;

		xForward = centerX + majorAxis.x * Mathf.Cos (forwardAlpha * 0.0174f);
		yForward = centerY + minorAxis.z * Mathf.Sin (forwardAlpha * 0.0174f);

		currentPosition=transform.position;
		target=new Vector3(x, currentPosition.y, y);
		forwardDirection=new Vector3(xForward, currentPosition.y, yForward);

		// Dog Movement
		GetComponent<Rigidbody>().MovePosition (Vector3.MoveTowards (transform.position, target, moveSpeed * Time.deltaTime));
		transform.LookAt(forwardDirection);

		if (DogJustMoved != null)
			DogJustMoved ();
	}

	// Reset lane at start of game
	public void ResetLane()
	{
		currentLane = 1;
		majorAxis = circuitLaneData [currentLane].majorAxis.transform.position;
		minorAxis = circuitLaneData [currentLane].minorAxis.transform.position;
	}

	// Fire event from outside
	public void FireLapTriggeredEvent()
	{
		if(LapTriggered!=null)
			LapTriggered ();
	}

	#region Coroutines

	// Change lane dog in moving
	public IEnumerator ChangeLane(int targetLane)
	{
		Debug.Log (targetLane);
		if(targetLane < 0 || targetLane >= circuitLaneData.Length)
		{
			// Do nothing
		}
		else
		{
			while(majorAxis!=circuitLaneData [targetLane].majorAxis.transform.position 
				|| minorAxis!=circuitLaneData [targetLane].minorAxis.transform.position )
			{
				yield return new WaitForFixedUpdate ();
				// code for changing lane
				majorAxis = Vector3.MoveTowards (majorAxis, circuitLaneData [targetLane].majorAxis.transform.position, laneChangeSpeed * Time.deltaTime);
				minorAxis = Vector3.MoveTowards (minorAxis, circuitLaneData [targetLane].minorAxis.transform.position, laneChangeSpeed * Time.deltaTime);
			}
		}

		if (LaneChangeComplete != null)
			LaneChangeComplete ();
		
		yield return null;
	}

	#endregion
		
	#region triggerCallbacks

	void OnTriggerStay()
	{
		isGrounded=true;
	}

	void OnTriggerExit()
	{
		isGrounded=false;
	}

	#endregion
}
