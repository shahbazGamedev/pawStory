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
    
    bool listenForStart;
    bool gameRunning;
    float distance;

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
        if (pattern == SwipeRecognizer.TouchPattern.swipeUp)
        {
            if (Jump != null)
            {
                Jump();
            }
        }
        else if (listenForStart)
        {
            if (pattern == SwipeRecognizer.TouchPattern.singleTap)
            {
                gameRunning = true;
                if (StartGame != null)
                    StartGame();
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
        gameRunning = false;
        Pooler.InstRef.HideAll();
        
        distance = 0;
        instructText.text = "Distance Covered: " + (int) distance + " m";
        gameOverPanel.SetActive(false);

        SpawnTrigger.beforePrevPlat = 1;
        SpawnTrigger.prevPlat = 1;
        DogRunner.instRef.ResetPos();

        //if(!initialPlat1.activeInHierarchy)
        initialPlat1.SetActive(true);
        //if (!initialPlat2.activeInHierarchy)
        initialPlat2.SetActive(true);
    }

    // game pause
    void OnGamePause()
    {
        gameRunning = false;
    }

    // game resume
    void OnGameResume()
    {
        gameRunning = true;
    }
    #endregion EventHandlers
 
}
