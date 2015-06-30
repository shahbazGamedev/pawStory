/**
Script Author : Vaikash 
Description   : Swipe Recognizer
**/
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ShootManager : MonoBehaviour {

	//public Text swipeText;
	Vector2 startPoint;
	Vector2 endPoint;
	List<Vector2> swipeData;

	int touchBeginID;
	int touchEndID;

	SwipeRecognizer.Swipe swipe;
	DogManager dogManager;

	bool tap;
	bool jump;
	bool shoot;
	public bool hasBall;
	public float jumpFactor;
	public float ballSpeedFactor;

	Vector3 jumpForce;
	public Transform spawnPoint;
	public Transform ballPrefab;
	public Transform ballHolder;

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
			if(hasBall)
				shoot = true;
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(jump)
		{
			if(dogManager.isGrounded)
			{
				// Jump Code Here
				transform.rotation = Quaternion.Euler (0, swipe.swipeAngle, 0);
				jumpForce = Quaternion.Euler (0, swipe.swipeAngle, 0) * (new Vector3 (0f, 1f, 0.8f) * swipe.swipeLength * jumpFactor);
				dogManager.Jump (jumpForce);
				jump = false;
			}
		}
		if(!dogManager.isGrounded)
		{
			if(shoot)
			{
				// Shoot Code Here
//				Debug.Log ("shoot!");
				var thisInstance = (Transform)Instantiate (ballPrefab, spawnPoint.position, Quaternion.identity);
				thisInstance.root.GetComponent <Rigidbody> ().AddForce (jumpForce / ballSpeedFactor, ForceMode.Impulse);
				thisInstance.parent = ballHolder;
				hasBall = false;
			}
		}
		shoot = false;
	}

	public void OnBeginDrag (BaseEventData data)
	{
//		Debug.Log ("Begin");
		var pointData = (PointerEventData)data;
		touchBeginID = pointData.pointerId;
		startPoint = pointData.position;
		swipe.pattern = SwipeRecognizer.TouchPattern.reset;
	}

	public void OnEndDrag (BaseEventData data)
	{
//		Debug.Log ("End");
		var pointData = (PointerEventData)data;
		touchEndID = pointData.pointerId;
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
		var pointData = (PointerEventData)data;

		// check for tap 
		if (swipeData.Count <= 1)
		{
			// check for double tap
			if (pointData.clickCount > 1)
				Debug.Log ("More than single tap");
			else
				tap=true;
		}
	}
}
