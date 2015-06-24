using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DockJump : MonoBehaviour {
	public GameObject dogRef;
	private Animator dogAnim;
	public Vector3 jumpHeight;
	public float moveSpeed;
	bool isJumping=false;
	public float jumpForce;
	public float speedDampTime = 0.1f;
	Rigidbody rb;
	Vector2 swipeBegin;
	Vector2 swipeEnd;
	public float jumpspeed;
	void awake()
	{

	}

	// Use this for initialization
	void Start () {
		dogAnim = dogRef.GetComponent<Animator> ();
		jumpHeight = new Vector3 (0, jumpForce, jumpspeed);
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {


	}
	void FixedUpdate() 
	{
		running ();
	}
	void jumping()
	{
		dogAnim.SetTrigger ("jump");
		rb.AddForce(jumpHeight,ForceMode.Impulse);


	}
	void running()
	{
		dogAnim.SetFloat ("running",1f, speedDampTime, Time.deltaTime);
		rb.AddForce (transform.forward * moveSpeed);

}
	//void OnTriggerStay()
	//{
		//Debug.Log ("triggered");
		//isJumping = true;
//	}
	
	//void OnTriggerExit()
	//{
	//	isJumping = false;
//	}
	public void OnPointerDown(BaseEventData  data)
	{
		Debug.Log("Begins");
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
			Debug.Log("up swipe");
			//if(isJumping)
			//{
				jumping();
		//	}
				
			
		}
		//swipe down
		if(direction.y < 0 && direction.x > -0.5f && direction.x < 0.5f)
		{
			Debug.Log("down swipe");
		
		}
		//swipe left
		//if(direction.x < 0 && direction.y > -0.5f && direction.y < 0.5f)
		//{
		//	Debug.Log("left swipe");
		//}
		//swipe right
		//if(direction.x > 0 && direction.y > -0.5f && direction.y < 0.5f)
		//{
		////	Debug.Log("right swipe");
		//}
	}
}