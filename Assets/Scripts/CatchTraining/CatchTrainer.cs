/**
Script Author : Vaikash 
Description   : Catch Training Game Manager
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatchTrainer : MonoBehaviour {

	public static CatchTrainer instRef;
	public GameObject dogRef;
	public GameObject ballPrefab;
	public GameObject markerPrefab;
	public GameObject cannonRef;
	public GameObject ballCatched;

	public int noOfMarkers;
	public float sensitivity;
	public float clampLimit;
	public bool isHoldingBall;

	float projVel;
	float projAngle;
	float thrustForce;
	bool isPressed;
	bool gameOver;

	Vector3 targetPos;
	Vector2 touchStartPos;
	Vector2 touchCurPos;
	Vector2 touchPrevPos;
	Vector3 projectileVelocity;
	Vector3 direction;

    bool toySelected;
    GameObject toyRef;
    string layerName;


    public GameObject[] toys;
    public LayerMask mask;

    List<GameObject> markerList;

	// UI Components
	public Text instruction;

	// Use this for initialization
	void Start () {
		instRef = this;
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		targetPos = dogRef.transform.position;
		markerList = new List<GameObject> ();
		InitMarker ();
		instruction.text="Aim for Puppy!";
    }

	// Listen for restart
	void OnEnable() {
		EventMgr.GameRestart += OnRestartGame;
	}

	// Decouple event listener
	void OnDisable() {
		EventMgr.GameRestart -= OnRestartGame;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp (KeyCode.Space))
		{
			ThrowBall ();
		}
		if (isHoldingBall) {
			ballCatched.SetActive (true);
			gameOver = true;
			isHoldingBall = false;
			instruction.text="Puppy Caught the Ball!!";
		}
	}

	// Pool the marker in a list
	void InitMarker()
	{
		for(int i = 0; i < noOfMarkers; i++)
		{
			GameObject instanceRef = Instantiate (markerPrefab);
			instanceRef.GetComponent<Renderer>().enabled = false;
			instanceRef.transform.parent = cannonRef.transform;
			markerList.Insert(i, instanceRef);
		}
	}

	// calculate force for projectile
	Vector2 CalcForce(Vector2 fromPos, Vector2 toPos)
	{
		thrustForce = Mathf.Clamp (Vector3.Distance (touchStartPos, touchCurPos)/sensitivity, 0f, clampLimit);
		var force = (toPos - fromPos) * thrustForce;
		force = new Vector2 (force.x < 0 ? -force.x : 0, force.y < 0 ? -force.y : 0);
		return force;
	}

	// calculate direction for projectile
	Vector2 CalcDirection(Vector2 fromPos, Vector2 toPos)
	{
		var dir=(toPos - fromPos);
		dir = new Vector2 (dir.x < 0 ? -dir.x : 0, dir.y < 0 ? -dir.y : 0);
		return dir;

	}

	// updates the position of markers
	void setMarker(Vector3 pStartPosition , Vector3 pVelocity )
	{
		projVel = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
		//Debug.Log (projVel);
		var angle = Mathf.Rad2Deg*(Mathf.Atan2(pVelocity.y , pVelocity.x));
		angle = angle < 0 ? angle += 360 : angle;
		float fTime = 0;
		fTime += 0.1f;
		for (int i = 0 ; i < noOfMarkers ; i++)
		{
			float dx = projVel * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
			float dy = projVel * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
			Vector3 pos = new Vector3 (pStartPosition.x + dx, pStartPosition.y + dy, 0);
			markerList [i].transform.position = pos;
			markerList[i].GetComponent <Renderer>().enabled = true;
            markerList[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
			fTime += 0.1f;
		}
	}

	// throw ball
	void ThrowBall()
	{
		if(!gameOver && toyRef !=null) {
			var ballRef = (GameObject)Instantiate (ballPrefab, toyRef.transform.position, Quaternion.identity);
			var force = Quaternion.Euler (new Vector3 (0, projAngle, 0)) * (projectileVelocity);
			ballRef.GetComponent <Rigidbody> ().AddForce (force, ForceMode.Impulse);
			HideMarker ();
		}
	}

	// Hides Marker
	void HideMarker()
	{
		for(int i = 0; i < noOfMarkers; i++)
		{
			markerList[i].GetComponent <Renderer>().enabled = false;
		}
	}

    // Raycast at touch
    void findToySelected(Vector3 touchPos)
    {
        toyRef = null;
        Vector3 screenPoint = new Vector3(touchPos.x, touchPos.y, -5f);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            //Debug.DrawRay(screenPoint, hit.point, Color.yellow);
            //Debug.Log("Hit");
            layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            //Debug.Log(layerName);
            if (layerName == "Toys")
            {
                toyRef = hit.collider.gameObject;
                cannonRef.transform.position = toyRef.transform.position;
            }
            if(toyRef!=null)
               Debug.Log(toyRef.name);
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
		if (projVel > 5) {
			ThrowBall ();
//			instruction.text="";
		}
	}

	// Event trigger - Drag
	public void OnDrag (BaseEventData data)
	{
        if (toyRef == null)
        {
            return;
        }
        else
        {
            var pointData = (PointerEventData)data;
            touchCurPos = pointData.position;
            projectileVelocity = CalcForce(touchStartPos, touchCurPos);
            direction = CalcDirection(touchStartPos, touchCurPos);
            projAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projAngle = projAngle < 0 ? projAngle += 360 : projAngle;
            projAngle = 360 - projAngle;
            Debug.Log(projAngle);
            if (toyRef == toys[0])
            {
                
            }
            else if (toyRef == toys[1])
            {
                //projAngle -= 45;
            }
            else if (toyRef == toys[2])
            {
                //projAngle -= 60;
            }
            cannonRef.transform.eulerAngles = new Vector3(0, 0, 0);
            setMarker(toyRef.transform.position, projectileVelocity);
            cannonRef.transform.eulerAngles = new Vector3(0, projAngle, 0);
        }
	}

	// Event trigger - Pointer Click
	public void OnClickEnd(BaseEventData data)
	{

	}

	// Event Trigger - Pointer Down
	public void OnPointerDown(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if (pointData.pointerId == -1) {
			touchStartPos = pointData.position;
			isPressed = true;
		}

        // raycast to find the toy
        findToySelected(touchStartPos);
	}

	// Event Trigger - Pointer Up
	public void OnPointerUp(BaseEventData data)
	{
		var pointData = (PointerEventData)data;
		if (pointData.pointerId == -1) {
			isPressed = false;
		}
	}
		
	// Reset ball
	public void ResetBall()
	{
		gameOver=false;
		isHoldingBall=false;
		ballCatched.SetActive(false);
		instruction.text="Aim for Puppy!";
	}

	// Reset game
	void OnRestartGame()
	{
		Application.LoadLevel (GlobalConst.Scene_CatchTraining);
	}

	#endregion
}
