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
	MouseRotation rotation;


	void Awake()
	{
		rotation= pulleyRef.GetComponent<MouseRotation>();
		dogAnimator =  dogRef.GetComponent<Animator>();
		dogRigidBody = this.GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () 
	{

	    
	}



	public void PointerDown(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log("Selected");
		rotation.isTouched=true;



	}


	public void PointerUp(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log("Unselected");
		rotation.isTouched=false;

	
	}





	void FixedUpdate()
	{
		timer.text="Time Left: "+(int)levelTime;
		levelTime-=Time.deltaTime;
	}


	// Update is called once per frame
	void Update () 
	{
//
//	      if (Input.GetMouseButton(0) && isDragging) 
//		{
//			dogRef.GetComponent<DogMovementTugOfWar>().Movement();
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
//		if(!isPulleyTouched)
//		{
//			pulleyRef.transform.Rotate(Vector3.forward * Time.deltaTime * 100);
//			dogRef.GetComponent<DogMovementTugOfWar>().BackMovement();
//		}
//
//		pulleyRef.transform.Rotate(Camera.main.transform.forward * theSpeed.x * rotationSpeed, Space.World);
//		pulleyRef.transform.Rotate(Camera.main.transform.forward* theSpeed.y * rotationSpeed, Space.World);
//	if(isPulleyTouched)
//		{
//    	pulleyRef.transform.Rotate(-Vector3.forward * Time.deltaTime * 100);
//		dogRef.GetComponent<DogMovementTugOfWar>().Movement();
//		}
//		else
//		{
//			pulleyRef.transform.Rotate(Vector3.forward * Time.deltaTime * 100);
//			dogRef.GetComponent<DogMovementTugOfWar>().BackMovement();
//
//		} 




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

	




