/**
Script Author : Vaikash 
Description   : Catch Training Game Manager
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CatchTrainer : MonoBehaviour {

	public GameObject dogRef;
	public GameObject ballPrefab;
	public GameObject markerPrefab;
	public int noOfMarkers;
	public float thrustForce;

	bool isPressed;
	Vector3 targetPos;
	Vector3 startPos;
	Vector3 projectileVelocity;

	List<GameObject> markerList;
	List<Vector3> pathList;

	// Use this for initialization
	void Start () {
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		targetPos = dogRef.transform.position;
		markerList = new List<GameObject> ();
		InitMarker ();
	}
	
	// Update is called once per frame
	void Update () {
		if(isPressed)
		{
			// Code to update and display projectile trajectory
			Debug.Log ("Trajectory code under progress");
			UpdateMarker ();
		}
	}

	// Pool the marker in a list
	void InitMarker()
	{
		for(int i = 0; i < noOfMarkers; i++)
		{
			GameObject instanceRef = Instantiate (markerPrefab);
			instanceRef.GetComponent<Renderer>().enabled = false;
			instanceRef.transform.parent = transform;
			markerList.Insert(i, instanceRef);
		}
	}

	// Update marker position based on user interaction
	void UpdateMarker()
	{
		Vector3 vel = CalcForce(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		float angle = Mathf.Atan2 (vel.y, vel.x) * Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(0,0,angle);
		setMarker(transform.position, vel);
	}

	// calculate force for projectile
	Vector3 CalcForce(Vector3 fromPos, Vector3 toPos)
	{
		return (new Vector2 (toPos.x, toPos.y) - new Vector2 (fromPos.x, fromPos.y)) * thrustForce;
	}

	// Updates the position of markers
	void setMarker(Vector3 pStartPosition , Vector3 pVelocity )
	{
		float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
		float angle = Mathf.Rad2Deg*(Mathf.Atan2(pVelocity.y , pVelocity.x));
		float fTime = 0;
		fTime += 0.1f;
		for (int i = 0 ; i < noOfMarkers ; i++)
		{
			float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
			float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
			Vector3 pos = new Vector3(pStartPosition.x + dx , pStartPosition.y + dy ,2);
			markerList[i].transform.position = pos;
			markerList[i].GetComponent <Renderer>().enabled = true;
			markerList[i].transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude)*fTime,pVelocity.x)*Mathf.Rad2Deg);
			fTime += 0.1f;
		}
	}

	#region EventTriggers

	// Event trigger - BeginDrag
	public void OnBeginDrag (BaseEventData data)
	{

	}

	// Event trigger - EndDrag
	public void OnEndDrag (BaseEventData data)
	{

	}

	// Event trigger - Drag
	public void OnDrag (BaseEventData data)
	{

	}

	// Event trigger - Pointer Click
	public void OnClickEnd(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
	}

	// Event Trigger - Pointer Down
	public void OnPointerDown(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if (pointData.pointerId == -1) {
			isPressed = true;
		}
	}

	// Event Trigger - Pointer Up
	public void OnPointerUp(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if (pointData.pointerId == -1) {
			isPressed = false;
		}
	}

	#endregion
}
