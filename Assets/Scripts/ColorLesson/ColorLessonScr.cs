using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorLessonScr : MonoBehaviour
{
	public GameObject DogObj;
	public Transform StartPosOfDog;
	public Transform StartPosOfCam;
	public List<GameObject> BlockList;
	public List<GameObject> PickupList;
	public GameObject PickupsParentObj;
	public Text QuestionText;
	public Text ScoreText;
	public Text GameOverText;
	public GameObject GameOverScreenObj;
	public GameObject GameHudObj;
	public Image QuestionImg;
	public List<Sprite> QuestionList;
	public Animator DogAnimator;
	public ParticleSystem Ps;
    public GameObject Image_Life_01;
    public GameObject Image_Life_02;
    public GameObject Image_Life_03;


	int dogAnimState = 0; // 0: Idle, 1: Idle 2, 2: Idle 3, 3: Run 
	int dogSpeed = 10;
	float pickupStartPos = 10.0f;
	int curLane = 1; // starts with zero
	int TotalLanes = 3;
	float laneWidth = 2.0f;
	List<Vector3> lanePostion;
	int TotalQuestions = 10;
	List<int> Questions;
	int curQuestion = 0;
	int ScoreVal = 0;
	float pickupGap = 5.0f;
	bool isGameOver = false;
	Vector3 dogMoveOffset = Vector3.zero;
    int wrongAnswer = 0;
    int totalWrongAnswers = 3;

	// Swipe
	private Vector3 firstPos;
	private Vector3 lastPos;
	private float dragDistance;  //minimum distance for a swipe to be registered
	private List<Vector3> touchPositions = new List<Vector3>();
	
	private bool waitForTap = true;

	void OnEnable() {
		EventManager.GameRestart += OnRestartGame;
		//EventMgr.GameRestart += OnRestartGame;
	}
	
	
	void OnDisable() {
		EventManager.GameRestart -= OnRestartGame;
		//EventMgr.GameRestart -= OnRestartGame;
	}


	void Start () {
		OnRestartGame ();
	}


	void Update ()	{
		if (waitForTap) {
#if UNITY_ANDROID || UNITY_IOS 
			if(Input.touchCount > 0) {
				Touch touch = Input.touches[0];
				if (touch.phase == TouchPhase.Ended) {
					waitForTap = false;
					dogAnimState = 3;
					DogAnimator.SetInteger("DogAnimState", dogAnimState);
					QuestionImg.gameObject.SetActive (true);
					ScoreText.gameObject.SetActive(true);
					QuestionText.text = "Item to collect ";
				}
			}
#endif
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp (0))
			{
				waitForTap = false;
				dogAnimState = 3;
				DogAnimator.SetInteger("DogAnimState", dogAnimState);
				QuestionImg.gameObject.SetActive (true);
				ScoreText.gameObject.SetActive(true);
				QuestionText.text = "Item to collect ";
			}
		}
		else if (!isGameOver)
		{
			// restrict to two touches
			for(int i = 0; i < 2 && i < Input.touchCount; i++)
			{ 
				Touch touch = Input.touches[i];
			    if (touch.phase == TouchPhase.Moved)
				{
					touchPositions.Add(touch.position);
				}
				if (touch.phase == TouchPhase.Ended)
				{
			        firstPos =  touchPositions[0]; 
			        lastPos =  touchPositions[touchPositions.Count-1]; 

					if (Mathf.Abs(lastPos.x - firstPos.x) > dragDistance || Mathf.Abs(lastPos.y - firstPos.y) > dragDistance)
			        {
						if (Mathf.Abs(lastPos.y - firstPos.y) > Mathf.Abs(lastPos.x - firstPos.x))
			            {   //the vertical movement 
							if (lastPos.y>firstPos.y)
							{
								Debug.Log("Up Swipe");
								OnLeft();
							}
							else
							{
								Debug.Log("Down Swipe");
								OnRight();
							}
						}
						else
						{
							if ((lastPos.x>firstPos.x))
							{
								Debug.Log("Right Swipe");
							}
							else
							{
								Debug.Log("Left Swipe");
							}
						}
					}
					touchPositions.Clear();
				}
			}

			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				OnLeft();
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				OnRight();
			}

			// move the dog
			dogMoveOffset = new Vector3 (0, 0, Time.deltaTime * dogSpeed);
			DogObj.transform.position += dogMoveOffset;
		}
	}


	private void LateUpdate()
	{
		if (!isGameOver && !waitForTap) {
			Camera.main.transform.position += dogMoveOffset;
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.name.Contains ("Trigger"))
		{
			// dynamic level load trigger
		}
		else
		{
            if (other.name.Contains("Ans"))
            {
                Ps.gameObject.transform.position = other.gameObject.transform.position;
                ScoreVal++;
                Ps.Play();
            }
            else
            {
                wrongAnswer++;
                if (wrongAnswer >= totalWrongAnswers)
                    OnGameOver();
                else
                    UpdateLifeUI();
            }
			ScoreText.text = ScoreVal + " / "  + TotalQuestions;
			curQuestion++;
			if (curQuestion >= TotalQuestions)
			{
                OnGameOver();
			}
			else
			{
				QuestionImg.sprite = QuestionList[Questions[curQuestion]];
				//QuestionText.text = "Item to collect ";// + Questions[curQuestion] + QuestionList[Questions[curQuestion]].name;
				Debug.Log("curQuestion :  " + curQuestion);
			}
			other.gameObject.SetActive(false);
		}
	}


	void LoadPickups()
	{
		int i = 0;
		Questions = new List<int> ();

		pickupStartPos = StartPosOfDog.transform.position.z + pickupStartPos;

		while (i < TotalQuestions)
		{
			int newQuestion = Random.Range(0, PickupList.Count);
			Questions.Add(newQuestion);
			int prevQuestion = newQuestion - 1;
			int nextQuestion = newQuestion + 1;
			if(prevQuestion < 0)
				prevQuestion = PickupList.Count - 1;
			if(nextQuestion >= PickupList.Count)
				nextQuestion = 0;

			GameObject parentObj = new GameObject();
			parentObj.name = "Question_" + i;
			parentObj.transform.parent = PickupsParentObj.transform;

			int nextLane = Random.Range(0, TotalLanes);
			GameObject curObj = GameObject.Instantiate(PickupList[newQuestion]) as GameObject;
			curObj.transform.position = new Vector3(lanePostion[nextLane].x,
			                                        lanePostion[nextLane].y + 0.5f,
			                                        pickupStartPos + i * pickupGap);
			curObj.transform.localScale = new Vector3(1f, 1f, 1f);
			curObj.transform.parent = parentObj.transform; 
			curObj.name = PickupList[newQuestion].name + "_" + i + "_1" + "Ans";
			nextLane++;
			if(nextLane >= TotalLanes)
				nextLane = 0;

			curObj = GameObject.Instantiate(PickupList[prevQuestion]) as GameObject;
			curObj.transform.position = new Vector3(lanePostion[nextLane].x,
			                                        lanePostion[nextLane].y + 0.5f,
			                                        pickupStartPos + i * pickupGap);
			curObj.transform.parent = parentObj.transform; 
			curObj.name = PickupList[prevQuestion].name + "_" + i + "_2";
			nextLane++;
			if(nextLane >= TotalLanes)
				nextLane = 0;

			curObj = GameObject.Instantiate(PickupList[nextQuestion]) as GameObject;
			curObj.transform.position = new Vector3(lanePostion[nextLane].x,
			                                        lanePostion[nextLane].y + 0.5f,
			                                        pickupStartPos + i * pickupGap);
			curObj.transform.parent = parentObj.transform; 
			curObj.name = PickupList[nextQuestion].name + "_" + i + "_3";

			i++;
		}
	}


	public void OnLeft()
	{
		if (curLane > 0)
		{
			curLane--;
			DogObj.transform.position =
				new Vector3(lanePostion[curLane].x,
				            lanePostion[curLane].y,
				            DogObj.transform.position.z);
		}
	}


	public void OnRight()
	{
		if (curLane < TotalLanes-1)
		{
			curLane++;
			DogObj.transform.position =
				new Vector3(lanePostion[curLane].x,
				            lanePostion[curLane].y,
				            DogObj.transform.position.z);
		}
	}


	public void OnRestartGame()
	{
		dogSpeed = 8;
		curLane = 1;
		TotalLanes = 3;
		laneWidth = 2.0f;
		TotalQuestions = 10;
		curQuestion = 0;
		ScoreVal = 0;
		pickupGap = 20.0f;
		pickupStartPos = pickupGap;
		dogMoveOffset = Vector3.zero;
		isGameOver = false;
		waitForTap = true;

		dogAnimState = 0;
		DogAnimator.SetInteger("DogAnimState", dogAnimState);

		foreach (Transform child in PickupsParentObj.transform)
			GameObject.Destroy(child.gameObject);

		lanePostion = new List<Vector3>();
		lanePostion.Add (new Vector3(-2, 0f, 0));
		lanePostion.Add (new Vector3(0.5f, 0f, 0));
		lanePostion.Add (new Vector3(2.75f, 0f, 0));

		LoadPickups();

		GameOverScreenObj.SetActive(false);
		//GameHudObj.SetActive(true);

		Camera.main.transform.position = StartPosOfCam.position;

		QuestionImg.gameObject.SetActive (false);
		ScoreText.gameObject.SetActive(false);
		QuestionImg.sprite = QuestionList[Questions[curQuestion]];
		ScoreText.text = ScoreVal + " / "  + TotalQuestions;

		DogObj.transform.position = StartPosOfDog.position;

		dragDistance = Screen.height * 0.1f;
		QuestionText.text = "Tap to start";

        wrongAnswer = 0;
        totalWrongAnswers = 3;
        UpdateLifeUI();
	}


	public void OnHome(){
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}


	void IdleStartFired(){
	}


    void UpdateLifeUI()
    {
        Image_Life_01.SetActive(true);
        Image_Life_02.SetActive(true);
        Image_Life_03.SetActive(true);

        switch (wrongAnswer)
        {
            case 1:
                Image_Life_03.SetActive(false);
                break;
            case 2:
                Image_Life_02.SetActive(false);
                Image_Life_03.SetActive(false);
                break;
            case 3:
                Image_Life_01.SetActive(false);
                Image_Life_02.SetActive(false);
                Image_Life_03.SetActive(false);
                break;
        }
    }


    void OnGameOver()
    {
        isGameOver = true;
		dogAnimState = 2;
        GameOverScreenObj.SetActive(true);
        //GameHudObj.SetActive(false);
        GameOverText.text = "Collected " + ScoreVal + " out of " + TotalQuestions;
    }


}


