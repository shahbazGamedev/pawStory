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
	SwipeRecognizer.TouchPattern pattern;

	// Use this for initialization
	void Start () {
		swipeData = new List<Vector2> ();	
		pattern = SwipeRecognizer.TouchPattern.reset;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBeginDrag (BaseEventData data)
	{
		//Debug.Log ("Begin");
		PointerEventData pointData = (PointerEventData)data;
		touchBeginID = pointData.pointerId;
		startPoint = pointData.position;
		swipeData.Clear ();
		pattern = SwipeRecognizer.TouchPattern.reset;
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
