/**
Script Author : Vaikash 
Description   : Game Manager - Jump and Combo
**/

using UnityEngine;
using System.Collections;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instRef;
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
    }

    public void OnDisable()
    {
        TouchManager.PatternRecognized -= HandleSwipeDetection;
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

    #endregion EventHandlers
 
}
