/**
Script Author : Vaikash 
Description   : Process Touch Input
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{

	public GameObject touchMat;
	public SwipeRecognizer.TouchDataCollection[] touchDataCollection;

	SwipeRecognizer.TouchPattern pattern;
	SwipeRecognizer.TouchPattern gestureCache;

	public delegate void touchEventBroadcastSystem(SwipeRecognizer.TouchPattern touchPattern);
	public event touchEventBroadcastSystem patternRecognized;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if(pattern!=SwipeRecognizer.TouchPattern.reset)
		{
			if (patternRecognized != null)
				patternRecognized (pattern);
			gestureCache = pattern;
			Debug.Log (gestureCache);
			pattern = SwipeRecognizer.TouchPattern.reset;
		}
	}

	// Reset previous touch input
	void SwipeReset()
	{
		pattern = SwipeRecognizer.TouchPattern.reset;
	}


	// Reset particular swipe data
	void dataReset(int pointerID)
	{
		touchDataCollection [pointerID].swipeData.Clear ();
	}

	#region EventTriggers

	// Event trigger - BeginDrag
	public void OnBeginDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		touchDataCollection [-pointData.pointerId].startPoint= pointData.position;
	}

	// Event trigger - EndDrag

	public void OnEndDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		touchDataCollection [-pointData.pointerId].endPoint= pointData.position;

		if(pointData.pointerId==-1 && !touchDataCollection [2].isActive) 
		{
			SwipeRecognizer.RecogonizeSwipe (touchDataCollection [-pointData.pointerId], out pattern, false);
		}
		dataReset (-pointData.pointerId);
	}

	// Event trigger - Drag
	public void OnDrag (BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		touchDataCollection [-pointData.pointerId].swipeData.Add (pointData.position);
	}

	// Event trigger - Pointer Click
	public void OnClickEnd(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if (pointData.pointerId == -1 && !touchDataCollection [2].isActive) 
		{
			// check for tap 
			if (touchDataCollection [-pointData.pointerId].swipeData.Count <= 1) 
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
		touchDataCollection [-pointData.pointerId].isActive = true;
	}

	// Event Trigger - Pointer Up
	public void OnPointerUp(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		touchDataCollection [-pointData.pointerId].isActive = false;
		touchDataCollection [-pointData.pointerId].holdTime = 0;
	}

	#endregion
}
