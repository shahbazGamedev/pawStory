/**
Script Author : Vaikash 
Description   : Agility Game Manager
**/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AgilityManager : MonoBehaviour {

	#region Variables

	public static AgilityManager instanceRef;

	public GameObject dogRef;
	public GameObject gameOverPannel;
	public GameObject startBtn;
	public GameObject freezeSkillBtn;
	public Text timerText;
	public Text gameOverText;

	//public float totalTimeAvailable;
	public float totalCheckpoints;
	public float dogSpeedReduction;
	public float additionalTime;
	public float checkPointCount; // Always int values
	public float jumpForce;
	public float timeForSlideReset;
	public int hurdlesCollided;
	public float timeScale;

	TouchManager touchManagerRef;

	Vector3 jumpHeight;
	Vector3 startPos;
	Quaternion startRot;
	public Vector3 targetColliderSize;
	public Vector3 targetColliderCenter;
	Vector3 boxColliderSize;
	Vector3 boxColliderCenter;
    BoxCollider boxCollider;

    public float currentCheckpointTimer; // In secs
	int currentLane;

	bool startGame;
	bool gameOver;
	bool pauseTimer;

	Animator dogAnim;
	EllipseMovement dogCircuitManager;
	CameraFollow camRef;

	#endregion

    void OnEnable()
    {
        EventMgr.GameRestart += OnRestartGame;
    }


    void OnDisable()
    {
        EventMgr.GameRestart -= OnRestartGame;
        // Decouple listeners from events
        TouchManager.PatternRecognized -= SwipeEventHandler;
        EllipseMovement.LaneChangeComplete -= LaneChangeHandler;
        EllipseMovement.LapTriggered -= LapComplete;
    }

	// Use this for initialization
	void Start () 
	{
		instanceRef = this;
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		touchManagerRef = FindObjectOfType <TouchManager> ();
		camRef = FindObjectOfType <CameraFollow> ();
		dogAnim = dogRef.GetComponent<Animator>();
		dogCircuitManager = dogRef.GetComponent <EllipseMovement> ();

		currentLane = 1;
		jumpHeight = new Vector3(0 , jumpForce, 0);
		startPos = dogRef.transform.position;
		startRot = dogRef.transform.rotation;

		// Save default dog collider values
		var boxCollider = (BoxCollider)dogRef.GetComponent <Collider> ();
		boxColliderSize = boxCollider.size;
		boxColliderCenter = boxCollider.center;

		// Event Listeners
		TouchManager.PatternRecognized += SwipeEventHandler; //  Add Listener to touch manager
		EllipseMovement.LaneChangeComplete += LaneChangeHandler;
		EllipseMovement.LapTriggered += LapComplete;
	}

	// Update is called once per frame
	void Update () 
	{
		if (startGame) 
		{
			if (!pauseTimer) {
				currentCheckpointTimer -= Time.deltaTime * timeScale;
			}
			gameOver = (int)currentCheckpointTimer <= 0 ? true : false;
			if (currentCheckpointTimer < 10)
				timerText.text = "Time Remaining: 0" + (int)currentCheckpointTimer + " s";
			else
				timerText.text = "Time Remaining: " + (int)currentCheckpointTimer + " s";
		}

		if(gameOver)
		{
			StartCoroutine (GameOver ());
            gameOver = false;
			startGame = false;
		}
	}

	// Update checkpoint and timer
	void ReachedCheckPoint()
	{
		checkPointCount += 0.5f;
		currentCheckpointTimer += additionalTime;
		GetComponent <SpawnManager>().SpawnPowerUp ();
	}

	// Handle patternRecognized event
	void SwipeEventHandler(SwipeRecognizer.TouchPattern pattern)
	{
		if (startGame) 
		{
			if (pattern == SwipeRecognizer.TouchPattern.swipeUp) 
			{
				Jump ();
			}
			else if(pattern == SwipeRecognizer.TouchPattern.swipeLeft || pattern == SwipeRecognizer.TouchPattern.swipeUpLeft || pattern == SwipeRecognizer.TouchPattern.swipeDownLeft)
			{
				StartCoroutine (dogCircuitManager.ChangeLane (currentLane-=1));
			}
			else if(pattern == SwipeRecognizer.TouchPattern.swipeRight || pattern == SwipeRecognizer.TouchPattern.swipeUpRight || pattern == SwipeRecognizer.TouchPattern.swipeDownRight)
			{
				StartCoroutine (dogCircuitManager.ChangeLane (currentLane+=1));
			}
			else if (pattern == SwipeRecognizer.TouchPattern.swipeDown) 
			{
				Slide ();
			}
			else
			{
				Debug.Log (pattern);
			}
		}
	}

	// Handle LaneChangeEvent event
	void LaneChangeHandler()
	{
		if (currentLane < 0)
			currentLane = 0;
		else if (currentLane >= dogCircuitManager.circuitLaneData.Length)
			currentLane = dogCircuitManager.circuitLaneData.Length - 1;
	}

	// Called to make the dog jump
	void Jump()
	{
		if (dogCircuitManager.isGrounded)
		{
			dogAnim.SetTrigger ("Jump");
			dogRef.GetComponent<Rigidbody> ().AddForce (jumpHeight, ForceMode.Impulse);
		}
	}

	// Called to make dog slide
	void Slide()
	{
		if (dogCircuitManager.isGrounded) 
		{
			dogAnim.SetTrigger ("Slide");

			// Reduce dog collider size
			boxCollider = dogRef.GetComponent <Collider> () as BoxCollider;
			boxCollider.size=targetColliderSize;
			boxCollider.center = targetColliderCenter;
		}
	}

	// Reset box collider size afer sliding
	public void ResetColliderSize()
	{
		boxCollider = dogRef.GetComponent <Collider> () as BoxCollider;
		boxCollider.size = boxColliderSize;
		boxCollider.center = boxColliderCenter;
	}

	// Called when lap completes
	void LapComplete()
	{
		ReachedCheckPoint ();
	}

	// Resume timer 
	void ResumeTimer()
	{
		pauseTimer = false;
	}
	#region Coroutines

	// Activates End Game Display
	IEnumerator GameOver()
	{
        //Debug.Log("GameOver");
        freezeSkillBtn.SetActive(false);
		dogCircuitManager.updatePos = false;
		timerText.gameObject.transform.parent.gameObject.SetActive (false);
		gameOverPannel.SetActive (true);
		GetComponent <SpawnManager> ().spawnOn = false;
		gameOverText.text = "Lap Count: " + checkPointCount + "\n\n" + "Hurdles Collided: " 
			+ hurdlesCollided + "\n\nDistance Travelled: " + (int)EllipseMovement.alpha+" m"; // Game Play Stats
		yield return null;
	}

	// Displays countdown and starts game
	IEnumerator GameStart()
	{
		startBtn.SetActive (false);
		timerText.gameObject.transform.parent.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		dogCircuitManager.updatePos=true;
		yield return null;
	}

	#endregion

	#region BtnCallbacks

	// Start Btn callback
	public void startGameBtn()
	{
		//ReachedCheckPoint ();
		startGame = true;
		freezeSkillBtn.SetActive (true);
		StartCoroutine (GameStart ());
	}

	// Restart Btn callback
	public void OnRestartGame()
	{
        //Debug.Log("Restart");
		//gameOver = false;
		dogRef.transform.position = startPos;
		dogRef.transform.rotation = startRot;
		gameOverPannel.SetActive (false);
		startBtn.SetActive (true);
		EllipseMovement.alpha=0f;
		dogCircuitManager.ResetLane ();
		camRef.ResetPosition ();
		currentCheckpointTimer = 20;
		hurdlesCollided = 0;
		checkPointCount = 0;
		GetComponent <SpawnManager> ().spawnOn = true;
		GetComponent <SpawnManager> ().ClearHurdles ();
		StartCoroutine (GetComponent <SpawnManager> ().Spawner ());
	}

	// Freeze skill Btn callback
	public void freezeBtn()
	{
		// code to stop timer
		pauseTimer = true;
		freezeSkillBtn.SetActive (false);
		Invoke ("ResumeTimer", 3f);
	}

	// Home Btn Callback
	public void homeBtn()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}

	#endregion
}