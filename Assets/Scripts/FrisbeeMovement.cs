﻿/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;

public class FrisbeeMovement : MonoBehaviour {
	private float power= 500.0f;
	Rigidbody rb;
	public Vector3 endPos;
	public Vector3 force;
	private Vector3 startPos;


	void Awake()
	{

	}
    void  Start ()
	{
		rb = GetComponent<Rigidbody>();

	}

	void Update()
	{


	}
	void FixedUpdate()
	{

	}
	
	void  OnMouseDown ()
	{
		startPos = Input.mousePosition;
		startPos.z = transform.position.z - Camera.main.transform.position.z;
		startPos = Camera.main.ScreenToWorldPoint(startPos);
	}
	
	void  OnMouseUp ()
	{
		endPos= Input.mousePosition;
		endPos.z = transform.position.z - Camera.main.transform.position.z;
		endPos = Camera.main.ScreenToWorldPoint(endPos);
		
		force= endPos - startPos;
		force.z = force.magnitude;
		force.Normalize();
		
		rb.AddForce(force * power);
		StartCoroutine(  ReturnFrisbee() );

	}
	
	IEnumerator ReturnFrisbee ()
	{
		yield return new WaitForSeconds(4.0f);
		Debug.Log("Return Ball");
		//yield return new WaitForSeconds(4.0f);
		transform.position = Vector3.zero;
		rb.velocity = Vector3.zero;
	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody)
		{
			Destroy (this.gameObject);



		}
	}
}




