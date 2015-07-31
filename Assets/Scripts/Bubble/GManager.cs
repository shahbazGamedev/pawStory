using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GManager : MonoBehaviour {
	//public static GManager instanceRef;

	public Text timer;
	public Text score;
	SpawnPoint[] spawnCollection;
	public float levelTime;
	bool gameStart;
	bool gameOver;
	public float maxBaloons;
	 public GameObject[] baloonCollection;
	public float baloonWaitTime;
	Transform baloonParent;
	GlobalValues gValues;
	int a;


//	public int playerScore;


	// Use this for initialization
	void Start () 
	{
		gValues = GlobalValues.instanceRef;
		Reset ();
		baloonParent = GameObject.Find ("BaloonHolder").transform;
		//Random.seed=(int)System.DateTime.Now.Ticks;
		FindSpawnPts ();


	}

	void Update()
	{

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
		if (gameStart) {
			levelTime -= Time.deltaTime;
		}


	}

	public void Reset()
	{
		gValues.Reset();
		levelTime = 20;
		maxBaloons = 3;
		gameStart = true;
		gameOver = false;
		a = 0;

	}

	void Spawner()
	{
		if (gValues.baloonsAtScene < maxBaloons && baloonWaitTime<=0) {
			SpawnPoint point = spawnCollection[Random.Range (0, spawnCollection.Length)];
			GameObject randomBaloonPrefab=baloonCollection[Random.Range(0,3)];
			GameObject thisInstance=(GameObject)Instantiate(randomBaloonPrefab, point.transform.position, Quaternion.identity);
			thisInstance.transform.parent =baloonParent;
			thisInstance.name="Baloon"+a;
			gValues.baloonsAtScene+=1;
			if(thisInstance!=null)
			{
				a+=1;
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

	}

	public void FindSpawnPts()
	{
		spawnCollection = FindObjectsOfType (typeof(SpawnPoint))as SpawnPoint[];
	}
}
