/**
Script Author : Vaikash
Description   : Skipping Game Manager
**/

using UnityEngine;
using UnityEngine.UI;

public class SkippingManager : MonoBehaviour
{
    #region Variables

    public static SkippingManager instanceRef;

    public GameObject dogRef;
    public GameObject ropeRef;
    public GameObject[] livesRef;
	public GameObject touchMat;

	public int touchCounter;
    public int dogLives;
    public int dogMaxLives;
    public int comboMultiplierCap;
    public float scoreIncrement;
    public float maxGameTime;
    public float[] ropeSpeeds;

    bool gameStart;
    bool comboChain;
    bool lastHit;
    bool gameHold;

    int comboCount;
    int maxCombo;
    float sliderValue;
    float score; // Always int
    float tapCount;
    float timer;

    SkippingRope skipRope;
    Animator dogAnimator;

    // UI components
    public Slider slider;
    public Text scoreText;
    public Text comboText;
    public Text gameOverText;
    public Image analogTimer;
    public GameObject gameOverPannel;
   

    //slider vars
    float sliderTimeElapsed, sliderTimeDuration=0.01f, sliderOffsetValue=0.01f;

    #endregion Variables

    void Awake()
    {
        dogAnimator = dogRef.GetComponent<Animator>();
        skipRope = ropeRef.GetComponent<SkippingRope>();
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

	//EventMgr
	void OnEnable()
	{
		EventManager.GameRestart += OnResetBtn;
		EventManager.GamePaused += OnGamePause;
		EventManager.GameResumed += OnGameResume;

		/*EventMgr.GameRestart += OnResetBtn;
		EventMgr.GamePause += OnGamePause;
		EventMgr.GameResume += OnGameResume;*/
	}

    // Decouple Event Listeners
    void OnDisable()
    {
        TriggerDetection.DogHitSkipRope -= DogHitSkipRopeEvent;
        TriggerDetection.SkipRopeReset -= SkipRopeResetEvent;
       // TouchManager.PatternRecognized -= PatternRecognizedEvent;
		EventManager.GameRestart -= OnResetBtn;
		EventManager.GamePaused -= OnGamePause;
		EventManager.GameResumed -= OnGameResume;

		/*EventMgr.GameRestart -= OnResetBtn;
		EventMgr.GamePause -= OnGamePause;
		EventMgr.GameResume -= OnGameResume;*/
    }

    // Update is called once per frame
    void Update()
    {
        SliderFunction();

        Timer();

		if (touchCounter > 0) 
		{
			onTap ();
		}
    }

    // Initialize game
    void Init()
    {
		touchCounter = 0;
        instanceRef = this;
        comboCount = 1;
        dogLives = dogMaxLives;
        scoreText.text = "Score: " + score;
		touchMat.SetActive(true);
        DisplayInstruction();

        // Add Event Listener
        TriggerDetection.DogHitSkipRope += DogHitSkipRopeEvent;
        TriggerDetection.SkipRopeReset += SkipRopeResetEvent;
       // TouchManager.PatternRecognized += PatternRecognizedEvent;
       // EventMgr.GameRestart += OnResetBtn;
    }

    // Listens for player input
    void PlaySkipping()
    {
        dogAnimator.SetTrigger("Skip");
        tapCount += 1;
    }

    void SliderFunction()
    {
        // //sliderValue = skipRope.angle;
        // //sliderValue = sliderValue < 0 ? sliderValue + 360 : sliderValue;

        /*
        sliderValue = skipRope.angle * 2 / 360;

        if (sliderValue > 1)
        {
            sliderValue = 2 - sliderValue;
        }
        slider.value = sliderValue;
        */

        if(gameStart && sliderTimeElapsed > sliderTimeDuration)
        {

            if (sliderValue > 1f)
                sliderOffsetValue = -sliderOffsetValue;//reverses the direction
            else
            if (sliderValue < 0f)
                sliderOffsetValue = -sliderOffsetValue;//reverses the direction


            sliderValue -= sliderOffsetValue;


            slider.value = sliderValue;//move the slider

            if(sliderOffsetValue > 0) //If its positive, 
            {
                skipRope.angle = sliderValue * 360f;   //rotate the rope
                //Debug.Log("Additive");
            }
            else
            if(sliderOffsetValue < 0) //If its negative, find the diff and multiply
            {
                skipRope.angle = (1.0f - sliderValue) * 360f;   //rotate the rope
                //Debug.Log("Subtractive");
            }
					
            sliderTimeElapsed = 0f;
        }

        sliderTimeElapsed += Time.deltaTime;
    }

    // Timer function
    void Timer()
    {
        if (gameStart)
        {
            timer += Time.deltaTime;
            if (timer >= maxGameTime)
            {
                Invoke("GameOver", 0.5f);
            }
        }
       // analogTimer.fillAmount = (maxGameTime - timer) / maxGameTime;
    }

    // Disable life GUI object
    void ReduceLife(int index)
    {
       // livesRef[index].SetActive(false);
    }

    // gameOver Function
    void GameOver()
    {
        gameStart = false;    
        gameOverPannel.SetActive(true);
        ropeRef.GetComponent<SkippingRope>().rotateRope = false;
        scoreText.gameObject.transform.parent.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
		touchMat.SetActive(false);
		touchCounter = 0;
        //maxCombo = tapCount < 1 ? 0 : maxCombo;
        // gameover screen full life - 0 maxCombo bug fix
        if (tapCount < 1)
        {
            maxCombo = 0;
        }
        //else if(maxCombo==0)
        //{
            if(comboCount > maxCombo && comboCount != 1)
			{
                maxCombo = comboCount;
            }
        //}
        gameOverText.text = "Score: " + score + "\n\n" + "Max Combo: "
            + maxCombo + "\n\nTotal Taps: " + tapCount; // Game Play Stats
    }

    // Display instruction to start game
    void DisplayInstruction()
    {
        scoreText.text = "Tap to Start!!";
    }

    // Reset rope position and wait for user tap (game pause)
    void onTap()
    {
        gameHold = false;
        gameStart = true;
        ropeRef.GetComponent<SkippingRope>().rotateRope = true;
        scoreText.text = "Score: " + score;
    }

    #region EventHandlers

    // Event Handler for DogHitSkipRope
    void DogHitSkipRopeEvent()
    {
        dogLives -= 1;
        if (dogLives >= 0)
        {
            ReduceLife(dogLives);
        }
        else
            dogLives = 0;
        comboChain = false;
        //comboCount = 1;
        lastHit = true;
        if (dogLives <= 0)
        {
            Invoke("GameOver", 0.5f);
        }
    }

    // Event Handler for SkipRopeReset
    void SkipRopeResetEvent()
    {
        if (!lastHit)
        {
            score += scoreIncrement * comboCount > comboMultiplierCap ? 10 : comboCount;
            if (comboChain)
            {
                comboCount += 1;
                comboText.text = "Combo : " + comboCount + " X ";
            }
            else
            {
                comboChain = true;
            }
            scoreText.text = "Score: " + score;
        }
        else
        {
            lastHit = false;
            maxCombo = comboCount >= maxCombo ? comboCount : maxCombo;
            comboCount = 1;
            comboText.text = "";
            gameHold = true;
            gameStart = false;
            ropeRef.GetComponent<SkippingRope>().rotateRope = false;
            //ropeRef.GetComponent<SkippingRope>().ResetPosition();
            scoreText.text = "--Tap to Resume--";
        }
        //skipRope.skipSpeed = ropeSpeeds[Random.Range(0, 8)];
    }

    //Event Handler for PatternRecognized Event
//    void PatternRecognizedEvent(SwipeRecognizer.TouchPattern pattern)
//    {
//        // if(pattern==SwipeRecognizer.TouchPattern.singleTap && gameStart)
//        if (pattern != SwipeRecognizer.TouchPattern.hold)
//        {
//            //Debug.Log("Tap");
//            if (!gameStart && !gameHold)
//            {
//                OnPlayBtn();
//            }
//            else if (gameHold)
//                onTap();
//            else
//                PlaySkipping();
//        }
//    }

    #endregion EventHandlers

    #region BtnCallbacks

    // Play Btn Callback
//    public void OnPlayBtn()
//    {
//			gameStart = true;
//			timer = 0;
//			ropeRef.GetComponent<SkippingRope> ().rotateRope = true;
//			scoreText.text = "Score: 0";
//    }

    // Reset Btn Callback
    public void OnResetBtn()
    {
		touchCounter = 1;
        dogLives = dogMaxLives;
        timer = 0;
        gameOverPannel.SetActive(false);
        ropeRef.GetComponent<SkippingRope>().ResetPosition();
        scoreText.gameObject.transform.parent.gameObject.SetActive(true);
        comboText.gameObject.SetActive(true);
        slider.gameObject.SetActive(true);
        sliderValue = 0; //reset the slide values to 0 (leftmost)
        slider.value = 0;
        comboCount = 1;
        comboChain = false;
        score = 0;
        maxCombo = 0;
        tapCount = 0;
        foreach (var gameObj in livesRef)
        {
            gameObj.SetActive(true);
        }
        scoreText.text = "";
		touchMat.SetActive(true);
        DisplayInstruction();
        comboText.text = "";

        Debug.Log("Resetted the slider values");
    }

    // Home Btn Callback
    public void OnHomeBtn()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }

    public void OnPointerDown()
	{
		touchCounter += 1;
		if (touchCounter > 1) 
		{
			PlaySkipping ();
		}
	}
		
    #endregion BtnCallbacks


	//Events
	public void OnGamePause()
	{
		touchMat.SetActive (false);	
	}

	public void OnGameResume()
	{
		touchMat.SetActive (true);	
	}
}




