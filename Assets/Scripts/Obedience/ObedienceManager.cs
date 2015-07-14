using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObedienceManager : MonoBehaviour {
	
	public Text instructions;
	bool gameOn;
	bool catchUserInput;
	int randomNumber;
	public float instructionWaitTime;

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
		public SwipeRecognizer.TouchPattern value;
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
	}
	
	// Update is called once per frame
	void Update () {
		if(catchUserInput)
		{
			if (pattern != SwipeRecognizer.TouchPattern.reset) 
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
		}
	}

	IEnumerator Instruct()
	{
		while(gameOn) 
		{
			randomNumber = Random.Range (0, 10);
			//randomNumber = 10;
			presentGesture = gestureCollection[randomNumber].value;
			instructions.text = gestureCollection[randomNumber].key;
			catchUserInput = true;
			yield return new WaitForSeconds (instructionWaitTime);
		}
		yield return null;
	}
			

	public void OnBeginDrag (BaseEventData data)
	{
		//Debug.Log ("Begin");
		PointerEventData pointData = (PointerEventData)data;
		startPoint = pointData.position;

	}

	public void OnEndDrag (BaseEventData data)
	{
		//Debug.Log ("End");
		PointerEventData pointData = (PointerEventData)data;
		endPoint = pointData.position;
		SwipeRecognizer.RecogonizeSwipe (startPoint, endPoint, swipeData, out pattern);
		//Debug.Log (pattern.ToString ());
		swipeData.Clear ();
	}

	public void OnDrag (BaseEventData data)
	{
		PointerEventData pointData = (PointerEventData)data;
		swipeData.Add(pointData.position);
		//Debug.Log (pointData.position);
	}

	void SwipeReset()
	{
		//swipeText.text = pattern.ToString ();
		pattern = SwipeRecognizer.TouchPattern.reset;
		swipeData.Clear ();
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
}
