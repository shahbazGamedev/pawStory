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

	Vector2 startPoint;
	Vector2 endPoint;
	List<Vector2> swipeData;
	SwipeRecognizer.TouchPattern pattern;
	SwipeRecognizer.TouchPattern presentGesture;

	[System.Serializable]
	public struct GestureCollection {
		public string key;
		public SwipeRecognizer.TouchPattern value;
	}

	public GestureCollection[] gestureCollection;

	// Use this for initialization
	void Start () {
		swipeData = new List<Vector2> ();	
		pattern = SwipeRecognizer.TouchPattern.reset;
		gameOn = true;
		StartCoroutine (Instruct ());
	}
	
	// Update is called once per frame
	void Update () {
		if(catchUserInput)
		{
			if(pattern!=SwipeRecognizer.TouchPattern.reset)
			{
				if(pattern==presentGesture)
				{
					Debug.Log ("Woot!!");
					catchUserInput = false;
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
			presentGesture = gestureCollection[randomNumber].value;
			instructions.text = gestureCollection[randomNumber].key;
			catchUserInput = true;
			yield return new WaitForSeconds (5);
		}
		yield return null;
	}
			

	public void OnBeginDrag (BaseEventData data)
	{
		//Debug.Log ("Begin");
		PointerEventData pointData = (PointerEventData)data;
		startPoint = pointData.position;
		swipeData.Clear ();
	}

	public void OnEndDrag (BaseEventData data)
	{
		//Debug.Log ("End");
		PointerEventData pointData = (PointerEventData)data;
		endPoint = pointData.position;
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
