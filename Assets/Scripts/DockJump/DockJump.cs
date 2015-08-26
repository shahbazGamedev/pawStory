using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum DogStates
{
	Idle = 0,
	Sit = 1,
	Walk = 2,
	Jog = 3,

	RunStart = 10,
	Run = 11,
	RunEnd = 12,

	JumpStart = 15,
	Jump = 16,
	JumpEnd = 17,

	Win = 30,
	Lost = 31
};

public class DockJump : MonoBehaviour {

	// Dog
	public GameObject DogObj;
	public Animator DogAnim;
	float runSpeed;
	float jumpForce;
	float jumpspeed;
	public Transform DogStartTrans;
	Rigidbody rb;
	bool isInJumpZone;
	public DogStates dogState; // 0: sit, 1: Idle, 2: jog, 3: Run, 5: Jump, 
	int TotalIdleStates = 3;
	int IdleRandom;
	List<float> jumpDistList;
	Vector3 dir;// = new Vector3 (0, 0, 1);


	// Camera
	public GameObject CamObj;
	public Transform CamStartTrans;
	public Animator CamAnim;
	public bool isCamMove;


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
	public bool play;
	int jumpCount;
	int maxJumpCount;
	public Transform target;
	float speedDampTime = 0.1f;
	float dragRatio;
	float distance;
	bool isGameOver;


	void ChangeDogState (DogStates newDogState)
	{
		dogState = newDogState;
		switch(newDogState)
		{
		case DogStates.Idle:
			IdleRandom = Random.Range (0, TotalIdleStates);
			DogAnim.SetInteger ("IdleRandom", IdleRandom);
			break;
		case DogStates.Sit:
			break;
		case DogStates.Walk:
			break;
		case DogStates.Jog:
			break;
		case DogStates.RunStart:
		case DogStates.Run:
		case DogStates.RunEnd:
			break;
		case DogStates.JumpStart:
		case DogStates.Jump:
		case DogStates.JumpEnd:
			break;
		case DogStates.Win:
			break;
		};
		DogAnim.SetInteger( "DogState", (int)newDogState);
	}


	public void OnRestartGame ()
	{
		Debug.Log ("OnRestartGame");
		rb = GetComponent<Rigidbody> ();

		waitForTap = true;
		isGameOver = false;
		isInJumpZone = false;

		// dog
		ChangeDogState (DogStates.Idle);
		rb.isKinematic = true;
		transform.position = DogStartTrans.position;
		runSpeed = 5;
		jumpForce = 10;
		jumpCount = 0;
		maxJumpCount = 3;
		jumpDistList = new List<float> ();
		dir = new Vector3 (0, 0, 1);

		// Cam
		CamObj.transform.position = CamStartTrans.position;
		isCamMove = false;

		// HUD
		TapToPlayTxt.text = "Tap to Play";
		JumpCountTxt.text = "Chances: " + jumpCount + " / " + maxJumpCount;
		ScoreTxt.text = "Distance: " + distance;
		GameOverPanel.SetActive (false);
	}


	void OnEnable() {
		EventMgr.GameRestart += OnRestartGame;
	}
	
	
	void OnDisable() {
		EventMgr.GameRestart -= OnRestartGame;
	}


	void Start ()
	{
		OnRestartGame ();
		StartCoroutine(PlayGame());
	}


	void Update () 	{


		if (GameMgr.Inst.IsGamePaused ()) {
			return;
		} 
		// Detect Tap
		if ((Input.GetMouseButtonDown (0) || Input.GetKeyUp (KeyCode.Space)) 
            && !isGameOver && play ) 
		{
			if (waitForTap ) 
			{
				waitForTap = false;
				TapToPlayTxt.text = "Tap to Jump";
				ChangeDogState (DogStates.Run);
				CamAnim.enabled = false;

			}
			else if (isInJumpZone && dogState == DogStates.Run) 
			{
				ChangeDogState (DogStates.Jump);
				FinalJump ();
			}
		}
	}


	void FixedUpdate() 
	{
		if(dogState == DogStates.Run && !isGameOver)
		{
			DogObj.transform.position += Time.deltaTime * dir * runSpeed;
		}
	}


	void LateUpdate()
	{
		if(dogState == DogStates.Run && !isGameOver)
			CamObj.transform.position += new Vector3(0, 0, 1) * Time.deltaTime * 5;
	}


	void FinalJump()
	{
		rb.isKinematic = false;
		ChangeDogState (DogStates.Jump);
		rb.AddForce(new Vector3(0, 0.5f, 0.8f) * jumpForce, ForceMode.Impulse);
	}


	void OnTriggerStay(Collider other)
	{
		Debug.Log ("OnTriggerStay");
		isInJumpZone = true;
	}


	void OnTriggerExit(Collider other)
	{
		Debug.Log ("OnTriggerExit");
		isInJumpZone = false;
		rb.isKinematic = false;
	}


	void OnCollisionEnter(Collision collision)
	{
		// On dog landing to pool
		if(collision.gameObject.tag == "floor")
		{
			AnalyzeLanding();
		}
		if(jumpCount >= maxJumpCount)
		{
			GameOver();
		}
	}


	private void GameOver()
	{
		isGameOver = true;
		waitForTap = true;
		GameOverPanel.SetActive (true);

		TouchMat.SetActive(false);
		gameScreen.SetActive(false);

		float longDist = 0;
		for (int i = 0; i < jumpDistList.Count; i++)
		{
			if(longDist < jumpDistList[i])
			{
				longDist = jumpDistList[i];
			}
		}
		HighScoreTxt.text = "Longest Jump : " + (int)longDist + " ft";;
	}


	void AnalyzeLanding()
	{
		rb.isKinematic = true;

		if(dogState == DogStates.Run)// not a jump so foul
		{
			distance = -1;
		}
		else 
		{
			distance = Vector3.Distance(target.position, transform.position);
		}

		jumpDistList.Add (distance);
		StartCoroutine (NextRound ());
	}


	IEnumerator NextRound()
	{
		//ChangeDogState(DogStates.Run);
		//yield return new WaitForSeconds (0.5f);
		ChangeDogState (DogStates.RunEnd);
		yield return new WaitForSeconds (1f);
		string distStr = "Distance: Foul";
		if (distance == -1) {
			TapToPlayTxt.text = "";
			ChangeDogState (DogStates.Lost);
		} else {
			distStr = "Distance: " + (int)distance + " ft";
			TapToPlayTxt.text = "Cool";
			ChangeDogState(DogStates.Win);
		}
		// Update UI
		ScoreTxt.text = distStr;
		yield return new WaitForSeconds (2f);

		ChangeDogState(DogStates.Idle);
		yield return new WaitForSeconds (0.25f);

		jumpCount += 1;
		if (jumpCount >= maxJumpCount)
		{
			GameOver ();
		}
		else
		{
			waitForTap = true;
			isCamMove = false;
			isGameOver = false;
			isInJumpZone = false;
			
			// dog
			rb.velocity = Vector3.zero;
			rb.isKinematic = true;
			runSpeed = 5;
			jumpForce = 10;
			transform.position = DogStartTrans.position;
			
			// Cam
			CamObj.transform.position = CamStartTrans.position;
			
			// HUD
			TapToPlayTxt.text = "Tap to Play";
			JumpCountTxt.text = "Chances: " + jumpCount + " / " + maxJumpCount;
			GameOverPanel.SetActive (false);			
		}
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
	
	
	public void CamMove()
	{
		if(isCamMove)
		{
			isCamMove = false;
			Transition();
		}
	}
	IEnumerator PlayGame()
	{
		yield return new WaitForSeconds(6.0f);
		play=true;
		
	}
	
}

