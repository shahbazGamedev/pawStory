using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObedienceManager : MonoBehaviour {
	
	public Text instructions;
	public int range;
	bool gameOn;
	bool catchUserInput;
	int randomNumber;
	public float instructionWaitTime;
	bool combo;
	bool isCoroutineON;
	bool isPointerDown;
	public float holdTime;
	float swipeDelta;

	Vector2 startPoint;
	Vector2 endPoint;
	List<Vector2> swipeData;
	SwipeRecognizer.TouchPattern pattern;
	SwipeRecognizer.TouchPattern presentGesture;
	GameObject dogRef;
	DogManager dogManager;
	Vector3 startPosition;
	Quaternion startRotation;

	[System.Serializable]
	public struct GestureCollection {
		public string key;
		public SwipeRecognizer.TouchPattern value1;
		public SwipeRecognizer.TouchPattern value2;
	}

	public GestureCollection[] gestureCollection;

	// Use this for initialization
	void Start () {
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		dogManager = dogRef.GetComponent <DogManager> ();
		swipeData = new List<Vector2> ();	
		pattern = SwipeRecognizer.TouchPattern.reset;
		gameOn = true;
		startPosition = dogRef.transform.position;
		startRotation = transform.rotation;
		StartCoroutine (Instruct ());
		combo = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(catchUserInput)
		{
			// Code executes when swipe is identified
			if (pattern != SwipeRecognizer.TouchPattern.reset) 
			{
				CheckUserInput ();
			}
		}

		// Touch Timer for hold down gesture
		if (isPointerDown)
			holdTime += Time.deltaTime;
		
		SyncAnimation ();
	}

	public void OnBeginDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		startPoint = pointData.position;
	}

	public void OnEndDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		endPoint = pointData.position;
		if (!combo) 
		{
			SwipeRecognizer.RecogonizeSwipe (startPoint, endPoint, swipeData, out pattern);
		}
		swipeData.Clear ();
	}

	public void OnDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeData.Add(pointData.position);
		//swipeDelta += (Vector2.Distance (startPoint, pointData.position) / Screen.dpi) * 160;
	}

	void SwipeReset()
	{
		pattern = SwipeRecognizer.TouchPattern.reset;
	}

	public void OnClickEnd(BaseEventData data)
	{
		var pointData = (PointerEventData)data;

		// check for tap 
		if (swipeData.Count <= 1)
		{
			// check for double tap
			if (pointData.clickCount == 2) 
			{
				pattern = SwipeRecognizer.TouchPattern.doubleTap;
				//Debug.Log ("dTap");
			}
			else 
			{
				pattern = SwipeRecognizer.TouchPattern.singleTap;
				//Debug.Log ("sTap");
			}
		}
	}

	public void OnPointerDown(BaseEventData data)
	{
		isPointerDown = true;

	}

	public void OnPointerUp(BaseEventData data)
	{
		isPointerDown = false;
		holdTime = 0;
	}

	// Checks non combo conditions
	void CheckUserInput()
	{
		// Seperate routines for combos and non combos!
		if (randomNumber != 2 && randomNumber != 3 && randomNumber != 4 && randomNumber != 5) 
		{
			if (pattern == presentGesture)
			{
				Debug.Log ("Success!!");
				catchUserInput = false;
				SwipeReset ();
				//StartCoroutine (dogManager.MoveToPosition (startPosition, startRotation));
			} 
			else 
			{
				Debug.Log ("Try Again");
				SwipeReset ();
			}
		} 
		else if (!isCoroutineON) 
		{
			isCoroutineON = true;
			StartCoroutine (CatchCombos ());
		}
	}

	//  Checks combo conditions
	IEnumerator CatchCombos()
	{
		while (catchUserInput)
		{
			yield return new WaitForFixedUpdate ();
			if (pattern == presentGesture && !combo) 
			{
				combo = true;
				SwipeReset ();
				StartCoroutine (DetectHold ());
			}
			if (combo)
			{
				if (pattern == gestureCollection [randomNumber].value2) 
				{
					Debug.Log ("Combo Success");
					catchUserInput = false;
					combo = false;
					SwipeReset ();
				}
			}
		}
		isCoroutineON = false;
		yield return null;
	}
		
	// Sets random instruction at fixed intervals 
	IEnumerator Instruct()
	{
		while(gameOn) 
		{
			randomNumber = Random.Range (0, range);
			//randomNumber = 9;
			presentGesture = gestureCollection[randomNumber].value1;
			instructions.text = gestureCollection[randomNumber].key;
			catchUserInput = true;
			SwipeReset ();
			yield return new WaitForSeconds (instructionWaitTime);
		}
		yield return null;
	}

	// Checks for touch pattern hold and move - only when combo is true
	IEnumerator DetectHold()
	{
		//Debug.Log ("i m alive");
		while (catchUserInput) 
		{
			yield return new WaitForFixedUpdate ();
			if(isPointerDown)
			{
				// Min. hold time of 2 secs
				if(holdTime>1.5f && swipeData.Count<1)
				{
					pattern = SwipeRecognizer.TouchPattern.hold;
					holdTime = 0;
				}
				else if(swipeData.Count>1)
				{
					pattern = SwipeRecognizer.TouchPattern.move;
				}
			}
		}
		//Debug.Log ("i m dead");
		yield return null;
	}

	// Sync animation of dog based on player input
	void SyncAnimation()
	{
		
	}
}
