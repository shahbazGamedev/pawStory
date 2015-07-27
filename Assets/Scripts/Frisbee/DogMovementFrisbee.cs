/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DogMovementFrisbee : MonoBehaviour {
	public Transform[] spawnPoint;
	public float jumpForce=200f;
	public Transform Dog;
	public GameObject dog;
	public Transform target;
	public GameObject MenuBtn;
	public GameObject RestartBtn;
	public GameObject Frisbee;
	public GameObject gameOver;
	public int Score;
	public GameObject Floor;
	public bool isMoving;
	public bool isGameover;
	public Text chance;
	public int chances;
	public Text ChanceUi;
	public Transform targetMove;
	public int spawnValue=0;
	public bool isSpawn;
	public GameObject FrisbeeAttached;
	public int MaxChances;
	public int Life;
	public Text score;
	public Text life;
	public GameObject startPanel;
	private Animator dogAnim;
	private Vector3 frisbeedirection;
	private Vector3 dogPos;
	private float distance;
	private float speed=2f;
	private Vector3 jumpHeight;
	private Vector3 direction;
	bool isCatching;
	Rigidbody rb;
	Vector3 position;




	void Awake()
	{
		jumpHeight = new Vector3(0 , jumpForce, 0);
		dogAnim = GetComponent<Animator>();
		dogPos = new Vector3 (-0.2f, 0.035f, 9.1f);
		}


	// Use this for initialization
	void Start ()
	{
		startPanel.SetActive(true);
		FrisbeeAttached.SetActive(false);
		SpawnValueReset();
		gameOver.SetActive(false);
		isMoving=true;
	    MenuBtn.SetActive(false);
		RestartBtn.SetActive(false);
		rb = GetComponent<Rigidbody> ();
		isGameover=false;
		Movement();
		}


	// Update is called once per frame
	void Update () 
	{
		if(isGameover==true)
		{
		GameOver();
		}
		if(isMoving)
		{
		Movement();
		}

		if(isCatching)
		{
			Debug.Log("nowcatching");
		FrisebeeCatch();

         }
		if(isSpawn)
		{
			SpawnValueReset();
			isSpawn=false;
		}
		ChanceUi.text="No Of Chances:"+chances + " / "+MaxChances;
		life.text="Life : "+Life;
		score.text="Score: "+Score;
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


//	 public IEnumerator ReturnDog ()
//	{
//		yield return new WaitForSeconds(2.0f);
//		transform.position = dogPos;
//		rb.velocity = Vector3.zero;
//	}
//}


//	void MovePosition()
//	{
//		if(Score==0&& isMoving==true )
//		{
//
//			Debug.Log ("pos1");
//			Pos1=Vector3.Distance(movePosition1.position,transform.position);
//			direction1=new Vector3(movePosition1.position.x,0,movePosition1.position.z);
//			transform.LookAt(direction1);
//			dogAnim.SetFloat("Walk",1f);
//			float step=speed*Time.deltaTime;
//			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition1.position, step));
//		
//		
//		if(Pos1<1f)
//			{
//				isMoving=false;
//				transform.LookAt(direction);
//				dogAnim.SetFloat("Walk",0f);
//
//			}
//		}
//
//		if(Score==1 && isMoving==true)
//		{
//			//isMoving=true;
//			Debug.Log ("pos2");
//			Pos2=Vector3.Distance(movePosition2.position,transform.position);
//			direction2=new Vector3(movePosition2.position.x,0,movePosition2.position.z);
//			transform.LookAt(direction2);
//			dogAnim.SetFloat("Walk",1f);
//			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition2.position, speed* Time.deltaTime));
//		
//			if(Pos2<1f)
//			{
//				isMoving=false;
//				transform.LookAt(direction);
//				dogAnim.SetFloat("Walk",0f);
//			}
//		}
//		if(Score==2 && isMoving==true)
//		{
//
//			Debug.Log ("pos3");
//			Pos3=Vector3.Distance(movePosition3.position,transform.position);
//			direction3=new Vector3(movePosition3.position.x,0,movePosition3.position.z);
//			transform.LookAt(direction3);
//			dogAnim.SetFloat("Walk",1f);
//			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition3.position, speed* Time.deltaTime));
//		
//		if(Pos3<1f)
//		{
//			isMoving=false;
//			transform.LookAt(direction);
//			dogAnim.SetFloat("Walk",0f);
//		}
//	}
//	}
	

//	void movement()
//	{
//		if(Vector3.Distance(Dog.transform.position,movePosition1.transform.position)>0.2f && Score==0)
//		{
//			Debug.Log("not working");
//			direction=movePosition1.transform.position-Dog.transform.position;
//			dogAnim.SetFloat("Walk",1f);
//			transform.LookAt (movePosition1);
//		//rb.AddForce(transform.forward*Speed);
//			rb.MovePosition(Vector3.MoveTowards (transform.position, movePosition1.position, speed));
//		}
//	}

	public void Movement()
	{

//		spawnValue=Random.Range(0,3);
//		targetMove=spawnPoint[spawnValue];
		distance=Vector3.Distance(targetMove.position,transform.position);
		if(distance>1f)
		{
			transform.LookAt(direction);
		}
		direction=new Vector3(targetMove.position.x,0,targetMove.position.z);

		dogAnim.SetFloat("Walk",1f);
		float step=speed*Time.deltaTime;

		position=targetMove.position;
		position.y=transform.position.y;
		rb.MovePosition(Vector3.MoveTowards (transform.position, position, step));

		if(distance<1f)
		{
			Debug.Log("Hi");
		dogAnim.SetFloat("Walk",0f);
		transform.LookAt(frisbeedirection);
		isMoving=false;
		}
	}
	void FrisebeeCatch()
	{
		frisbeedirection = new Vector3 (target.position.x, 0f, target.position.z);
		if((transform.position-target.position).magnitude>2f)
			transform.LookAt (frisbeedirection);
		ChanceUi.text="No Of Chances:"+chances;
	


	}

	public void GameOver()
	{
		//if(Score>=3)
		//{
		    startPanel.SetActive(false);
			gameOver.SetActive(true);
			dog.SetActive(false);
			Frisbee.SetActive(false);
			RestartBtn.SetActive(true);
			MenuBtn.SetActive(true);
			chance.text="SCORE: "+Score;

		//}
	}
	public void Restart()
	{

		Application.LoadLevel("DiscDogs");
	}


	public void Menu()
	{

		Application.LoadLevel("MainMenu");
	}
	public void SpawnValueReset()
	{
		spawnValue=Random.Range(0,3);
		targetMove=spawnPoint[spawnValue];
	}
	public void OnRestart()
	{
		Score=0;
		chances=0
	    
	}
}


		







			


	



		
	



