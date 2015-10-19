/**
Script Author : Vaikash 
Description   : Game Manager - Jump and Combo
**/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instRef;

    public GameObject initialPlat1;
    public GameObject initialPlat2;
    public GameObject initialPlat3;
    public Transform cameraStartPos;
    public float distance;

    bool listenForStart;
    bool gameRunning;
    
    //UI Components
    public Text instructText;
    public Text gameOverText;
    public GameObject gameOverPanel;


    // Event Dispatcher
    public delegate void ComboEventBroadcast();
    public static ComboEventBroadcast StartGame;
    public static ComboEventBroadcast Jump;

    public void Awake()
    {
        instRef = this;
    }

    void Start()
    {
        listenForStart = true;
        TouchManager.PatternRecognized += HandleSwipeDetection;
        EventMgr.GameRestart += OnReset;
        EventMgr.GamePause += OnGamePause;
        EventMgr.GameResume += OnGameResume;
    }

    public void OnDisable()
    {
        TouchManager.PatternRecognized -= HandleSwipeDetection;
        EventMgr.GameRestart -= OnReset;
        EventMgr.GamePause -= OnGamePause;
        EventMgr.GameResume -= OnGameResume;

    }

    void Update()
    {
        if (gameRunning)
        {
            distance += Time.deltaTime;
            instructText.text = "Distance Covered: " + (int) distance + " m";
        }
    }

    #region EventHandlers

    // Handle Swipe Detected Event
    void HandleSwipeDetection(SwipeRecognizer.TouchPattern pattern)
    {
       
        if (listenForStart)
        {
            if (pattern == SwipeRecognizer.TouchPattern.singleTap)
            {
                gameRunning = true;
                listenForStart = false;
                if (StartGame != null)
                    StartGame();
            }
        }
        else if (pattern == SwipeRecognizer.TouchPattern.singleTap)
        {
            if (Jump != null)
            {
                Jump();
            }
        }
    }

    public void GameOver()
    {
        gameRunning = false;
        gameOverPanel.SetActive(true);
        gameOverText.text = "Distance Covered: " + (int)distance +" m";
    }

    // Game Reset
    void OnReset()
    {
        DogRunner.instRef.GameOver();
        gameOverPanel.SetActive(false);
        gameRunning = false;
        listenForStart = true;
        Pooler.InstRef.HideAll();        

        distance = 0;
        instructText.text = "Distance Covered: " + (int) distance + " m";

        SpawnTrigger.beforePrevPlat = 1;
        SpawnTrigger.prevPlat = 1;
        DogRunner.instRef.ResetPos();
        Camera.main.transform.parent.transform.position = cameraStartPos.position;

        initialPlat1.SetActive(true);
        initialPlat2.SetActive(true);
        initialPlat3.SetActive(true);
    }

    // game pause
    void OnGamePause()
    {
        gameRunning = false;
    }

    // game resume
    void OnGameResume()
    {
        // need to check if user has started game
        gameRunning = true;
    }
    #endregion EventHandlers
 
}
