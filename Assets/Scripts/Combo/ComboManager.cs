/**
Script Author : Vaikash 
Description   : Game Manager - Jump and Combo
**/

using UnityEngine;
using System.Collections;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instRef;

    public GameObject initialPlat1;
    public GameObject initialPlat2;
    
    bool listenForStart;

    //UI Components
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
    }

    public void OnDisable()
    {
        TouchManager.PatternRecognized -= HandleSwipeDetection;
        EventMgr.GameRestart -= OnReset;
    }

    void Update()
    {

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
                if (StartGame != null)
                    StartGame();
            }
        }        
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // Game Reset
    void OnReset()
    {
        initialPlat1.SetActive(true);
        initialPlat2.SetActive(true);
        Pooler.InstRef.HideAll();
        DogRunner.instRef.ResetPos();
        gameOverPanel.SetActive(false);

        SpawnTrigger.beforePrevPlat = 1;
        SpawnTrigger.prevPlat = 1;
    }

    #endregion EventHandlers
 
}
