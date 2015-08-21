using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BubbleGame : MonoBehaviour {
	float levelTime;
	float maxBaloons;
	float nextBaloonTime;
	public GameObject gameOverPanel;
	public GameObject dog;
	public Text timer;
	public Text TxtScore;
	public Text HighScore;
	public List<GameObject> SpawnPosList;
	public List<GameObject> BallonList;
	public List<GameObject> BallonItemList;
	public Transform BaloonParent;
	int baloonIndex;
	bool gameOver;
	int baloonsAtScene;
	int playerScore;
	float speed;
	float distance;
	Vector3 targetPos;
	int score;
	bool isCollect;
	Vector3 dir;
	Animator dogAnim;
	Vector3 startingPos;
	Rigidbody rb;
	int baloonItemCount = 0;
	float oneSecTimer = 0;

	int dogState = 0;// 0= watching, 1= collecting


	void OnEnable()
	{
		EventMgr.SetPos += OnSetPos;
		EventMgr.GameRestart += OnRestartGame;
	}


    void OnDisable()
	{
        Debug.Log("Disable");
		EventMgr.SetPos -= OnSetPos;
		EventMgr.GameRestart -= OnRestartGame;
	}

	
	void Start () 
	{
		OnRestartGame ();
	}


	void Update()
	{
		if(dogState == 1)
			CollectItem();
	}


	void FixedUpdate () 
	{
		if (!gameOver && !GameMgr.Inst.IsGamePaused()) 
		{
			oneSecTimer += Time.deltaTime;

			if(oneSecTimer >= 1)
			{
				OneSecTimer();
			}

			Spawner ();
		}
	}


	void OneSecTimer()
	{
		nextBaloonTime -= 1;
		levelTime -= 1;				

		GuiUpdate ();

		if(levelTime <= 0f)
		{
			gameOver = true;
			GameOver();
		}
		oneSecTimer = 0;
	}


	public void OnRestartGame()
	{
        Debug.Log("OnRestartGame");

		gameOver=false;
		rb=GetComponent<Rigidbody>();
		dogAnim=GetComponent<Animator>();
		startingPos=transform.position;

		levelTime = 30;
		baloonsAtScene = 0;
		maxBaloons = 10;
		nextBaloonTime = levelTime / maxBaloons;
		baloonIndex = 0;
		speed = 10;
		dogState = 0;
		gameOver = false;
		dog.SetActive(true);
		gameOverPanel.SetActive(false);
	}


	void Spawner()
	{
		if (baloonsAtScene < maxBaloons && nextBaloonTime <= 0) 
		{
			GameObject curBaloon = GetBaloon();
			if(curBaloon!=null)
			{
				baloonsAtScene += 1;
				baloonIndex += 1;
				nextBaloonTime = 2f;
			}
		}
	}


	void GuiUpdate()
	{
		timer.text = "Time: "+(int)levelTime;
		TxtScore.text = "Saved: " + score;
	}


	void GameOver()
	{
		HighScore.text = "Score: " + score;
		//dog.SetActive(false);
		gameOverPanel.SetActive(true);
	}


	public void OnMainMenu()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}


	public void CollectItem()
	{
		distance = Vector3.Distance(targetPos, transform.position);
		//Debug.Log (distance);
		dir = new Vector3(targetPos.x, 0, targetPos.z);
		transform.LookAt(dir);
		if(distance > 1f)
		{
			transform.position += transform.forward * speed * Time.deltaTime;
			dogAnim.SetFloat("Walk", 1f);
		}
	}

	
	public void ScoreSystem()
	{
		Debug.Log ("collided");
		score += 1;
		dogAnim.SetFloat("Walk", 0f);
	}
	
	
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.name.Contains("baloonItem"))
		{
			ScoreSystem();
			isCollect = false;
			other.gameObject.SetActive(false);
			dogState = 0;
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


	void OnSetPos(Vector3 pos)
	{
		targetPos = pos;
		isCollect = true;
		dogState = 1;
		Debug.Log (dogState);
	}


	GameObject GetBaloon()
	{
		GameObject retVal;
		int randPos = Random.Range (0, SpawnPosList.Count);
		int randBaloon = Random.Range(0, BallonList.Count);

		retVal = (GameObject)Instantiate(BallonList[randBaloon], 
		                        SpawnPosList[randPos].transform.position, 
		                        Quaternion.identity);

		retVal.transform.parent = BaloonParent;
		retVal.name = "Baloon_" + baloonIndex;

		Baloon baloonScr = retVal.GetComponent<Baloon> () as Baloon;
		int randBaloonItem = Random.Range(0, BallonItemList.Count);

		baloonItemCount++;
		GameObject baloonItem = GameObject.Instantiate(BallonItemList [randBaloonItem]);
		baloonItem.name = "baloonItem_" + baloonItemCount;
		baloonScr.SetBaloonData (baloonItem, randBaloon, true);

		return retVal;
	}
}

