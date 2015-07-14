/**
Script Author : Vaikash 
Description   : Process Touch Input
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
	public Text swipeText;
	Vector2 startPoint;
	Vector2 endPoint;
	List<Vector2> swipeData;
	int touchBeginID;
	int touchEndID;
	SwipeRecognizer.TouchPattern pattern;

	// Use this for initialization
	void Start ()
	{
		swipeData = new List<Vector2> ();	
		pattern = SwipeRecognizer.TouchPattern.reset;
	}

	// Update is called once per frame
	void Update ()
	{
		
		switch(pattern)
		{
		case SwipeRecognizer.TouchPattern.swipeUp:
			{
				//Debug.Log ("SwipeUp");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeDown:
			{
				//Debug.Log ("SwipeDown");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeLeft:
			{
				//Debug.Log ("SwipeLeft");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeRight:
			{
				//Debug.Log ("SwipeRight");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeUpLeft:
			{
				//Debug.Log ("Diagonal - SwipeUpLeft");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeDownLeft:
			{
				//Debug.Log ("Diagonal - SwipeDownLeft");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeUpRight:
			{
				//Debug.Log ("Diagonal - SwipeUpRight");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.swipeDownRight:
			{
				//Debug.Log ("Diagonal - SwipeDownRight");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.clockwiseCircle:
			{
				//Debug.Log ("ClockwiseCircle");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.antiClockwiseCircle:
			{
				//Debug.Log ("AntiClockwiseCircle");
				SwipeReset ();
				break;
			}
		case SwipeRecognizer.TouchPattern.tryAgain:
			{
				SwipeReset ();
				break;
			}
		}

	}

	public void OnBeginDrag (BaseEventData data)
	{
		//Debug.Log ("Begin");
		PointerEventData pointData = (PointerEventData)data;
		touchBeginID = pointData.pointerId;
		startPoint = pointData.position;
		swipeData.Clear ();
	}

	public void OnEndDrag (BaseEventData data)
	{
		//Debug.Log ("End");
		PointerEventData pointData = (PointerEventData)data;
		touchEndID = pointData.pointerId;
		endPoint = pointData.position;
		if(touchBeginID==touchEndID)
			SwipeRecognizer.RecogonizeSwipe (startPoint, endPoint, swipeData, out pattern);
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
}
