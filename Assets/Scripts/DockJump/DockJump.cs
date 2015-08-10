using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DockJump : MonoBehaviour {

	// Dog
	public GameObject DogObj;
	public Animator DogAnim;
	public float moveSpeed;
	public float jumpForce;
	public float jumpspeed;
	public Transform DogStartTrans;
	Rigidbody rb;
	bool isJumping;
	bool isRunning;
	int DogState = 0; // 0: sit, 1: Stand, 2: jog, 3: Run, 5: Jump, 
	List<float> jumpDistList;

	// Camera
	public GameObject CamObj;
	public Transform CamStartTrans;
	public bool isCameraMove;
	public bool isCamReturn;

	// HUD
	public GameObject GameOverPanel;
	public GameObject TouchMat;
	public GameObject gameScreen;

	public Text TapToPlayTxt;
	public Text ScoreTxt;
	public Text JumpCountTxt;
	public Text HighScoreTxt;
	int highScore;
	int score;

	bool waitForTap;
	int jumpCount;
	int maxJumpCount;
	public Transform target;
	Vector3 jumpHeight;
	float speedDampTime = 0.1f;
	float dragRatio;
	float distance;
	bool isGameOver;
	bool isFoulJump;
	bool canJump;

	public void Restart ()
	{
		waitForTap = true;
		isFoulJump = true;
		isCamReturn = false;
		isCameraMove = false;
		isGameOver = false;

		// dog
		isRunning = false;
		canJump = false;
		transform.position = DogStartTrans.position;
		rb = GetComponent<Rigidbody> ();
		jumpHeight = new Vector3 (0, jumpForce, jumpspeed);
		jumpDistList = new List<float> ();
		jumpCount = 0;
		maxJumpCount = 3;

		// Cam
		CamObj.transform.position = CamStartTrans.position;

		// HUD
		TapToPlayTxt.text = "Tap to Play";
		JumpCountTxt.text = "Chances: " + jumpCount + " / " + maxJumpCount;
		ScoreTxt.text = "Distance: " + distance;
		GameOverPanel.SetActive (false);
	}


	void Start ()
	{
		Restart ();
	}


	void Update () 
	{
		if (waitForTap)
		{
#if UNITY_ANDROID || UNITY_IOS 
			if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
			{
				waitForTap = false;
				TapToPlayTxt.text = "Tap to Jump";
				if(isJumping && canJump)
				{
					Jumping();
				}
				isRunning = true;
			}
#endif
		}else{
			Movement ();
		}
	}


	void FixedUpdate() 
	{
		if(isRunning && !waitForTap)
		{
			Running();
		}
	}


	void LateUpdate()
	{
		if(isRunning)
			CamObj.transform.position += new Vector3(0, 0, 1) * Time.deltaTime * 5;
	}


	void Jumping()
	{
		canJump = false;
		isFoulJump = false;
		isRunning = false; 
		DogAnim.SetTrigger ("Jump");
		rb.AddForce(jumpHeight, ForceMode.Impulse);
	}


	void Running()
	{
		rb.drag = rb.velocity.magnitude * dragRatio;
		DogAnim.SetFloat ("Speed", 1f, speedDampTime, Time.deltaTime);
		rb.AddForce (transform.forward * moveSpeed);
	}

	void OnTriggerStay()
	{
		isJumping = true;
	}


	void OnTriggerExit()
	{
		isJumping = false;
		isRunning = false;
	}


	void Movement()
	{
		if(Input.GetKeyDown(KeyCode.Space) && isJumping)
		{
			Jumping();
		}
	}


	void OnCollisionEnter(Collision collision)
	{
		// On dog landing to pool
		if(collision.gameObject.tag == "floor")
		{
			waitForTap = true;
		
			DogAnim.SetFloat ("Speed", 0f);
			rb.isKinematic=true;
			GetComponent<Rigidbody>().detectCollisions = false;

			AnalyzeJump();
		}
		if(jumpCount >= maxJumpCount)
		{
			GameOver();
		}
	}


	private void GameOver()
	{
		GameOverPanel.SetActive (true);

		isRunning=false;
		TouchMat.SetActive(false);
		gameScreen.SetActive(false);

		float longDist = jumpDistList [0];
		for (int i = 1; i< jumpDistList.Count; i++)
		{
			if(longDist < jumpDistList[i])
			{
				longDist = jumpDistList[i];
			}
		}
		HighScoreTxt.text = "Longest Jump : " + (int)longDist + " ft";;
	}


	void AnalyzeJump()
	{			
		DogAnim.SetFloat ("Speed",0f);
		rb.isKinematic = true;
		GetComponent<Rigidbody>().detectCollisions = false;

		// jump analysis
		distance = Vector3.Distance(target.position, transform.position);
		string distStr = "Distance: Foul";
		if (isFoulJump) {
			distance = 0;
			TapToPlayTxt.text = "";
		} else {
			distStr = "Distance: " + (int)distance + " ft";
			TapToPlayTxt.text = "Cool";
		}

		// Update UI
		ScoreTxt.text = distStr;

		jumpDistList.Add (distance);
		NextRound ();
	}


	void NextRound()
	{
		CamObj.transform.position = CamStartTrans.position;
		transform.position = DogStartTrans.position;

		jumpCount += 1;
		if (jumpCount >= maxJumpCount) {
			GameOver ();
		}
		else 
		{
			rb.velocity = Vector3.zero;
			TapToPlayTxt.text = "Tap to Play";
			JumpCountTxt.text = "Chances: " + jumpCount + " / " + maxJumpCount;
			GetComponent<Rigidbody>().detectCollisions = true;
			rb.isKinematic = false;
			isCameraMove = true;
			canJump = true;
			isFoulJump = true;
		}
	}


	public void OnRestartBtn()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_DockJump);
	}


	public void OnMainMenuBtn()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_MainMenu);
	}


	public void Transition()
	{
		float transitionDuration = 2f;
		float t = 0.0f;
		Vector3 startingPos = transform.position;
		while (t < 1.0f)
		{
			t += Time.deltaTime * (Time.timeScale / transitionDuration);
			transform.position = Vector3.MoveTowards(startingPos, target.position, t);
		}
	}
	
	
	public void cameraMove()
	{
		if(isCameraMove)
		{
			isCameraMove = false;
			Transition();
		}
	}
	
}

