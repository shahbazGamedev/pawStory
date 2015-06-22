/**
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
	public GameObject dog;
	public GameObject frisbee;
	bool isJumping=false;
	public Vector3 direction;
	private float shootingAngle=45f;
	private float distance;
	private float angleRadians;
	private float velocity;
	private Vector3 frisbeeForce;



	void Awake()
	{

	}
    void  Start ()
	{
		rb = GetComponent<Rigidbody>();

	}

	void Update()
	{


		if (Vector3.Distance (dog.transform.position, frisbee.transform.position) < 2f && isJumping==false)
		{
			direction =   frisbee.transform.position- dog.transform.position;
			distance = direction.magnitude;
		 angleRadians=shootingAngle * Mathf.Deg2Rad;
			//direction.y = distance * Mathf.Tan(angleRadians);
			//distance += height / Mathf.Tan(angleRadians);
			velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angleRadians));
			frisbeeForce = velocity * direction.normalized;
			dog.GetComponent<DogMovementFrisbee> ().jumping (frisbeeForce);
			isJumping=true;
		}


	

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
		GetComponent<MeshRenderer>().enabled=true;
		isJumping = false;

	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody)
		{
//			Destroy (this.gameObject);
			GetComponent<MeshRenderer>().enabled=false;



		}
	}
	//void OnTriggerEnter()
	//{

	//	dog.GetComponent<DogMovementFrisbee>().jumping();
	//
	//	Debug.Log ("jumping");

//	}

}




