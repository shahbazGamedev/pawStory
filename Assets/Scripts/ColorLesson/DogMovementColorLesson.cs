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


		isTargetRed=true;
		
			

	}
	//public void BlueMove()
	//{
	

//	}
//	public void GreenMove()
//	{
//	}
//	public void YellowMove()
//	{
//		
//	}
	void Movement()
	{
		if(Vector3.Distance(Dog.transform.position,Target.transform.position)>0f && isTargetRed==true)
			direction=Target.transform.position-Dog.transform.position;

		     transform.LookAt (Target);
		//rb.AddForce(transform.forward*Speed);
		rb.MovePosition(Vector3.MoveTowards (transform.position, Target.position, Speed* Time.deltaTime));
		


	}


}

