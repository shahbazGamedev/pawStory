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
	
	public Text instructions;
	public GameObject gameOverPannel;
	public Slider dogCapacity;
	public GameObject lineRenderer;
	public GameObject dogRef;
	public GameObject[] life;
	public Text roundInfoDisplay;

	LineRenderer line;
	string layerName;

	Vector3 worldPoint;
	Vector3 potentialSamplePoint;

	public int round;
	public int resetChances;
	public int points;
	public int maxRounds;
	public int scoreIncrement;

	public float minSqrDistance;
	public float interpolationScale;
	public float maxTrackingCapacity;
	public List<Vector3> dragData;

	bool isFirstRun;
	bool needToPop;
	bool swipeFinished;
	bool isGameOn;
	bool gameOver;
	bool startTracking;
	bool pathEnable;
	bool reset;
	bool lineRendererActive;
	public bool roundComplete;

	float distanceCovered;
	float remainingCapacity;

	Vector3 touchStartPosition;
	Vector3 previousPosition;
	Vector3 startPosition;
	Quaternion startRotation;

	BezierCurve bezierPath;
	List<Vector3> drawingPoints;

	// Use this for initialization
	void Start () 
	{
		line = lineRenderer.GetComponent<LineRenderer>(); 
		dogCapacity.maxValue = maxTrackingCapacity;
		bezierPath = new BezierCurve();
		isFirstRun=true;
		needToPop=false;
		swipeFinished=false;
		roundComplete = true;
		StartCoroutine (UpdateSlider ());
		startPosition = dogRef.transform.position;
		startRotation = dogRef.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// sets the dog path data and enables path movement
		if(startTracking)
		{
			RenderBezier ();
			//Debug.Log ("Done");
			drawingPoints = bezierPath.GetDrawingPoints2();
			dogRef.GetComponent<DogPathMovement>().SetPathData(drawingPoints);
			dogRef.GetComponent<DogPathMovement>().EnableDogPathMovement(true);
			swipeFinished = false;
			startTracking = false;
		}

		// intiates gameOver function
		if(gameOver)
		{
			GameOver ();
			gameOver = false;
		}

		// initiates nextRound function
		if(roundComplete)
		{
			StartNextRound ();
			roundComplete = false;
		}

		// updates gameover bool based on maxRounds
		if (round > maxRounds)
			gameOver = true;

		// checks if dog has reached the set path and intiates next round
		if (dogRef.GetComponent<DogPathMovement> ().reachedPathEnd)
		{
			roundComplete = true;
			dogRef.GetComponent<DogPathMovement> ().reachedPathEnd = false;
		}
	}

	#region BezierInterface

	// smooth and render curve each frame as player swipes/drags on screen 
	void RenderBezier()
	{
		//bezierPath = new BezierCurve();
		bezierPath.Interpolate(dragData, interpolationScale);
		drawingPoints = bezierPath.GetDrawingPoints2();
		SetLinePoints(drawingPoints);
		lineRendererActive = true;
	}

	// update line renderer with fresh count and positions
	void SetLinePoints(List<Vector3> drawingPoint)
	{
		line.SetVertexCount(drawingPoint.Count);

		for (int i = 0; i < drawingPoint.Count; i++)
		{
			line.SetPosition(i, drawingPoint[i]);
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
	#endregion

	// Activates the gameOver Panel
	void GameOver()
	{
		gameOverPannel.SetActive (true);
		instructions.text = "Score: " + points + " / " + maxRounds;
		foreach(var live in life) // Destroys remaining resets / lives
		{
			live.SetActive (false);
		}
		gameOver = false;
	}

	// Starts next round and updates the corresponfing variables
	void StartNextRound()
	{
		dragData.Clear ();
		StartCoroutine (dogRef.GetComponent <DogManager> ().MoveToPosition (startPosition, startRotation));
		line.SetVertexCount (0);
		lineRendererActive = false;
		round += 1;
		StartCoroutine (FillDogCapacity ());
		StartCoroutine (UpdateRoundInfoDisplay ());
		if (round >= maxRounds)
			return;
		pathEnable = true;

	}
		
	#region Coroutines

	// Fills the dog capacity bar over time
	IEnumerator FillDogCapacity()
	{
		reset = true;
		while (dogCapacity.value < dogCapacity.maxValue) 
		{
			yield return new WaitForFixedUpdate ();
			distanceCovered -= Time.deltaTime;
		}
		distanceCovered = 0;
		dogCapacity.value = dogCapacity.maxValue;
		reset = false;
		yield return null;
	}

	// Depletes the dog capacity bar as player swipe the path
	IEnumerator UpdateSlider()
	{
		while(!gameOver)
		{
			//Debug.Log ("Alive");
			yield return new WaitForFixedUpdate ();
			remainingCapacity = maxTrackingCapacity - distanceCovered;
			dogCapacity.value = remainingCapacity;
			if (remainingCapacity <= 0)
				isGameOn = false;
		}
		yield return null;
	}

	// Displays round info at start of each round
	IEnumerator UpdateRoundInfoDisplay()
	{
		if (round <= maxRounds) {
			roundInfoDisplay.text = "Round " + round;
			yield return new WaitForSeconds (1.5f);
			roundInfoDisplay.text = "";
		}
	}
	#endregion

	#region EventTriggers
	// Clear previous Swipe on Drag Begin
	public void OnBeginDrag(BaseEventData Data)
	{
		//Debug.Log("onDrag");
		var data=(PointerEventData)Data;
		if (resetChances >= 0) 
		{
			if ((!isGameOn || pathEnable) && !reset) 
			{
				Vector3 screenPoint = new Vector3(data.position.x, data.position.y, 0f);
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(screenPoint);
				if (Physics.Raycast (ray, out hit, 200f)) {
					layerName = LayerMask.LayerToName (hit.collider.gameObject.layer);
					if (layerName == "Floor") {
						touchStartPosition = hit.point + (Vector3.up * 0.01f);
					}
				}

				if (Vector3.Distance (startPosition, touchStartPosition) < 0.4f)
				{
					//Debug.Log ("Working");
					pathEnable = false;
					isGameOn = true;
					//StartCoroutine (UpdateSlider ());
				}
			}
		}
		if (isGameOn) {
			dragData.Clear ();
			isFirstRun = true;
		}
	}

	// Add end point
	public void OnEndDrag(BaseEventData data)
	{
		swipeFinished = true;
		//Debug.Log("offDrag");
		if(isGameOn) {
			addEndPoint (data);
			isGameOn = false;
		}
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

					if(dragData.Count<1) // dragData is empty
					{
						dragData.Add (touchStartPosition);
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
			if(dragData.Count>1)
			{
				// Render Line
				RenderBezier();
			}
		}
	}

	#endregion

	#region ButtonCallbacks
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

	// Reset Path button
	public void ResetPathBtn()
	{
		if(!dogRef.GetComponent <DogPathMovement>().followPath && !dogRef.GetComponent <DogManager>().isCoroutineOn && lineRendererActive)
		{
		resetChances -= 1;
		if (resetChances >= 0) 
			{
			line.SetVertexCount (0);
			lineRendererActive=false;
			StartCoroutine (FillDogCapacity ());
			life [resetChances].SetActive (false);
		} 
		else
			resetChances = 0;
		}
	}

	// Signal Dog to track
	public void StartTracking()
	{
		if(dragData.Count<2)
		{
			Debug.Log ("Swipe data empty");
		}
		else
		{
			if (swipeFinished)
				startTracking = true;
		}
	}
	#endregion
}
