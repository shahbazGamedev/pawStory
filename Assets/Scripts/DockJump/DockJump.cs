using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DockJump : MonoBehaviour {
	public GameObject dogRef;
	private Animator dogAnim;
	private Vector3 jumpHeight;
	public float moveSpeed;
	bool isJumping=false;
	public float jumpForce;
	private float speedDampTime = 0.1f;
	Rigidbody rb;
	Vector2 swipeBegin;
	Vector2 swipeEnd;
	public float jumpspeed;
	private float dragRatio;
	bool isRunning;
	public int chances =3;
	private Vector3 dogPos;

	void awake()
	{

	}

	// Use this for initialization
	void Start () {
		isRunning=false;
		dogAnim = dogRef.GetComponent<Animator> ();
		jumpHeight = new Vector3 (0, jumpForce, jumpspeed);
		rb = GetComponent<Rigidbody> ();
	    dogPos=new Vector3(0.16f,1.285f,4.67f);
	}
	
	// Update is called once per frame
	void Update () {
		movement();
		//detectSwipe();
		StartCoroutine(ReturnDog());


	}
	void FixedUpdate() 
	{

		if(isRunning==true)
		{
		running ();
		}
	}
	void jumping()
	{
		isRunning=false; 
		dogAnim.SetTrigger ("jump");
		Debug.Log("WORKING FINE");
		rb.AddForce(jumpHeight,ForceMode.Impulse);




	}
	void running()
	{
		//if(isRunning=true)
		rb.drag = rb.velocity.magnitude * dragRatio;
		dogAnim.SetFloat ("running",1f, speedDampTime, Time.deltaTime);
		rb.AddForce (transform.forward * moveSpeed);

}


		IEnumerator ReturnDog ()
		{
		yield return new WaitForSeconds(7.0f);
		transform.position = dogPos;
		rb.velocity = Vector3.zero;
	


	}
	void OnTriggerStay()
	{
		Debug.Log ("triggered");
		isJumping = true;
	}
	
	void OnTriggerExit()
	{
		isJumping = false;
	}
//	public void OnPointerDown(BaseEventData  data)
//	{
//		Debug.Log("Begins");
//		PointerEventData e=(PointerEventData) data;
//		swipeBegin=e.position;
//	}
//	
//	public void OnPointerUp(BaseEventData  data)
//	{
//		//Debug.Log("Ends");
//		PointerEventData e=(PointerEventData) data;
//		swipeEnd=e.position;
//		detectSwipe();
//	}
	void movement()
	{
		if(Input.GetKeyDown(KeyCode.Space)&& isJumping==true)
		{
			jumping();

			Debug.Log("WORKING");

		}

	}
	
//	void detectSwipe()
//	{


//		Vector2 direction=swipeEnd-swipeBegin;
//		direction.Normalize();
//		
//		//swipe upwards
//		if(direction.y > 0 &&  direction.x > -0.5f && direction.x < 0.5f)
//		{
//			Debug.Log("up swipe");
//			//if(isJumping)
//			//{
//
//
//		//	}
//				
//			
//		}
//		//swipe down
//		if(direction.y < 0 && direction.x > -0.5f && direction.x < 0.5f)
//		{
//			Debug.Log("down swipe");
//		
//		}
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

	//}
	void OnCollisionEnter(Collision collision)
	{
		if(collision.rigidbody)
		{
			chances=chances-1;
			dogAnim.SetFloat ("running",0f, speedDampTime, Time.deltaTime);
		}
	}
	private void gameStart()
	{
		isRunning=true;

	}
	private void GameOver()
	{
		if(chances==0)
		{
			//Application.LoadLevel("gameover");
		}
	

	

	}
}