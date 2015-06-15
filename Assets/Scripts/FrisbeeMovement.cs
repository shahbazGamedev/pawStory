
/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/

using UnityEngine;
using System.Collections;
<<<<<<< HEAD

public class FrisbeeMovement : MonoBehaviour {
=======
using UnityEngine.EventSystems;


public class FrisbeeMovement : MonoBehaviour 
{
>>>>>>> origin/master
	
	
	private float power= 500.0f;
	Rigidbody rb;
<<<<<<< HEAD
	public Vector3 endPos;
	public Vector3 force;
	
	private Vector3 startPos;
	void  Start (){
		rb = GetComponent<Rigidbody>();
	}
	
	
	void  OnMouseDown (){
		startPos = Input.mousePosition;
		startPos.z = transform.position.z - Camera.main.transform.position.z;
		startPos = Camera.main.ScreenToWorldPoint(startPos);
	}
	
	void  OnMouseUp (){
		endPos= Input.mousePosition;
		endPos.z = transform.position.z - Camera.main.transform.position.z;
		endPos = Camera.main.ScreenToWorldPoint(endPos);
		
		force= endPos - startPos;
		force.z = force.magnitude;
		force.Normalize();
		
		rb.AddForce(force * power);
		//ReturnBall();
=======
	public float curveAmount;
	public float height;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 sideDir = Vector3.Cross(transform.up, rb.velocity).normalized;
		rb.AddForce(sideDir * curveAmount);
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
		DetectSwipe();
	}

	void DetectSwipe()
	{
		Debug.Log ("swiped");
		Vector2 direction = swipeEnd - swipeBegin;
		Vector3 newDirection = new Vector3 (direction.x, height, direction.y);
		Vector3 velocity = newDirection * speed;
		rb.velocity = velocity;
>>>>>>> origin/master
	}
	
	// void ReturnBall (){
	//yield return new WaitForSeconds(4.0f);
	//transform.position = Vector3.zero;
	//rb.velocity = Vector3.zero;
	//}
}
