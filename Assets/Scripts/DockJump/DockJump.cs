using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DockJump : MonoBehaviour {
	public GameObject dogRef;
	public GameObject MenuBtn;
	public GameObject RestartBtn;
	public GameObject PlayBtn;
	public GameObject Stage;
	public GameObject Pool;
	public GameObject Floor;
	public GameObject Bg;
	public GameObject TouchMat;
	public float moveSpeed;
	public float jumpForce;
	public float jumpspeed;
	public Text gameOver;
	public int chances;
	public Transform target;
	private Animator dogAnim;
	private Vector3 jumpHeight;
	private Vector3 dogPos;
	private float speedDampTime = 0.1f;
	private float dragRatio;
	private float distance;
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
	public Text Score;
	public Text chance;
	public int maxChances;
	public float[] scoreSystem;
	public int highScore;
	public int score;
	bool scoreUpdate;
	public Text highScoreTxt;
	public bool isFoulJump;

	void awake()
	{

	}

	// Use this for initialization
	void Start () {
		isFoulJump=true;
		scoreUpdate=true;
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
		Bg.SetActive(false);
		chance.text="Chances: "+chances+ " / "+maxChances;
		Score.text="Distace: " + distance;


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
		isFoulJump=false;
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
		chance.text="Chances: "+chances+ " / "+maxChances;
		isCameraMove=true;
		GetComponent<Rigidbody>().detectCollisions=true;
		scoreUpdate=true;

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
		if(collision.gameObject.tag=="floor")
		{

			if(scoreUpdate)
			{
				isCamReturn=true;
				Debug.Log("new");
				chances+=1;
				dogAnim.SetFloat ("Speed",0f);
				GetComponent<Rigidbody>().detectCollisions=false;
			ScoreSystem();
				scoreUpdate=false;
			}

			if(!isCoroutine)
				StartCoroutine(ReturnDog());
			}
		if(chances>=maxChances)
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
		Stage.SetActive(false);
		Pool.SetActive(false);
		Floor.SetActive(false);
		dogRef.SetActive(false);
		Bg.SetActive(true);
		TouchMat.SetActive(false);
		if(scoreSystem[1]>scoreSystem[2])
		{
			if(scoreSystem[1]>scoreSystem[3])
			{
				highScoreTxt.text="Longest Jump : "+(int)scoreSystem[1]+" ft ";
			}
			else
				highScoreTxt.text="Longest Jump : "+(int)scoreSystem[3]+" ft ";
		}
		else
		{
			if(scoreSystem[2]>scoreSystem[3])
			{
				highScoreTxt.text="Longest Jump : "+(int)scoreSystem[2]+" ft ";
			}
			else
				highScoreTxt.text="Longest Jump : "+(int)scoreSystem[3]+" ft ";
		}


		}


	void Distance()
	{
		distance=Vector3.Distance(target.position,transform.position);
		//print("Jumping Distance: " + distance);
		}


	void ScoreSystem()
	{
		//if(chances==4)
	//	{
		Debug.Log("hello");
			Distance();
			Score.text="Distace: " + (int)distance+ " ft ";
		scoreSystem[chances]=distance;
		if(isFoulJump)
		{
			Score.text="Distace: FOUL JUMP";
		}


	//	}
	

	}


	public void restart()
	{
		Application.LoadLevel("DockJump");
	}


	public void menu()
	{
		Application.LoadLevel("MainMenu");
	}


	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(3f);
		isGameOver=true;
	}


	}

	

