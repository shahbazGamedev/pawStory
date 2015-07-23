using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DockJump : MonoBehaviour {
	public GameObject dogRef;
	public GameObject MenuBtn;
	public GameObject RestartBtn;
	public GameObject PlayBtn;
	public GameObject Try1;
	public GameObject Try2;
	public GameObject Try3;
	public GameObject Stage;
	public GameObject Pool;
	public GameObject Floor;
	public GameObject Bg;
	public GameObject TouchMat;
	public float moveSpeed;
	public float jumpForce;
	public float jumpspeed;
	public Text chance1;
	public Text chance2;
	public Text chance3;
	public Text gameOver;
	public int chances;
	public Transform target;
	private Animator dogAnim;
	private Vector3 jumpHeight;
	private Vector3 dogPos;
	private float speedDampTime = 0.1f;
	private float dragRatio;
	private float dist;
	Vector2 swipeBegin;
	Vector2 swipeEnd;
	Rigidbody rb;
	bool isJumping=false;
	public bool isRunning;
	bool isCoroutine;
	bool isGameOver;
	public bool isCameraMove;
	public GameObject camera;
	public bool isCamReturn;


	void awake()
	{

	}

	// Use this for initialization
	void Start () {
		isCamReturn=false;
		isCameraMove=true;
		isRunning=false;
		isGameOver=false;
		dogAnim = dogRef.GetComponent<Animator> ();
		jumpHeight = new Vector3 (0, jumpForce, jumpspeed);
		rb = GetComponent<Rigidbody> ();
	    dogPos=new Vector3(1.19f,2.84f,-15.35f);
		MenuBtn.SetActive(false);
		RestartBtn.SetActive(false);
		Try1.SetActive(false);
		Try2.SetActive(false);
		Try3.SetActive(false);
		Bg.SetActive(false);


	}
	
	// Update is called once per frame
	void Update () {
		if(isGameOver==true)
		{
			GameOver();
		}
		movement();

		//detectSwipe();
		//distance();
	}


	void FixedUpdate() 
	{
		if(isRunning==true)
		{
		running();
		}
	}


	void jumping()
	{
		isRunning=false; 
		dogAnim.SetTrigger ("Jump");
		Debug.Log("WORKING FINE");
		rb.AddForce(jumpHeight,ForceMode.Impulse);
		}


	void running()
	{
		//if(isRunning=true)
		rb.drag = rb.velocity.magnitude * dragRatio;
		dogAnim.SetFloat ("Speed",1f, speedDampTime, Time.deltaTime);
		rb.AddForce (transform.forward * moveSpeed);

}


		IEnumerator ReturnDog ()
		{
		isCoroutine=true;
		yield return new WaitForSeconds(3.0f);
		transform.position = dogPos;
		rb.velocity = Vector3.zero;
		isCoroutine=false;
		}


	void OnTriggerStay()
	{
		Debug.Log ("triggered");
		camera.GetComponent<CameraMovement>().cameraMove();
		isJumping = true;

	}


	void OnTriggerExit()
	{
		isJumping = false;
		isRunning=false;
	}


	public void OnPointerDown(BaseEventData  data)
	{
		Debug.Log("Begins");
		PointerEventData e=(PointerEventData) data;
		swipeBegin=e.position;
		if(isJumping==true)
		{
		jumping();
		}
	}
	
	public void OnPointerUp(BaseEventData  data)
	{
		//Debug.Log("Ends");
		PointerEventData e=(PointerEventData) data;
		swipeEnd=e.position;

	}


	void movement()
	{
		if(Input.GetKeyDown(KeyCode.Space)&& isJumping==true)
		{
			jumping();
			//dogAnim.SetFloat ("Speed",0f, speedDampTime, Time.deltaTime);
			Debug.Log("WORKING");
			}
	}


//	void detectSwipe()
//	{


//		Vector2 direction=swipeEnd-swipeBegin;
//		direction.Normalize();
//		
//		//swipe upwards
//		if(direction.y > 0 &&  direction.x > -0.5f && direction.x < 0.5f)
//		{
//			Debug.Log("up swipe");
//			//if(isJumping)
//			//{
//
//
//		//	}
//				
//			
//		}
//		//swipe down
//		if(direction.y < 0 && direction.x > -0.5f && direction.x < 0.5f)
//		{
//			Debug.Log("down swipe");
//		
//		}
		//swipe left
		//if(direction.x < 0 && direction.y > -0.5f && direction.y < 0.5f)
		//{
		//	Debug.Log("left swipe");
		//}
		//swipe right
		//if(direction.x > 0 && direction.y > -0.5f && direction.y < 0.5f)
		//{
		////	Debug.Log("right swipe");
		//}

	//}


	void OnCollisionEnter(Collision collision)
	{
		if(collision.rigidbody)
		{
			isCamReturn=true;
			Debug.Log("new");
			chances-=1;
			dogAnim.SetFloat ("Speed",0f);
			ScoreSystem();

			if(!isCoroutine)
				StartCoroutine(ReturnDog());
			}
		if(chances==0)
		{
			StartCoroutine(EndGame());
			}
	}


	private void gameStart()
	{
		isRunning=true;

		}


	private void GameOver()
	{
	//Application.LoadLevel("MainMenu");
	gameOver.text="Game over";
			Debug.Log ("gameover");
		MenuBtn.SetActive(true);
		RestartBtn.SetActive(true);
		PlayBtn.SetActive(false);

		isRunning=false;
		Try1.SetActive(true);
		Try2.SetActive(true);
		Try3.SetActive(true);
		Stage.SetActive(false);
		Pool.SetActive(false);
		Floor.SetActive(false);
		dogRef.SetActive(false);
		Bg.SetActive(true);
		TouchMat.SetActive(false);
		}


	void distance()
	{
		dist=Vector3.Distance(target.position,transform.position);
		print("Jumping Distance: " + dist);
		}


	void ScoreSystem()
	{
		if(chances==4)
		{
			distance();
			chance1.text="Chance 1:"+dist;
			Try1.SetActive(true);
		}
		if(chances==2)
		{
			distance();
			chance2.text="Chance 2:"+dist;
			Try2.SetActive(true);
			Try1.SetActive(false);
		}
		if(chances==0)
		{
			distance();
		    chance3.text="Chance 3:"+dist;
			Try3.SetActive(true);
			Try2.SetActive(false);
		}

	}


	public void restart()
	{
		Application.LoadLevel("testSceneDockJump");
	}


	public void menu()
	{
		Application.LoadLevel("MainMenu");
	}


	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(3.0f);
		isGameOver=true;
	}


	}

	

