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
	Vector2 startPoint;
	Vector2 endPoint;
	List<Vector2> swipeData;
	int pointerID;
	SwipeRecognizer.TouchPattern pattern;

	// Use this for initialization
	void Start ()
	{
		swipeData = new List<Vector2> ();	
	}

	// Update is called once per frame
	void Update ()
	{
		 
	}

	public void OnBeginDrag (BaseEventData data)
	{
		PointerEventData pointData = (PointerEventData)data;
		pointerID = pointData.pointerId;
		startPoint = pointData.position;
		swipeData.Clear ();
	}

	public void OnEndDrag (BaseEventData data)
	{
		PointerEventData pointData = (PointerEventData)data;
		endPoint = pointData.position;
		SwipeRecognizer.RecogonizeSwipe (startPoint, endPoint, swipeData, out pattern);
	}

	public void OnDrag (BaseEventData Data)
	{

	}
}
