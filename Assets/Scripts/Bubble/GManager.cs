using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GManager : MonoBehaviour {
	//public static GManager instanceRef;


	public float levelTime;
	public float maxBaloons;
	public float baloonWaitTime;
	public GameObject[] baloonCollection;
	public GameObject gameOverPanel;
	public GameObject dog;
	public bool isCollect;
	public Text timer;
	public Text score;
	public Text highScore;
	Transform baloonParent;
	GlobalValues gValues;
	int ballonIndex;
	SpawnPoint[] spawnCollection;
	bool gameStart;
	bool gameOver;




//	public int playerScore;


	// Use this for initialization
	void Start () 
	{
		gameOver=false;
		gValues = GlobalValues.instanceRef;
		Reset ();
		baloonParent = GameObject.Find ("BaloonHolder").transform;
		//Random.seed=(int)System.DateTime.Now.Ticks;
		FindSpawnPts ();
	}


	void Update()
	{
		if(gameOver)
		{
			GameOver();
		}
//		if(isCollect)
//		{
//			dog.GetComponent<DogMovementBubble>().Movement();
//			isCollect=false;
//		}
		}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (!gameOver) {
			TimerKeeper ();
			Spawner ();
			GuiUpdate ();
		}
		}

	void TimerKeeper()
	{
		if (gameStart) 
		{
			levelTime -= Time.deltaTime;

		}
		if(levelTime<=0f)
		{
			Debug.Log ("gameover");
			gameOver=true;
		}
		}


	public void Reset()
	{
		gValues.Reset();
		levelTime = 30;
		maxBaloons = 3;
		gameStart = true;
		gameOver = false;
		ballonIndex = 0;
		dog.SetActive(true);
		gameOverPanel.SetActive(false);
		}


	void Spawner()
	{
		if (gValues.baloonsAtScene < maxBaloons && baloonWaitTime<=0) 
		{
			SpawnPoint point = spawnCollection[Random.Range (0, spawnCollection.Length)];
			GameObject randomBaloonPrefab=baloonCollection[Random.Range(0,3)];
			GameObject thisInstance=(GameObject)Instantiate(randomBaloonPrefab, point.transform.position, Quaternion.identity);
		    thisInstance.transform.parent =baloonParent;
		    thisInstance.name="Baloon"+ballonIndex;
			gValues.baloonsAtScene+=1;
			if(thisInstance!=null)
			{
				ballonIndex+=1;
				baloonWaitTime=2f;
			}
		}
		baloonWaitTime -= Time.deltaTime;
	}

	
	void Awake() {
		//DontDestroyOnLoad(transform.gameObject);
		//instanceRef = this;
	}


	void GuiUpdate()
	{
		timer.text="Time: "+(int)levelTime;
		score.text="Score: "+dog.GetComponent<DogMovementBubble>().score;
	}


	public void FindSpawnPts()
	{
		spawnCollection = FindObjectsOfType (typeof(SpawnPoint))as SpawnPoint[];
	}


	void GameOver()
	{
		highScore.text="Score: "+dog.GetComponent<DogMovementBubble>().score;
		dog.SetActive(false);
		gameOverPanel.SetActive(true);
	}


	public void MainMenu()
	{
		Application.LoadLevel("MainMenu");
	}


	public void Restart()
	{
		Application.LoadLevel("bubbleTap");
	}
	}

