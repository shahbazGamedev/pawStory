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
	DogStates dogState; // 0: sit, 1: Idle, 2: jog, 3: Run, 5: Jump, 
	int TotalIdleStates = 3;
	int IdleRandom;
	List<float> jumpDistList;

	// Camera
	public GameObject CamObj;
	public Transform CamStartTrans;
	public bool isCamMove;
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
	float speedDampTime = 0.1f;
	float dragRatio;
	float distance;
	bool isGameOver;
	bool isFoulJump;
	bool canJump;


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


	public void Restart ()
	{
		waitForTap = true;
		isFoulJump = true;
		isCamReturn = false;
		isCamMove = false;
		isGameOver = false;
		isInJumpZone = false;

		// dog
		ChangeDogState (DogStates.Idle);
		rb.isKinematic = true;
		canJump = false;
		runSpeed = 5;
		jumpForce = 10;
		transform.position = DogStartTrans.position;
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
		rb = GetComponent<Rigidbody> ();
		Restart ();
	}


	void Update () 
	{
		if (waitForTap)
		{
			if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
			{
				waitForTap = false;
				TapToPlayTxt.text = "Tap to Jump";
				ChangeDogState(DogStates.Run);
			}
		}else{
			if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)
			   && isInJumpZone && dogState != DogStates.Jump )
			{
				ChangeDogState(DogStates.Jump);
				FinalJump();
			}
		}
	}


	void FixedUpdate() 
	{
		if(dogState == DogStates.Run && !waitForTap)
		{
			DogObj.transform.position += Time.deltaTime * new Vector3 (0, 0, 1) * runSpeed;
		}
	}


	void LateUpdate()
	{
		if(dogState == DogStates.Run)
			CamObj.transform.position += new Vector3(0, 0, 1) * Time.deltaTime * 5;
	}


	void FinalJump()
	{
		canJump = false;
		isFoulJump = false;
		rb.isKinematic = false;
		ChangeDogState (DogStates.Jump);
		rb.AddForce(new Vector3(0, 0.7f, 1) * jumpForce, ForceMode.Impulse);
	}


	void OnTriggerStay(Collider other)
	{
		isInJumpZone = true;
	}


	void OnTriggerExit(Collider other)
	{
		isInJumpZone = false;
		rb.isKinematic = false;
	}


	void OnCollisionEnter(Collision collision)
	{
		// On dog landing to pool
		if(collision.gameObject.tag == "floor")
		{
			waitForTap = true;
			AnalyzeLanding();
		}
		if(jumpCount >= maxJumpCount)
		{
			GameOver();
		}
	}


	private void GameOver()
	{
		GameOverPanel.SetActive (true);

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


	void AnalyzeLanding()
	{
		rb.isKinematic = true;
		GetComponent<Rigidbody>().detectCollisions = false;

		// jump analysis
		if (isFoulJump) {
			distance = -1;
		} else {
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

		CamObj.transform.position = CamStartTrans.position;
		transform.position = DogStartTrans.position;

		jumpCount += 1;
		if (jumpCount >= maxJumpCount)
		{
			GameOver ();
		}
		else 
		{
			ChangeDogState(DogStates.Idle);
			rb.velocity = Vector3.zero;
			TapToPlayTxt.text = "Tap to Play";
			JumpCountTxt.text = "Chances: " + jumpCount + " / " + maxJumpCount;
			rb.isKinematic = true;
			isCamMove = true;
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
	
	
	public void CamMove()
	{
		if(isCamMove)
		{
			isCamMove = false;
			Transition();
		}
	}
	
}

