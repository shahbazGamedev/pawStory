using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObedienceManager : MonoBehaviour {
	
	public Text instructions;
	public GameObject gameOverPannel;
	public GameObject gestureMat;

	public int range;
	bool gameOn;
	bool catchUserInput;
	int randomNumber;
	public float instructionWaitTime;
	bool combo;
	bool isCoroutineON;

	int chance;
	public int maxChances;
	int points;
	public int scoreIncrement;

	Animator dogAnim;

//	Vector2 startPoint;
//	Vector2 endPoint;

	SwipeRecognizer.TouchPattern pattern;
	SwipeRecognizer.TouchPattern presentGesture;
	SwipeRecognizer.TouchPattern gestureCache;

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

	[System.Serializable]
	public struct SwipeDataCollection {
		public string name;
		public int touchID;
		public bool isActive;
		public float holdTime;
		public Vector2 startPoint;
		public Vector2 endPoint;
		public List<Vector2> swipeData;
	}

	public SwipeDataCollection[] swipeDataCollection;

	// Use this for initialization
	void Start () {
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		dogManager = dogRef.GetComponent <DogManager> ();
		//swipeData = new List<Vector2> ();	
		pattern = SwipeRecognizer.TouchPattern.reset;
		gameOn = true;
		startPosition = dogRef.transform.position;
		startRotation = transform.rotation;
		StartCoroutine (Instruct ());
		combo = false;
		dogAnim = dogRef.GetComponentInChildren<Animator> ();
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
		if (swipeDataCollection [1].isActive)
			swipeDataCollection [1].holdTime += Time.deltaTime;
		if (swipeDataCollection [2].isActive)
			swipeDataCollection [2].holdTime += Time.deltaTime;
		
		SyncAnimation ();

	}

	// Checks non combo conditions
	void CheckUserInput()
	{
		// Seperate routines for combos and non combos!
		//if (randomNumber != 2 && randomNumber != 3 && randomNumber != 4 && randomNumber != 5) 
		if (randomNumber != 10) 
		{
			if (pattern == presentGesture)
			{
				//Debug.Log ("Success!!");
				instructions.text="Excellent";
				catchUserInput = false;
				points += scoreIncrement;
				gestureCache = pattern;
				DeactivateGestureMat ();
				//StartCoroutine (dogManager.MoveToPosition (startPosition, startRotation));
			} 
			else // if wrong gesture
			{
				if(randomNumber==4)
				{
					if (pattern == SwipeRecognizer.TouchPattern.singleTap)
						return;
				}
				Debug.Log (pattern);
				instructions.text="Wrong";
				DeactivateGestureMat ();
			}
		} 
		else if (!isCoroutineON) // Prevent stacking of coroutine
		{
			isCoroutineON = true;
			StartCoroutine (CatchCombos ());
		}
	}

	// Sync animation of dog based on player input
	void SyncAnimation()
	{
		switch(gestureCache)
		{
		case SwipeRecognizer.TouchPattern.reset:
			{
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeUp:
			{
				dogAnim.SetTrigger ("Stand");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeDown:
			{
				dogAnim.SetTrigger ("Sit");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeUpLeft:
			{
				dogAnim.SetTrigger ("Jump");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeUpRight:
			{
				dogAnim.SetTrigger ("Jump");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.clockwiseCircle:
			{
				dogAnim.SetTrigger ("ClockWise");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.antiClockwiseCircle:
			{
				dogAnim.SetTrigger ("AntiClockWise");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.doubleCircle:
			{
				dogAnim.SetTrigger ("TailChase");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.doubleTap:
			{
				dogAnim.SetBool ("StandOn", true);
				StartCoroutine (ResetAnimBool ("StandOn", 2));
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.hold:
			{
				dogAnim.SetTrigger ("Jump");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		}
	}

	// Disable touchInput
	public void DeactivateGestureMat()
	{
		catchUserInput = false;
		//isPointerDown = false;
		gestureMat.SetActive (false);
		SwipeReset ();
	}

	// Reset previous touch input
	void SwipeReset()
	{
		pattern = SwipeRecognizer.TouchPattern.reset;
	}


	// Reset particular swipe data
	void dataReset(int pointerID)
	{
		swipeDataCollection [pointerID].swipeData.Clear ();

	}

	#region Coroutines
	//  Checks combo conditions
	IEnumerator CatchCombos()
	{
		while (catchUserInput)
		{
			yield return new WaitForFixedUpdate (); // Wait for fixed update
			if (pattern != SwipeRecognizer.TouchPattern.reset)
			{
				if (combo) 
				{
					if (pattern == gestureCollection [randomNumber].value2) 
					{
						//Debug.Log ("Combo Success");
						instructions.text = "2x Combo Success";
						points += scoreIncrement;
						catchUserInput = false;
						combo = false;
						DeactivateGestureMat ();
					} 
					else // if wrong gesture 
					{
						instructions.text = "Wrong";
						DeactivateGestureMat ();
					}
				}
				else  
				{
					if (pattern == presentGesture && !combo)
					{
						instructions.text = "1x Success";
						combo = true;
						SwipeReset ();
						StartCoroutine (DetectHold ());
					}
					else // if wrong gesture
					{
						instructions.text = "Wrong";
						DeactivateGestureMat ();
					}

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
			chance += 1;

			if (chance > maxChances) // if chances depleted gameOver
			{
				DeactivateGestureMat ();
				gameOn = false;
				gameOverPannel.SetActive (true);
				instructions.text = "Score: " + points + " / " + maxChances;
			}
			else  // else put a random instruction
			{
				randomNumber = Random.Range (0, range);
				//randomNumber = 8;
				presentGesture = gestureCollection [randomNumber].value1;
				instructions.text = gestureCollection [randomNumber].key;

				SwipeReset (); // reset previous swipe data
				catchUserInput = true;

				gestureMat.SetActive (true); // Turn on user input if off

				// reset hold time
				swipeDataCollection [1].holdTime = 0; 
				swipeDataCollection [2].holdTime = 0;

				if(randomNumber==5)
				{
					StartCoroutine (DetectHold ());
				}

				yield return new WaitForSeconds (instructionWaitTime);
			}

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
			if(swipeDataCollection [1].isActive && !swipeDataCollection [2].isActive)
			{
				// Min. hold time of 1 sec
				if(swipeDataCollection [1].holdTime>1 && swipeDataCollection [1].swipeData.Count<1)
				{
					pattern = SwipeRecognizer.TouchPattern.hold;
					swipeDataCollection [1].holdTime = 0;
				}
				else if(swipeDataCollection [1].swipeData.Count>1)
				{
					pattern = SwipeRecognizer.TouchPattern.move;
				}
			}
		}
		//Debug.Log ("i m dead");
		yield return null;
	}

	// Resets animation state after wait time
	IEnumerator ResetAnimBool(string animName, int wait)
	{
		yield return new WaitForSeconds (wait);
		dogAnim.SetBool (animName, false);
	}
	#endregion

	#region EventTriggers
	// Event trigger - BeginDrag
	public void OnBeginDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		//swipeDataCollection [-pointData.pointerId].isActive = true;
		swipeDataCollection [-pointData.pointerId].startPoint= pointData.position;

		//		if(pointData.pointerId==-1)
		//			startPoint = pointData.position;
	}

	// Event trigger - EndDrag
	public void OnEndDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		//swipeDataCollection [-pointData.pointerId].isActive = false;
		swipeDataCollection [-pointData.pointerId].endPoint= pointData.position;

		if(pointData.pointerId==-1 && !swipeDataCollection [2].isActive) 
		{
			//			endPoint = pointData.position;
			if (!combo) 
			{
				SwipeRecognizer.RecogonizeSwipe (swipeDataCollection [-pointData.pointerId], out pattern);
			}
			//			swipeData.Clear ();
		}
		dataReset (-pointData.pointerId);
	}

	// Event trigger - Drag
	public void OnDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeDataCollection [-pointData.pointerId].swipeData.Add (pointData.position);

		//		if(pointData.pointerId==-1)
		//		{
		//			swipeData.Add(pointData.position);
		//			//swipeDelta += (Vector2.Distance (startPoint, pointData.position) / Screen.dpi) * 160;	
		//		}
	}

	// Event trigger - Pointer Click
	public void OnClickEnd(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if (pointData.pointerId == -1 && !swipeDataCollection [2].isActive) 
		{
			// check for tap 
			if (swipeDataCollection [-pointData.pointerId].swipeData.Count <= 1) 
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
	}

	// Event Trigger - Pointer Down
	public void OnPointerDown(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeDataCollection [-pointData.pointerId].isActive = true;
		//Debug.Log (pointData.pointerId);
	}

	// Event Trigger - Pointer Up
	public void OnPointerUp(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeDataCollection [-pointData.pointerId].isActive = false;
		swipeDataCollection [-pointData.pointerId].holdTime = 0;
	}
	#endregion

	#region ButtonCallbacks
	// Back button
	public void GoBack()
	{
		Application.LoadLevel ("MainMenu");
	}

	// Replay button
	public void PlayAgain()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
	#endregion
}
