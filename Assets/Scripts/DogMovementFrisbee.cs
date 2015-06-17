/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;

public class DogMovementFrisbee : MonoBehaviour {
	public float jumpForce=100;
	public Vector3 jumpHeight;
	Rigidbody rb;
 
	public Transform target;
	private Animator dogAnim;

	Vector3 direction;
	void Awake()
	{
		jumpHeight = new Vector3(0 , jumpForce, 0);
		dogAnim = GetComponent<Animator>();

	}
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
		direction = new Vector3 (target.position.x, 0, target.position.z);
		transform.LookAt (direction);


	}



	}



	



		
	



