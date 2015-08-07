using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BubbleGame : MonoBehaviour {
	public float levelTime;
	public float maxBaloons;
	public float baloonWaitTime;
	public GameObject[] baloonCollection;
	public GameObject gameOverPanel;
	public GameObject dog;
	public Text timer;
	public Text TxtScore;
	public Text highScore;
	public List<GameObject> spawnPosList;
	public List<GameObject> foodList;
	Transform baloonParent;
	int ballonIndex;
	bool gameOver;

	public int baloonsAtScene;
	public int playerScore;


	public float speed;
	public float distance;
	public Transform target;
	public int score;
	public bool isCollect;
	private Vector3 direction;
	private Animator dogAnim;
	Vector3 startingPos;
	Rigidbody rb;

	
	void Start () 
	{
		gameOver=false;
		Reset ();
		baloonParent = GameObject.Find ("BaloonHolder").transform;

		rb=GetComponent<Rigidbody>();
		dogAnim=GetComponent<Animator>();
		startingPos=transform.position;
	}


	void Update()
	{
		if(gameOver)
		{
			GameOver();
		}
		if(isCollect)
			Movement();
	}


	void FixedUpdate () 
	{
		if (!gameOver) {
			baloonWaitTime -= Time.deltaTime;
			levelTime -= Time.deltaTime;				

			if(levelTime <= 0f)
			{
				gameOver=true;
			}

			Spawner ();
			GuiUpdate ();
		}
	}


	public void Reset()
	{
		levelTime = 5;
		baloonsAtScene = 0;
		maxBaloons = 3;
		baloonWaitTime = 0;
		ballonIndex = 0;
		gameOver = false;
		dog.SetActive(true);
		gameOverPanel.SetActive(false);
	}


	void Spawner()
	{
		if (baloonsAtScene < maxBaloons && baloonWaitTime <= 0) 
		{
			int randPos = Random.Range (0, spawnPosList.Count);
			GameObject randomBaloonPrefab = baloonCollection[Random.Range(0, baloonCollection.Length)];
			GameObject thisInstance=(GameObject)Instantiate(randomBaloonPrefab, 
			                                                spawnPosList[randPos].transform.position, 
			                                                Quaternion.identity);
			if(thisInstance!=null)
			{
				thisInstance.transform.parent = baloonParent;
				thisInstance.name = "Baloon" + ballonIndex;
				baloonsAtScene += 1;
				ballonIndex += 1;
				baloonWaitTime = 2f;
			}
		}
	}


	void GuiUpdate()
	{
		timer.text = "Time: "+(int)levelTime;
		TxtScore.text = "Score: " + score;
	}


	void GameOver()
	{
		highScore.text = "Score: " + score;
		dog.SetActive(false);
		gameOverPanel.SetActive(true);
	}


	public void OnMainMenu()
	{
		GameMgr.instance.LoadScene(GlobalConst.Scene_MainMenu);
	}


	public void OnRestart()
	{
		GameMgr.instance.LoadScene(GlobalConst.Scene_Bubble);
	}


	public void Movement()
	{
		distance=Vector3.Distance(target.position,transform.position);
		direction=new Vector3(target.position.x,0,target.position.z);
		transform.LookAt(direction);
		if(distance>1f)
		{
			rb.AddForce(transform.forward*speed);
			dogAnim.SetFloat("Walk",1f);
		}
	}
	
	public void ScoreSystem()
	{
		Debug.Log ("collided");
		score += 1;
		dogAnim.SetFloat("Walk",0f);
		transform.position=startingPos;
	}
	
	
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.name.Contains("food"))
		{
			ScoreSystem();
			isCollect=false;
			Destroy(this.gameObject);
		}
		/*
		else
		{
			Debug.Log("collected");
			isCollect=true;
			target=transform;
		}
		*/
	}


}

