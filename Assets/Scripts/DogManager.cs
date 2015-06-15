using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

[System.Serializable]
public class DogProperties
{
	public int _Id;
	public int _WeeksOld;
	public string _Name;
	public float _Health;
	public float _WalkSpeed;
	public float _RunSpeed;
	public float _Tiredness;
	public float _Happiness;
	public float _SleepTime;
	public float _WaterLevel;
	public float _FoodLevel;
}


public class DogManager : MonoBehaviour 
{
	public GameObject dogReference;
	public DogProperties dogProps;

	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.1f;  // The damping for the speed parameter

	private Animator dogAnim;

	public float moveSpeed;

	public Vector3 moveDirection;
	Vector3 jumpHeight;
	public float jumpForce;
	public float dragFactor;
	public bool isGrounded=false;
	public bool jump;
	
	Vector2 swipeBegin;
	Vector2 swipeEnd;

	public bool isCircuitRun; // Added to override Run animation during Circuit Run

	void Awake()
	{
		dogAnim = dogReference.GetComponent<Animator>();
		jumpHeight = new Vector3(0 , jumpForce, 0);
	}

	// Use this for initialization
	void Start ()
	{

	}

	 
	void FixedUpdate ()
	{

			// Cache the inputs.
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			float v = CrossPlatformInputManager.GetAxis ("Vertical");

			MovementManagement (h, v);
	}

	void MovementManagement (float horizontal, float vertical)
	{
		 
	 
		if(CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded==true)
		{
			jump=true;
			Jump ();
		}

		moveDirection =  new Vector3(horizontal, 0, vertical);


		
		// If there is some axis input...
		if (horizontal != 0f || vertical != 0f || isCircuitRun==true) 
		{
			// ... set the players rotation and set the speed parameter to 5.5f.
			//Rotating (horizontal, vertical);
			 
			dogAnim.SetFloat ("Speed",1f, speedDampTime, Time.deltaTime);
			//GetComponent<Rigidbody> ().AddForce(transform.forward * moveSpeed );


			 
		} 
		else
			// Otherwise set the speed parameter to 0.
			 dogAnim.SetFloat ("Speed", 0);


		GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * moveSpeed * Time.deltaTime);
		transform.LookAt(transform.position + moveDirection);
		GetComponent<Rigidbody>().drag=GetComponent<Rigidbody>().velocity.magnitude * dragFactor;
	}
	

//	void Rotating (float horizontal, float vertical)
//	{
//		// Create a new vector of the horizontal and vertical inputs.
//		Vector3 targetDirection = new Vector3 (horizontal, 0f, vertical);
//		
//		// Create a rotation based on this new vector assuming that up is the global y axis.
//		Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
//		
//		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
//		Quaternion newRotation = Quaternion.Lerp (GetComponent<Rigidbody> ().rotation, targetRotation, turnSmoothing * Time.deltaTime);
//		
//		// Change the players rotation to this new rotation.
//		GetComponent<Rigidbody> ().MoveRotation (newRotation);
//	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void Jump()
	{
		dogAnim.SetTrigger("Jump");
		GetComponent<Rigidbody>().AddForce(jumpHeight,ForceMode.Impulse);

	}

	void OnTriggerStay()
	{
		isGrounded=true;
		jump=false;
	}
	
	void OnTriggerExit()
	{
		isGrounded=false;
	}

	public void OnPointerDown(BaseEventData  data)
	{
		//Debug.Log("Begins");
		PointerEventData e=(PointerEventData) data;
		swipeBegin=e.position;
	}
	
	public void OnPointerUp(BaseEventData  data)
	{
		//Debug.Log("Ends");
		PointerEventData e=(PointerEventData) data;
		swipeEnd=e.position;
		detectSwipe();
	}

	void detectSwipe()
	{
		Vector2 direction=swipeEnd-swipeBegin;
		direction.Normalize();
		
		//swipe upwards
		if(direction.y > 0 &&  direction.x > -0.5f && direction.x < 0.5f)
		{
			//Debug.Log("up swipe");
			if(isGrounded)
				Jump ();

		}
		//swipe down
		if(direction.y < 0 && direction.x > -0.5f && direction.x < 0.5f)
		{
			Debug.Log("down swipe");
		}
		//swipe left
		if(direction.x < 0 && direction.y > -0.5f && direction.y < 0.5f)
		{
			Debug.Log("left swipe");
		}
		//swipe right
		if(direction.x > 0 && direction.y > -0.5f && direction.y < 0.5f)
		{
			Debug.Log("right swipe");
		}
	}
}
