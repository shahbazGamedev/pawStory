﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObedienceManager : MonoBehaviour {
	
	public Text instructions;
	public Text roundInfo;
	public Text timerNotification;
	public GameObject gameOverPanel;
	public GameObject gestureMat; // Image UI element on which event triggers are attached
	public GameObject[] directionRef;

	float timer;
	public int range;
	bool gameOn;
	bool catchUserInput; // Bool for catchUserInput coroutine
	int randomNumber;
	public float instructionWaitTime;
	bool combo;
	bool isCoroutineON; // True when catchCombos coroutine is running
	public bool registerAnimEvent; // If true listenes for trigger from idle animation
	public bool nextInstruct;
	bool isDogSiting;

	int chance;
	int points;
	public int maxChances;
	public int scoreIncrement;

	Animator dogAnim;
	public float angularVelocity;
	public float jumpForce;

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

	// Emulates hashtable in editor
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

	// Emulates hashtable in editor
	public SwipeDataCollection[] swipeDataCollection;

	// Use this for initialization
	void Start () {
		OnRestart ();
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
		Timer ();
	}

	// Checks non combo conditions
	void CheckUserInput()
	{
		// Seperate routines for combos and non combos!
		if (randomNumber != 10) 
		{
			if (pattern == presentGesture)
			{
				instructions.text="Excellent";
				catchUserInput = false;
				points += scoreIncrement;
				gestureCache = pattern;
				DeactivateGestureMat ();
			} 
			else // if wrong gesture
			{
				if(true)
				{
					if (pattern == SwipeRecognizer.TouchPattern.singleTap)
						return;
				}
				gestureCache = pattern;
				catchUserInput = false;
				instructions.text = "Wrong";
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
				nextInstruct = false;
				registerAnimEvent = true;
				dogAnim.SetTrigger ("Stand");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeDown:
			{
				nextInstruct = true;
				//registerAnimEvent = true;
				dogAnim.SetTrigger ("Sit");
				isDogSiting = true;
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeUpLeft:
			{
				nextInstruct = false;
				registerAnimEvent = true;
				//dogAnim.SetTrigger ("Jump");
				StartCoroutine (DogJump (gestureCache));
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeUpRight:
			{
				nextInstruct = false;
				registerAnimEvent = true;
				//dogAnim.SetTrigger ("Jump");
				StartCoroutine (DogJump (gestureCache));
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.clockwiseCircle:
			{
				nextInstruct = false;
				registerAnimEvent = true;
				dogAnim.SetTrigger ("ClockWise");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.antiClockwiseCircle:
			{
				nextInstruct = false;
				registerAnimEvent = true;
				dogAnim.SetTrigger ("AntiClockWise");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.doubleCircle:
			{
				nextInstruct = false;
				registerAnimEvent = true;
				dogAnim.SetTrigger ("TailChase");
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.doubleTap:
			{
				nextInstruct = false;
				registerAnimEvent = true;
				dogAnim.SetBool ("StandOn", true);
				StartCoroutine (ResetAnimBool ("StandOn", 4));
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		case SwipeRecognizer.TouchPattern.hold:
			{
				nextInstruct = false;
				registerAnimEvent = true;
				dogAnim.SetTrigger ("Jump");
				dogRef.GetComponent <Rigidbody> ().AddForce (new Vector3 (0f, 1f, 0f) * jumpForce, ForceMode.Impulse);
				gestureCache = SwipeRecognizer.TouchPattern.reset;
				break;
			}
		}
	}

	// Disable touchInput
	public void DeactivateGestureMat()
	{
		catchUserInput = false;
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

	// Timer function
	void Timer()
	{
		if (gameOn)
		{
			timer += Time.deltaTime;
			if(timer>instructionWaitTime)
			{
				timer = 5;
			}
		}
		else
		{
			timerNotification.gameObject.SetActive (false);
		}
		timerNotification.text = (int)timer + " / " + (int)instructionWaitTime;
	}

	// Flag for issuing next instruction
	void GotoNextInstruction()
	{
		nextInstruct = true;
		Debug.Log ("Fired");
	}

	// Decouple listener from event
	void OnDisable()
	{
		dogManager.ResetComplete -= GotoNextInstruction;
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
			if (chance >= maxChances) // if chances depleted gameOver
			{
				yield return new WaitForSeconds (2);
				DeactivateGestureMat ();
				gameOn = false;
				gameOverPanel.SetActive (true);
				instructions.text = "Score: " + points + " / " + maxChances;

			}
			else  // else put a random instruction
			{
				if(!registerAnimEvent)
				{
					nextInstruct = true;
				}

				// Wait till animation is complete
				yield return StartCoroutine (WailTillIdleAnimation ());

				chance += 1;
				if(randomNumber==0 && isDogSiting)
				{
					randomNumber = 1;
					isDogSiting = false;
				}
				else
				{
					int prevRandomvalue = randomNumber;
					// Generate random number that is not same as previous or 1 (prevent instruction stand before sit)
					while(randomNumber==prevRandomvalue || randomNumber==1)
					{
						randomNumber = Random.Range (0, range);
					}
				}
					
				//randomNumber = 8;
				presentGesture = gestureCollection [randomNumber].value1;
				instructions.text = gestureCollection [randomNumber].key;

				SwipeReset (); // reset previous swipe data
				catchUserInput = true;
				timer = 0;

				gestureMat.SetActive (true); // Turn on user input if off

				// reset hold time
				swipeDataCollection [1].holdTime = 0; 
				swipeDataCollection [2].holdTime = 0;

				if (randomNumber == 5) // Change condition to check hold gesture if need arises
				{
					StartCoroutine (DetectHold ());
				}
				roundInfo.text = chance + " / " + maxChances;
				nextInstruct = false;
				yield return new WaitForSeconds (instructionWaitTime);
			}

		}
		yield return null;
	}

	// Checks for touch pattern hold and move - only when combo is true
	IEnumerator DetectHold()
	{

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
					swipeDataCollection [1].isActive = false;
				}
				else if(swipeDataCollection [1].swipeData.Count>1)
				{
					pattern = SwipeRecognizer.TouchPattern.move;
					swipeDataCollection [1].isActive = false;
				}
			}
		}
		yield return null;
	}

	// Resets animation state after wait time
	IEnumerator ResetAnimBool(string animName, int wait)
	{
		yield return new WaitForSeconds (wait);
		dogAnim.SetBool (animName, false);
	}

	// Makes the dog jump in the desired direction
	IEnumerator DogJump(SwipeRecognizer.TouchPattern touchPattern)
	{
		if(touchPattern==SwipeRecognizer.TouchPattern.swipeUpLeft) // Left Jump
		{
			Quaternion targetRotation = directionRef [0].transform.rotation;
			while (dogRef.transform.rotation != targetRotation)
			{
				yield return new WaitForFixedUpdate ();
				Quaternion deltaRotation= Quaternion.RotateTowards (dogRef.transform.rotation, targetRotation, angularVelocity * Time.deltaTime);
				dogRef.transform.rotation = deltaRotation;
			}
			dogAnim.SetTrigger ("Jump");
			dogRef.GetComponent <Rigidbody> ().AddForce (targetRotation * (new Vector3 (0f, 1f, 1f)) * jumpForce, ForceMode.Impulse);
		}
		else // Right Jump
		{
			Quaternion targetRotation = directionRef [1].transform.rotation;
			while (dogRef.transform.rotation != targetRotation)
			{
				yield return new WaitForFixedUpdate ();
				Quaternion deltaRotation= Quaternion.RotateTowards (dogRef.transform.rotation, targetRotation, angularVelocity * Time.deltaTime);
				dogRef.transform.rotation = deltaRotation;
			}
			dogAnim.SetTrigger ("Jump");
			dogRef.GetComponent <Rigidbody> ().AddForce (targetRotation * (new Vector3 (0f, 1f, 0.7f)) * jumpForce, ForceMode.Impulse);
		}
		yield return new WaitForSeconds (2);

		// Returns the dog to starting position
		StartCoroutine (dogManager.MoveToPosition (startPosition, startRotation)); 
		yield return null;
	}
		
	// check for idle animation start - Animation Event listener
	IEnumerator WailTillIdleAnimation()
	{
		while(!nextInstruct)
		{
			yield return new WaitForFixedUpdate ();
		}
		yield return null;
	}
	#endregion

	#region EventTriggers
	// Event trigger - BeginDrag
	public void OnBeginDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeDataCollection [-pointData.pointerId].startPoint= pointData.position;
	}

	// Event trigger - EndDrag
	public void OnEndDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeDataCollection [-pointData.pointerId].endPoint= pointData.position;

		if(pointData.pointerId==-1 && !swipeDataCollection [2].isActive) 
		{
			if (!combo) 
			{
				SwipeRecognizer.RecogonizeSwipe (swipeDataCollection [-pointData.pointerId], out pattern);
			}
		}
		dataReset (-pointData.pointerId);
	}

	// Event trigger - Drag
	public void OnDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		swipeDataCollection [-pointData.pointerId].swipeData.Add (pointData.position);
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
	public void OnMainMenu()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_MainMenu);
	}


	// Replay button
	public void OnRestart()
	{
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		dogManager = dogRef.GetComponent <DogManager> ();
		randomNumber = 2;
		pattern = SwipeRecognizer.TouchPattern.reset;
		gameOn = true;
		startPosition = dogRef.transform.position;
		startRotation = dogRef.transform.rotation;
		nextInstruct = true;
		StartCoroutine (Instruct ());
		combo = false;
		dogAnim = dogRef.GetComponentInChildren<Animator> ();
		
		// Add listener to reset complete event
		dogManager.ResetComplete += GotoNextInstruction;

		chance = 0;
		points = 0;
		combo = false;
		gameOverPanel.SetActive (false);
		timerNotification.gameObject.SetActive (true);
	}
	#endregion
}
