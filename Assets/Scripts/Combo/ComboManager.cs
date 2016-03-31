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
	public GameObject txtTutorial;
   // public Transform cameraStartPos;
    public Canvas canvas;
    public static float distance;
    public static bool LifeCalc;
    float prevTime;

    public bool listenForStart;
    public static bool gameRunning=false;
    public bool Achive1;
    public bool Achive2;
    public bool Achive3;
    bool pause;
    bool hasResumed;
    Touch touch;
    
    //UI Components
    public Text instructText;
    public Text gameOverText;
    public GameObject gameOverPanel;
    public Text ScoreDisp;
    public Text ComboDisp;
    
	public Text screenShareDistanceText;

    // Event Dispatcher
    public delegate void ComboEventBroadcast();
    public static ComboEventBroadcast StartGame;
    public static ComboEventBroadcast Jump;

    public void Awake()
    {
        instRef = this;
        //Input.multiTouchEnabled = false;
        listenForStart = true;
        gameRunning = false;
    }

    void Start()
    {
        
        listenForStart = true;

        //TouchManager.PatternRecognized += HandleSwipeDetection;
		EventManager.GamePaused += OnGamePause;
		EventManager.GameRestart += OnReset;
		EventManager.GameResumed += OnGameResume;

		/* EventMgr.GameRestart += OnReset;
        EventMgr.GamePause += OnGamePause;
        EventMgr.GameResume += OnGameResume;*/
		
    }

    public void OnDisable()
    {
        //TouchManager.PatternRecognized -= HandleSwipeDetection;
		EventManager.GamePaused -= OnGamePause;
		EventManager.GameRestart -= OnReset;
		EventManager.GameResumed -= OnGameResume;

		/*EventMgr.GameRestart -= OnReset;
        EventMgr.GamePause -= OnGamePause;
        EventMgr.GameResume -= OnGameResume;*/

    }

    void Update()
    {
         
        LifeCalc = true;
        
        if (gameRunning)
        {

			txtTutorial.SetActive (false);
			//GooglePlayServiceManager.instance.UnlockAchievement("InitialRun");
			GlobalVariables.distanceCovered += Time.deltaTime;
			DogRunner.gameOver = false;
			DogRunner.instRef.dogAnim.SetBool("GameOver",DogRunner.gameOver);
			//if (GlobalVariables.distanceCovered - prevTime > 1f) // Memory Leak Fix
           // {
				instructText.text = "Distance : "+((int) GlobalVariables.distanceCovered).ToString();
				prevTime = GlobalVariables.distanceCovered;
                
          //  }
        }
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject())
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
        
            if(touch.phase == TouchPhase.Ended && touch.tapCount == 1)
            {
                if (listenForStart && !hasResumed)
                {
                    gameRunning = true;
                    listenForStart = false; 
                    if (StartGame != null)
                     StartGame();
                }
                else
                {
                    hasResumed=false;
                }
            }

           //line removed : && !IsPointerOverUIObject(canvas, touch.position)
            if (touch.phase == TouchPhase.Began && touch.tapCount == 1)
            {
                //  TODO: Not working with restart
                 if(!pause && !hasResumed)
                    DogRunner.instRef.HandleDogJump();
                else
                    hasResumed=false;
            }
        }
#endif
	
//update*/
	}

    #region EventHandlers

    public void GameOver()
    {
        gameRunning = false;
		//SoundManager.instance.PlaySfx(SFXVAL.gameOver);
        gameOverPanel.SetActive(true);
		gameOverText.text = "Distance Covered: " + (int)GlobalVariables.distanceCovered +" m\n\n"+"Score: " + ScoreSystem.score+" Pts.";
		//screenShareDistanceText.text =  "Distance Covered: " + (int)GlobalVariables.distanceCovered +"m";


    }

   
    // Game Reset
    public void OnReset()
    {
		txtTutorial.SetActive (true);
        listenForStart = true;
        DogRunner.instRef.GameOver();
        gameOverPanel.SetActive(false);
        gameRunning = false;
      //  Pooler.InstRef.HideAll();        
		DogRunner.gameOver = false;
        DogRunner.instRef.ResetPos();
		GlobalVariables.distanceCovered = 0;
        instructText.text ="";
        ScoreSystem.score = 0;
        ScoreSystem.comboCount = 0;
		ScoreDisp.text = "Score: " + ScoreSystem.score + " Points.";
        ComboDisp.text = "";

        Prime31_PlatformGenerator.prime31_PlatformGen.cleanOldPlatforms();

        Prime31_PlatformGenerator.prime31_PlatformGen.createPlatformsAtStart();//Generate the platforms on start
        OnGameResume();
        

       
        
    }

	public void ResetValueAfterContinueAD()
	{
		if (DogRunner.Life == 5 || GlobalVariables.distanceCovered<150f)
		{
            listenForStart = true;
            DogRunner.Life = 0;
             
			DogRunner.instRef.GameOver();
			gameOverPanel.SetActive(false);
			gameRunning = false;
			listenForStart = true;
			 
			DogRunner.instRef.ResetPos();
            
            Prime31_PlatformGenerator.prime31_PlatformGen.createPlatformsAtStart();//Generate the platforms on start

            
			OnGameResume();
			

		}
	}

    public void OnResetBonus()
    {
        DogRunner.Life = 0;
		Debug.Log("Showing Video AD");
        // Application.LoadLevel(Application.loadedLevel);

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
