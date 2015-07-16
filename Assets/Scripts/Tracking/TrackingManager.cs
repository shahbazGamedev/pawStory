/**
Script Author : Vaikash 
Description   : Tracking path of swipe
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class TrackingManager : MonoBehaviour {

	public Slider dogCapacity;
	public List<Vector3> dragData;
	public GameObject lineRenderer;
	LineRenderer line;
	string layerName;

	Vector3 worldPoint;
	Vector3 potentialSamplePoint;

	public float minSqrDistance;
	public float interpolationScale;

	bool isFirstRun;
	bool needToPop;
	bool swipeFinished;

	public GameObject dogRef;

	public float maxTrackingCapacity;
	float distanceCovered;
	float remainingCapacity;
	bool isGameOn;
	Vector3 previousPosition;
 
	// Use this for initialization
	void Start () 
	{
		dragData=new List<Vector3>();
		line = lineRenderer.GetComponent<LineRenderer>(); 
		dogCapacity.maxValue = maxTrackingCapacity;

		isFirstRun=true;
		needToPop=false;
		swipeFinished=false;
		isGameOn = true;
		StartCoroutine (UpdateSlider ());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(dragData.Count>1)
		{
			// Render Line
			RenderBezier();
		}
	}

	// Clear previous Swipe on Drag Begin
	public void OnBeginDrag(BaseEventData Data)
	{
		//Debug.Log("onDrag");
		var data=(PointerEventData)Data;
		dragData.Clear();
		isFirstRun=true;
	}

	// Add end point
	public void OnEndDrag(BaseEventData data)
	{
		//Debug.Log("offDrag");
		if(isGameOn)
			addEndPoint(data);
		swipeFinished=true;
	}

	// Check dist between drag points and add points that are min dist appart
	public void OnDrag(BaseEventData Data)
	{
		//Debug.Log("Dragging");
		PointerEventData data=(PointerEventData)Data;
		Vector3 screenPoint = new Vector3(data.position.x, data.position.y, 0f);
		if(isGameOn)
		{
			// raycast to find hit point on plane
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(screenPoint);
			if (Physics.Raycast(ray, out hit, 200f))
			{
				layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
				if(layerName == "Floor")
				{
					worldPoint = hit.point + (Vector3.up * 0.01f);

					if(dragData.Count<2) // dragData is empty
					{
						dragData.Add (worldPoint);
						previousPosition = worldPoint;
					}

					else if(dragData.Count==2 && isFirstRun) // dragData has drag start point
					{
						//dragData.Add(worldPoint);
						potentialSamplePoint=worldPoint;
						isFirstRun=false;
					}

					else // checks for minimum distance between points and adds them if true
					{
						if(needToPop)
						{
							dragData.RemoveAt(dragData.Count-1);
							needToPop=false;
						}
						if((dragData.Last() - worldPoint).sqrMagnitude >= minSqrDistance) 
						{
							dragData.Add(potentialSamplePoint);
						}
						else // added to prevent visual lag - removed @ next frame
						{
							dragData.Add(worldPoint);
							needToPop=true;
						}
						potentialSamplePoint=worldPoint;
					}
				}
				distanceCovered += Vector3.Distance (worldPoint, previousPosition);
				previousPosition = worldPoint;

			}
		}

	}

	// smooth and render curve each frame as player swipes/drags on screen 
	private void RenderBezier()
	{
		BezierCurve bezierPath = new BezierCurve();

		bezierPath.Interpolate(dragData, interpolationScale);
		List<Vector3> drawingPoints = bezierPath.GetDrawingPoints2();
		
		SetLinePoints(drawingPoints);
		if(swipeFinished)
		{
			dogRef.GetComponent<DogPathMovement>().SetPathData(drawingPoints);
			dogRef.GetComponent<DogPathMovement>().EnableDogPathMovement(true);
			swipeFinished=false;
		}
	}

	// update line renderer with fresh count and positions
	private void SetLinePoints(List<Vector3> drawingPoints)
	{
		line.SetVertexCount(drawingPoints.Count);
		
		for (int i = 0; i < drawingPoints.Count; i++)
		{
			line.SetPosition(i, drawingPoints[i]);
		}
	}

	// Add end point OnDragEnd
	void addEndPoint(BaseEventData Data)
	{
		PointerEventData data=(PointerEventData)Data;
		Vector3 screenPoint = new Vector3(data.position.x, data.position.y, 0f);
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(screenPoint);
		if (Physics.Raycast(ray, out hit, 200f))
		{
			layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
			if(layerName == "Floor")
			{
				worldPoint = hit.point + (Vector3.up * 0.01f);
				dragData.Add(worldPoint);
			}
		}
	}

	IEnumerator UpdateSlider()
	{
		while(isGameOn)
		{
			yield return new WaitForFixedUpdate ();
			remainingCapacity = maxTrackingCapacity - distanceCovered;
			dogCapacity.value = remainingCapacity;
			if (remainingCapacity <= 0)
				isGameOn = false;
		}
		yield return null;
	}

}
