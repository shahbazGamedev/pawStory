/**
Script Author : Vaikash 
Description   : Dog Shoot
**/
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ShootManager : MonoBehaviour {

	Vector2 startPoint;
	Vector2 endPoint;
	List<Vector2> swipeData;

	SwipeRecognizer.Swipe swipe;
	DogManager dogManager;

	bool tap;
	bool jump;
	bool shoot;
	public bool hasBall;
	public float jumpFactor;
	Vector3 ballVelocity;
	Vector3 startPosition;

	public Vector3 jumpForce;
	public Transform spawnPoint;
	public Transform ballPrefab;
	public Transform ballHolder;
	public float distance;
	public float actualDistance;
	float progress;

	public float boostRange;
	public float forceFactor;

	// Use this for initialization
	void Start () {
		swipeData = new List<Vector2> ();	
		swipe.pattern = SwipeRecognizer.TouchPattern.reset;
		dogManager = GetComponent <DogManager> ();
		tap = false;
		hasBall = true;
	}

	void Update()
	{
		// Check for tap each frame
		if (tap)
		{
			tap = false;

			// if dog has the ball then shoot
			if(hasBall)
				shoot = true;
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		// distance of dog from start point of dog
		actualDistance = (transform.position - startPosition).magnitude;

		if(jump)
		{
			startPosition = transform.position;

			// check if dog is on ground
			if(dogManager.isGrounded)
			{
				// Jump Code Here
				transform.rotation = Quaternion.Euler (0, swipe.swipeAngle, 0);
				jumpForce = Quaternion.Euler (0, swipe.swipeAngle, 0) * (new Vector3 (0f, 1f, 0.8f) * swipe.swipeLength * jumpFactor);
				dogManager.Jump (jumpForce);
				jump = false;
				StartCoroutine (CalcDistance ());
			}
		}

		// dog can shoot only if it has already jumped
		if(!dogManager.isGrounded)
		{
			if(shoot)
			{
				// Shoot Code Here
				var thisInstance = (Transform)Instantiate (ballPrefab, spawnPoint.position, Quaternion.identity);

				// Calc ball velocity based on angle of dog
				ballVelocity = GetComponent <Rigidbody> ().velocity;
				ballVelocity.y = CalcBallForceRatio ();

				thisInstance.root.GetComponent <Rigidbody> ().velocity = ballVelocity;
				thisInstance.parent = ballHolder;

				// dog has shot the ball
				hasBall = false;
			}
		}
		shoot = false;
	}

	public void OnBeginDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		startPoint = pointData.position;
		swipe.pattern = SwipeRecognizer.TouchPattern.reset;
	}

	public void OnEndDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		endPoint = pointData.position;

		// Recognize swipe
		SwipeRecognizer.RecogonizeSwipe (startPoint, endPoint, swipeData, out swipe, false);
		if (swipe.swipeAngle >= 0)
			jump = true;

		// clear swipe data
		swipeData.Clear ();

	}

	public void OnDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeData.Add(pointData.position);
	}

	public void OnClick(BaseEventData data)
	{
		//var pointData = (PointerEventData)data;

		// check for tap 
		if (swipeData.Count <= 1)
		{
				tap=true;
		}
	}

	// Fix for caculating velocity of dog correctly
	IEnumerator CalcDistance()
	{
		yield return new WaitForEndOfFrame ();
		distance = ( GetComponent <Rigidbody> ().velocity.sqrMagnitude) / -Physics.gravity.y;
	}

	// calculates the ratio based on progress of dog jump
	float CalcBallForceRatio()
	{
		progress = actualDistance / distance;

		// Max ball velocity at boostRange % of jump
		if (progress <= boostRange)
			return  (forceFactor * progress);
		else
			return  2.5f * (1 - progress);
	}
}
