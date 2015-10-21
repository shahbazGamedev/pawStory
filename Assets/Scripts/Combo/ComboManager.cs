/**
Script Author : Vaikash 
Description   : Game Manager - Jump and Combo
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instRef;

    public GameObject initialPlat1;
    public GameObject initialPlat2;
    public GameObject initialPlat3;
    public Transform cameraStartPos;
    public Canvas canvas;
    public float distance;
    float prevTime;

    bool listenForStart;
    public bool gameRunning;
    bool pause;
    bool hasResumed;
    Touch touch;
    
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
        //Input.multiTouchEnabled = false;
    }

    void Start()
    {
        listenForStart = true;
        //TouchManager.PatternRecognized += HandleSwipeDetection;
        EventMgr.GameRestart += OnReset;
        EventMgr.GamePause += OnGamePause;
        EventMgr.GameResume += OnGameResume;
    }

    public void OnDisable()
    {
        //TouchManager.PatternRecognized -= HandleSwipeDetection;
        EventMgr.GameRestart -= OnReset;
        EventMgr.GamePause -= OnGamePause;
        EventMgr.GameResume -= OnGameResume;

    }

    void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            System.GC.Collect();
        }
        if (gameRunning)
        {
            distance += Time.deltaTime;
            if (distance - prevTime > 1f) // Memory Leak Fix
            {
                instructText.text = ((int) distance).ToString();
                prevTime = distance;
            }
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {          
                if (listenForStart)
                {
                    gameRunning = true;
                    listenForStart = false;
                    if (StartGame != null)
                        StartGame();
                }
                else
                    DogRunner.instRef.HandleDogJump();            
        }

#else
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && touch.tapCount == 1 && !IsPointerOverUIObject(canvas, touch.position))
            {
                //  TODO: Not working with restart
                if (listenForStart && !hasResumed)
                {
                    gameRunning = true;
                    listenForStart = false;
                    if (StartGame != null)
                        StartGame();
                }
                else if(!pause && !hasResumed)
                    DogRunner.instRef.HandleDogJump();
                else
                    hasResumed=false;

            }
        }
#endif
    }


    #region EventHandlers

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
        instructText.text ="0";

        SpawnTrigger.beforePrevPlat = 1;
        SpawnTrigger.prevPlat = 1;
        DogRunner.instRef.ResetPos();
        Camera.main.transform.parent.transform.position = cameraStartPos.position;

        initialPlat1.SetActive(true);
        initialPlat2.SetActive(true);
        initialPlat3.SetActive(true);

        OnGameResume();
    }

    // game pause
    void OnGamePause()
    {
        gameRunning = false;
        pause = true;
        Debug.Log("OnPause");
    }

    // game resume
    void OnGameResume()
    {
        Debug.Log("OnResume");
        // need to check if user has started game
        pause = false;
        hasResumed = true;
        if (!listenForStart)
        {
            gameRunning = true;
        }
    }
#endregion EventHandlers


    // Sourced from Internet

    /// <summary>
    /// Cast a ray to test if screenPosition is over any UI object in canvas. This is a replacement
    /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
    /// </summary>
    private bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
    {
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;

        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
