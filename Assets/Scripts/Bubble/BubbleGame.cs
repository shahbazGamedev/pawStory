using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Assertions;
/*
    TODO:
    1. Item's vertical distance conflicts with the dog anim.

*/
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
	public float xz_distance;
	Vector3 targetPos;
	int score;
	bool isCollect;
	Vector3 dir;
	Animator dogAnim;
	Vector3 startingPos;
	Rigidbody rb;
	int baloonItemCount = 0;
	float oneSecTimer = 0;

	public static int dogState = 0;// 0= watching, 1= collecting
    public static BubbleGame instance;
    public static Queue<GameObject> ballonItemsToCollectionQueue;
    public static bool isCollecting=false;
    public GameObject currentItemToCollect;
    private float timeElapsed, timeDuration=2f;
    private int randSpawnPos, randBalloonItem, randBalloon;
    private string layerName;

    void OnEnable()
	{
        instance = this;
        EventMgr.SetPos += OnSetPos;
		EventMgr.GameRestart += OnRestartGame;
	}

    void OnDisable()
	{
		EventMgr.SetPos -= OnSetPos;
		EventMgr.GameRestart -= OnRestartGame;
	}
	
	void Start () 
	{
        ballonItemsToCollectionQueue = new Queue<GameObject>();
        timeElapsed = Time.time;
        OnRestartGame ();
	}

	void Update()
	{
        
        //Balloon spawning logic
        if(Time.time - timeElapsed > timeDuration)
        {
            //spawn a balloon
            spawnBalloon();
           // Debug.Log("Spawned a balloon");
            timeElapsed = Time.time;
        }

        //if there are items to collect in Queue and dog is idle
        if (ballonItemsToCollectionQueue.Count > 0  && !isCollecting)
        {
            //if dog is not collecting
            isCollecting = true;
            currentItemToCollect = ballonItemsToCollectionQueue.Dequeue().gameObject;
            Debug.Log("Going to collect item");

            dogState = 1;
        }
        
        if(isCollecting)
        {
            //set the dog direction
            transform.LookAt(new Vector3(currentItemToCollect.transform.position.x, 0, currentItemToCollect.transform.position.z), Vector3.up);

            //make the dog to collect the item
            CollectItem(currentItemToCollect);
        }
    }

    public void OnRestartGame()
	{
        //Debug.Log("OnRestartGame");

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
        isCollecting = false;
		dog.SetActive(true);
		gameOverPanel.SetActive(false);
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


	public void CollectItem(GameObject t_currentItem)
	{
        //distance = Vector3.Distance(t_currentItem.transform.position, transform.position);
        xz_distance = Vector3.Distance(new Vector3(t_currentItem.transform.position.x, 0, t_currentItem.transform.position.z),
            new Vector3(transform.position.x, 0, transform.position.z));

        //Move the dog
        if (xz_distance >= 0.6f)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            dogAnim.SetFloat("Walk", 1f);
        }
        else
        {
            dogAnim.SetFloat("Walk", 0f);
        }

        ////stop dog animation when xz-distance is reached. 
        if (xz_distance <= 2.5f)
        {
            dogAnim.SetFloat("Walk", 0f);
        }

    }

    public void ScoreSystem()
	{
		score += 1;
	}
	
	
	void OnCollisionEnter(Collision other)
	{

        layerName = LayerMask.LayerToName(other.gameObject.layer);

        //if dog collects the balloonItem
        if (layerName == "Toys")
		{
            isCollecting = false;
            Debug.Log("Dog collected the item");
            Destroy(other.gameObject); //Destroy the balloonItem
            ScoreSystem();
        }

    }


	void OnSetPos(Vector3 pos)
	{
		targetPos = pos;
		isCollect = true;
		dogState = 1;
		Debug.Log ("Dog state : "+ dogState);
	}


	//GameObject GetBaloon()
	//{
	//	GameObject t_balloon;
	//	int randPos = Random.Range (0, SpawnPosList.Count);
	//	int randBaloon = Random.Range(0, BallonList.Count);

	//	t_balloon = (GameObject)Instantiate(BallonList[0], 
	//	                        SpawnPosList[randPos].transform.position, 
	//	                        Quaternion.identity);

	//	t_balloon.transform.parent = BaloonParent;
	//	t_balloon.name = "Baloon_" + baloonIndex; //balloon name

	//	Balloon baloonScr = t_balloon.GetComponent<Balloon> () as Balloon;
	//	int randBaloonItem = Random.Range(0, BallonItemList.Count);

	//	baloonItemCount++;
	//	GameObject baloonItem = GameObject.Instantiate(BallonItemList [0]);
 //       Debug.Log(" Balloon item name : " + baloonItem.gameObject.name);
 //       baloonItem.name = "baloonItem_" + baloonItemCount;
	//	baloonScr.setBalloonItem(baloonItem);

 //       return t_balloon;
	//}

    public void spawnBalloon()
    {
        //Generate random values
        randSpawnPos = Random.Range (0, SpawnPosList.Count);
        randBalloon = Random.Range(0, BallonList.Count);
        randBalloonItem = Random.Range(0, BallonItemList.Count);


        //Instantiate the Balloon
        GameObject t_balloon = (GameObject) Instantiate(BallonList[randBalloon], SpawnPosList[randSpawnPos].transform.position, Quaternion.identity);
        Balloon balloon = t_balloon.GetComponent<Balloon>() as Balloon;

        //Instantiate the Balloon Item
        GameObject t_balloonItem = (GameObject) GameObject.Instantiate(BallonItemList[randBalloonItem].gameObject, t_balloon.transform.position, Quaternion.identity);

        t_balloonItem.SetActive(false);
        balloon.setBalloonItem(t_balloonItem);

    }
}