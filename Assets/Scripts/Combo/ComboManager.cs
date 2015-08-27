/**
Script Author : Vaikash 
Description   : Game Manager - Jump and Combo
**/

using UnityEngine;
using System.Collections;

public class ComboManager : MonoBehaviour
{

    // Event Dispatcher
    public delegate void ComboEventBroadcast();
    public static ComboEventBroadcast StartGame;
    public static ComboEventBroadcast Jump;

    public void Awake()
    {

    }

    void Start()
    {
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
    }

    #endregion EventHandlers
}
