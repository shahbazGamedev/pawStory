using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TugOfWarManager : MonoBehaviour 
{
	public GameObject dogRef;

	private Animator dogAnimator;
	private Rigidbody dogRigidBody;

	public bool isPulleyTouched;
	public Image pulleyRef;
	public Text timer;
	public float levelTime;
//	//
//	private float rotationSpeed = 10.0F;
//	private float lerpSpeed = 1.0F;
//	private Vector3 theSpeed;
//	private Vector3 avgSpeed;
//	public  bool isDragging = false;
//	private Vector3 targetSpeedX;
//	//
	void Awake()
	{
		dogAnimator =  dogRef.GetComponent<Animator>();
		dogRigidBody = this.GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () 
	{

	    
	}

	public void TrackMouseMovement(BaseEventData data)
	{
		 
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log(pointerData);
	}

	public void PointerDown(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log("Selected");
		isPulleyTouched = true;
		//isDragging=true;
	}

	public void PointerUp(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log("Unselected");
		isPulleyTouched = false;
	}

	public void RotatePulley(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		if(isPulleyTouched)
		Debug.Log( Mathf.Atan(pointerData.delta.x) * Mathf.Rad2Deg);

		Vector3 rotate = pulleyRef.rectTransform.localEulerAngles;
		rotate.z = Mathf.Atan(pointerData.delta.x) * Mathf.Rad2Deg;//pointerData.delta.x;
		pulleyRef.rectTransform.localEulerAngles = rotate;

	}

	public void DragPulley(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		if(isPulleyTouched)
		{
			Debug.Log(pointerData);
		}
	}
	 
	void FixedUpdate()
	{
		timer.text="Time Left: "+levelTime;
		levelTime-=Time.deltaTime;
	}


	// Update is called once per frame
	void Update () 
	{
//		
//		if (Input.GetMouseButton(0) && isDragging) {
//			theSpeed = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),0.0f);
//			avgSpeed = Vector3.Lerp(avgSpeed, theSpeed, Time.deltaTime * 5);
//		} else {
//			if (isDragging) {
//				theSpeed = avgSpeed;
//				isDragging = false;
//			}
//			float i = Time.deltaTime * lerpSpeed;
//			theSpeed = Vector3.Slerp(theSpeed, Vector3.zero, i);
//		}
//
//		pulleyRef.transform.Rotate(Camera.main.transform.forward * theSpeed.x * rotationSpeed, Space.World);
//		//pulleyRef.transform.Rotate(Camera.main.transform.forward* theSpeed.y * rotationSpeed, Space.World);
	if(isPulleyTouched)
		{
    	pulleyRef.transform.Rotate(-Vector3.forward * Time.deltaTime * 100);
		dogRef.GetComponent<DogMovementTugOfWar>().Movement();
		}
		else
		{
			pulleyRef.transform.Rotate(Vector3.forward * Time.deltaTime * 100);
			dogRef.GetComponent<DogMovementTugOfWar>().BackMovement();

		}
	    }

	public void ReStart()
	{
		Application.LoadLevel("TugOfWar");
	}

	public void MainMenu()
	{
		Application.LoadLevel("MainMenu");
	}
        }  

	




