using UnityEngine;
using System.Collections;

public class DogMovementBubble : MonoBehaviour {
	Vector3 startingPos;
	public float distance;
	public Transform target;
	private Vector3 direction;
	Rigidbody rb;
	public float speed;
	private Animator dogAnim;
	public int score;
	public bool isCollect;


	// Use this for initialization
	void Start () 
	{
		rb=GetComponent<Rigidbody>();
		dogAnim=GetComponent<Animator>();
		startingPos=transform.position;
	}

	
	// Update is called once per frame
	void Update () 
	{
		if(isCollect)
		Movement();
	}


	public void Movement()
	{

		distance=Vector3.Distance(target.position,transform.position);
		direction=new Vector3(target.position.x,0,target.position.z);
		transform.LookAt(direction);
		if(distance>1f)
		{
			rb.AddForce(transform.forward*speed);
			dogAnim.SetFloat("Walk",1f);

		}
		}
	public void ScoreSystem()
	{


			Debug.Log ("collided");
			score+=1;
			dogAnim.SetFloat("Walk",0f);
		}
	}

		
