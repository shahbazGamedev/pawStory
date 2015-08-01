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

	public GameObject[] obstacleCollection;
	public PowerUpCollection[] collectibleCollection;
	public SpawnPoint[] spawnPts;
	public GameObject dogRef;
	public GameObject gameOverPannel;
	public GameObject startBtn;
	public Text timerText;

	public float totalTimeAvailable;
	public float totalCheckpoints;
	public float dogSpeedReduction;
	public float additionalTime;
	public float checkPointCount; // Always int values

	float currentTimer; // In secs
	float currentCheckpointTimer; // In secs

	bool startGame;
	bool gameOver;
	bool spawnObstacle;
	bool spawnCollectible;

	// Emulates hashtable in editor - PowerUp
	[System.Serializable]
	public struct PowerUpCollection {
		public PowerUp key;
		public GameObject valueRef;
		public float coolDown;
	}

	// PowerUp types
	public enum PowerUp
	{
		SlowMotion,
		TurboRun,
		Freeze
	};

	#endregion

	// Use this for initialization
	void Start () {
		spawnPts = FindObjectsOfType<SpawnPoint> ();
		dogRef = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (startGame) 
		{
			//currentTimer += Time.deltaTime;
			currentCheckpointTimer -= Time.deltaTime;
			//gameOver = currentTimer > totalTimeAvailable ? true : false;
			gameOver = currentCheckpointTimer <= 0 ? true : false;
			if (currentCheckpointTimer < 10)
				timerText.text = "Time Remaining: 0" + (int)currentCheckpointTimer + " s";
			else
				timerText.text = "Time Remaining: " + (int)currentCheckpointTimer + " s";
		}

		if(gameOver)
		{
			StartCoroutine (GameOver ());
			startGame = false;
		}
	}


	void ReachedCheckPoint()
	{
		checkPointCount += 1;
		currentCheckpointTimer += additionalTime;
	}

	#region Coroutines

	// Activates End Game Display
	IEnumerator GameOver()
	{
		dogRef.GetComponent <EllipseMovement> ().updatePos = false;
		//yield return new WaitForSeconds (2);
		timerText.gameObject.transform.parent.gameObject.SetActive (false);
		gameOverPannel.SetActive (true);
		yield return null;
	}

	// Displays countdown and starts game
	IEnumerator GameStart()
	{
		startBtn.SetActive (false);
		timerText.gameObject.transform.parent.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		dogRef.GetComponent <EllipseMovement>().updatePos=true;
		yield return null;
	}

	#endregion

	#region BtnCallbacks

	// Start Btn callback
	public void startGameBtn()
	{
		ReachedCheckPoint ();
		startGame = true;
		StartCoroutine (GameStart ());
	}
		

	#endregion
}
