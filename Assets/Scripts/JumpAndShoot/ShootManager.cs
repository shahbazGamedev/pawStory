/**
Script Author : Vaikash 
Description   : Throw Food onto basket
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
    public Food foodRef;

	bool tap;
	bool jump;

	public float jumpFactor;
	Vector3 ballVelocity;
	Vector3 startPosition;

	public Vector3 jumpForce;
	public Transform spawnPoint;

	public float swipeLength;

	// Use this for initialization
	void Start () {
		swipeData = new List<Vector2> ();	
		swipe.pattern = SwipeRecognizer.TouchPattern.reset;
		tap = false;
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(jump)
		{
			// Limit dog jump force to make it realistic
			swipeLength = Mathf.Clamp (swipe.swipeLength, 250, 1000);
			startPosition = transform.position;

			// check if dog is on ground
			if(foodRef.isGrounded)
			{
				// Jump only in forward direction
				if(swipe.swipeAngle <= 90 || swipe.swipeAngle >= 275)
				{
					//transform.rotation = Quaternion.Euler (0, swipe.swipeAngle, 0);
					jumpForce = Quaternion.Euler (0, swipe.swipeAngle, 0) * (new Vector3 (0f, 1f, 0.8f) * swipeLength * jumpFactor);
                    foodRef.Jump (jumpForce);
					jump = false;
				}
			}
		}

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
		// check for tap 
		if (swipeData.Count <= 1)
		{
				tap=true;
		}
	}

}
