/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;

public class DogMovementFrisbee : MonoBehaviour {
	public float jumpForce=200f;
	public Vector3 jumpHeight;
	public GameObject dog;
	Rigidbody rb;
	public Transform target;
	private Animator dogAnim;
	public GameObject headRef;
	Vector3 direction;
	private Vector3 dogPos;
	//public GameObject frisbee;


	void Awake()
	{
		jumpHeight = new Vector3(0 , jumpForce, 0);
		dogAnim = GetComponent<Animator>();
		dogPos = new Vector3 (-0.2f, 0.035f, 9.1f);

	}
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
		direction = new Vector3 (target.position.x, 0f, target.position.z);
		if((transform.position-target.position).magnitude>2f)
			transform.LookAt (direction);
		//StartCoroutine (ReturnDog());



	}
public void jumping(Vector3 force)
	{

		dogAnim.SetTrigger("Jump");

		rb.velocity=(force);

	}
	public void jumpingLeft(Vector3 force)
	{

		dogAnim.SetTrigger ("LeftJump");
		rb.velocity = (force);
	}
	//IEnumerator ReturnDog ()
	//{
		//yield return new WaitForSeconds(5.0f);
		//transform.position = dogPos;
		//rb.velocity = Vector3.zero;

//}


}


			


	



		
	



