using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;


public class DogMovement : MonoBehaviour 
{
	public GameObject dogRef;
	Animator puppyAnim;
	bool gameStart;
	float horizontal;
	float vertical;
	Rigidbody rb;
	public float forceBoost;
	public float forceJump;
	Vector3 jumpHeight;
	bool jump;
	bool isGrounded=false;
	public float dragFactor;

	Vector2 swipeBegin;
	Vector2 swipeEnd;


	void Start () 
	{
		jump=false;
		jumpHeight = new Vector3(0 , forceJump, 0);
		puppyAnim= dogRef.gameObject.GetComponent<Animator>();
		gameStart=true;
		rb=GetComponent<Rigidbody>();
		dragFactor=1f;
	

	}
	
	// Update is called once per frame
	void Update () {

		rb.drag=rb.velocity.magnitude*dragFactor;

		vertical=CrossPlatformInputManager.GetAxis("Vertical");
		horizontal=CrossPlatformInputManager.GetAxis("Horizontal");

		if(vertical!=0||horizontal!=0)
		{
			puppyAnim.SetBool("run",gameStart);
		}
		else
		{
			puppyAnim.SetBool("run",false);
		}
		if(CrossPlatformInputManager.GetButton("Jump")&&isGrounded==true)
		{
			Jump();
		}

	
	}


	void Jump()
	{
		puppyAnim.SetTrigger("jump");
		jump=true;
		
	}

	void FixedUpdate()
	{

		if(vertical!=0||horizontal!=0)
		{

			rb.AddForce(transform.forward*forceBoost);

		}
		if(jump)
		{
			rb.AddForce(jumpHeight,ForceMode.Impulse);
			jump=false;
		}


	}

	void OnTriggerStay()
	{
		isGrounded=true;
	}

	void OnTriggerExit()
	{
		isGrounded=false;
	}

	public void OnPointerDown(BaseEventData  data)
	{
		Debug.Log("Begins");
		PointerEventData e=(PointerEventData) data;
		swipeBegin=e.position;
	}

	public void OnPointerUp(BaseEventData  data)
	{
		Debug.Log("Ends");
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
			Debug.Log("up swipe");
			Jump();
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
