/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;

public class DogMovementFrisbee : MonoBehaviour {
	public float jumpForce=200f;
	public Transform Dog;
	public GameObject dog;
	public GameObject headRef;
	public Transform target;
	private Animator dogAnim;
	private Vector3 direction;
	private Vector3 dogPos;
	Rigidbody rb;
	public int Score;
	public Transform movePosition1;
	public Transform movePosition2;
	public Transform movePosition3;
	private float Pos1;
	private float Pos2;
	private float Pos3;
	private float speed=2f;
	private Vector3 jumpHeight;
	private Vector3 direction1;
	private Vector3 direction2;
	private Vector3 direction3;
	public bool isMoving;
	bool isCatching;
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
		MovePosition();
		if(isCatching)
		{
			Debug.Log("nowcatching");
		FrisebeeCatch();
         }

	}

	void FixedUpdate()
	{
		//movement();
	}


public void jumpingRight(Vector3 force)
	{
		dogAnim.SetTrigger("RightJump");
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


	void MovePosition()
	{
		if(Score==0)
		{
			isMoving=true;
			Debug.Log ("pos1");
			Pos1=Vector3.Distance(movePosition1.position,transform.position);
			direction1=new Vector3(movePosition1.position.x,0,movePosition1.position.z);
			transform.LookAt(direction1);
			dogAnim.SetFloat("Walk",1f);
			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition1.position, speed* Time.deltaTime));
		}
		if(Score==1)
		{
			isMoving=true;
			Debug.Log ("pos2");
			Pos2=Vector3.Distance(movePosition2.position,transform.position);
			direction2=new Vector3(movePosition2.position.x,0,movePosition2.position.z);
			transform.LookAt(direction2);
			dogAnim.SetFloat("Walk",1f);
			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition2.position, speed* Time.deltaTime));
		}
		if(Score>2)
		{
			isMoving=true;
			Debug.Log ("pos3");
			Pos3=Vector3.Distance(movePosition3.position,transform.position);
			direction3=new Vector3(movePosition3.position.x,0,movePosition3.position.z);
			transform.LookAt(direction3);
			dogAnim.SetFloat("Walk",1f);
			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition3.position, speed* Time.deltaTime));
		}
	}
	 

//	void movement()
//	{
//		if(Vector3.Distance(Dog.transform.position,movePosition1.transform.position)>0.2f && Score==0)
//		{
//			Debug.Log("not working");
//			direction=movePosition1.transform.position-Dog.transform.position;
//			dogAnim.SetFloat("Walk",1f);
//			transform.LookAt (movePosition1);
//			//rb.AddForce(transform.forward*Speed);
//			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition1.position, speed));
//		}
//	}


	void FrisebeeCatch()
	{
		direction = new Vector3 (target.position.x, 0f, target.position.z);
		if((transform.position-target.position).magnitude>2f)
			transform.LookAt (direction);


	}


	void OnCollisionEnter(Collision collision)
	{
	if(collision.gameObject.tag=="MovePoints")
		{
			Debug.Log ("collided");

			isMoving=false;
			if(isMoving==false)
			{
				Debug.Log ("itcomeshere");
			//	dogAnim.SetFloat("Walk",0f);
				isCatching=true;
			}


		}
	}
		

}





			


	



		
	



