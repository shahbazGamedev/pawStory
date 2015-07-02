/**
Script Author : Srivatsan 
Description   : Color Lesson Dog Movements
**/
using UnityEngine;
using System.Collections;

public class DogMovementColorLesson : MonoBehaviour {
	public GameObject Dog;
	public Transform Target;
	Rigidbody rb;
	public bool isTargetRed;
	private Vector3 direction;
	public float Speed;
	private Animator dogAnim;
	public float speedDampTime;
	public Transform red;
		public Transform blue;
	public Transform green;
	public Transform yellow;
	public Transform ReturnPos;


	// Use this for initialization
	void Start () {
		rb=GetComponent<Rigidbody>();
		isTargetRed=false;
		dogAnim = GetComponent<Animator>();


	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
	void FixedUpdate()
	{
		if(isTargetRed==true)
		   {
			Debug.Log("working");
		
			Movement();
		
			dogAnim.SetFloat ("Walk",1f, speedDampTime, Time.deltaTime);

		}
	
	}
	public void RedMove()
	{
		Target=red;

		isTargetRed=true;
		
			

	}
	public void BlueMove()
	{
		Target=blue;
		isTargetRed=true;
	

	}
	public void GreenMove()
	{
		Target=green;
		isTargetRed=true;
	}
	public void YellowMove()
	{
	Target=yellow;
		isTargetRed=true;
	}
	void Movement()
	{
		if(Vector3.Distance(Dog.transform.position,Target.transform.position)>0f && isTargetRed==true)
			direction=Target.transform.position-Dog.transform.position;

		     transform.LookAt (Target);
		//rb.AddForce(transform.forward*Speed);
		rb.MovePosition(Vector3.MoveTowards (transform.position, Target.position, Speed* Time.deltaTime));
		 


	}
	void ReturnBack()
	{
		if(Vector3.Distance(Dog.transform.position,Target.transform.position)<0.3f )
			direction=ReturnPos.transform.position-Dog.transform.position;
		transform.LookAt (ReturnPos);
		rb.MovePosition(Vector3.MoveTowards (transform.position, ReturnPos.position, Speed* Time.deltaTime));

	}

}

