/**
Script Author : Srivatsan 
Description   : Color Lesson Dog Movements
**/
using UnityEngine;
using System.Collections;

public class DogMovementColorLesson : MonoBehaviour {
	public GameObject Dog;
	public GameObject Red;
	public GameObject Blue;
	public GameObject Green;
	public GameObject Yellow;
	public float distance;
	public Vector3 direction;
	public GameObject Target;
	private bool isTargetEnable;

	Rigidbody rb;
	public float Speed;



	// Use this for initialization
	void Start () {
		rb=GetComponent<Rigidbody>();

	
	}
	
	// Update is called once per frame
	void Update () {
//		if (Vector3.Distance (Dog.transform.position, Red.transform.position) >0f)
//		direction=Red.transform.position-Dog.transform.position;
//			transform.LookAt (direction);
//		rb.AddForce (transform.forward* Speed);
	
	}
	void FixedUpdate()
	{
		if(isTargetEnable=true)
		{
		Movement();
		}
	}
	public void RedMove()
	{
		Target=Red;
		isTargetEnable=true;

	}
//	public void BlueMove()
//	{
//		
//
//	}
//	public void GreenMove()
//	{
//		
//		
//	}
//	public void YellowMove()
//	{
//		
//		
//	}
	void Movement()
	{
		if (Vector3.Distance (Dog.transform.position, Target.transform.position)>0f)
			direction=Target.transform.position-Dog.transform.position;
		transform.LookAt (direction);
		rb.AddForce (transform.forward* Speed);


	}


}

