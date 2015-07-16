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
	bool isPointerDown;
	public float holdTime;
	float swipeDelta;

	int chance;
	public int maxChances;
	int points;
	public int scoreIncrement;


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
		if(pointData.pointerId==-1)
			startPoint = pointData.position;
	}

	public void OnEndDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if(pointData.pointerId==-1) {
			endPoint = pointData.position;
			if (!combo) {
				SwipeRecognizer.RecogonizeSwipe (startPoint, endPoint, swipeData, out pattern);
			}
			swipeData.Clear ();
		}
	}

	public void OnDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if(pointData.pointerId==-1)
		{
			swipeData.Add(pointData.position);
			//swipeDelta += (Vector2.Distance (startPoint, pointData.position) / Screen.dpi) * 160;	
		}
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
		var pointData = (PointerEventData)data;
		isPointerDown = true;
		//Debug.Log (pointData.pointerId);
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
				//Debug.Log ("Success!!");
				instructions.text="Excellent";
				catchUserInput = false;
				points += scoreIncrement;
				DeactivateGestureMat ();
				//StartCoroutine (dogManager.MoveToPosition (startPosition, startRotation));
			} 
			else // if wrong gesture
			{
				Debug.Log (pattern);
				instructions.text="Wrong";
				DeactivateGestureMat ();
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
				instructions.text = "Score: " + points;
			}
			else  // else put a random instruction
			{
				randomNumber = Random.Range (0, range);
				//randomNumber = 4;
				presentGesture = gestureCollection [randomNumber].value1;
				instructions.text = gestureCollection [randomNumber].key;

				SwipeReset (); // reset previous swipe data
				catchUserInput = true;

				gestureMat.SetActive (true); // Turn on user input if off
				holdTime = 0; // reset hold time
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
			if(isPointerDown)
			{
				// Min. hold time of 1 sec
				if(holdTime>1 && swipeData.Count<1)
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

	// Disable touchInput
	public void DeactivateGestureMat()
	{
		catchUserInput = false;
		gestureMat.SetActive (false);
		SwipeReset ();
	}

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
}
