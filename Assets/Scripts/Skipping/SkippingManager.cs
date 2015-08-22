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

    public int dogLives;
    public int dogMaxLives;
    public int comboMultiplierCap;
    public float scoreIncrement;
    public float maxGameTime;
    public float[] ropeSpeeds;

    bool gameStart;
    bool comboChain;
    bool lastHit;

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
    public GameObject touchMat;

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

    // Decouple Event Listeners
    void OnDisable()
    {
        TriggerDetection.DogHitSkipRope -= DogHitSkipRopeEvent;
        TriggerDetection.SkipRopeReset -= SkipRopeResetEvent;
        TouchManager.PatternRecognized -= PatternRecognizedEvent;
        EventMgr.GameRestart -= OnResetBtn;
    }

    // Update is called once per frame
    void Update()
    {
        SliderFunction();
        Timer();
    }

    // Initialize game
    void Init()
    {
        instanceRef = this;
        comboCount = 1;
        dogLives = dogMaxLives;
        scoreText.text = "Score: " + score;
        touchMat.SetActive(true);
        DisplayInstruction();

        // Add Event Listener
        TriggerDetection.DogHitSkipRope += DogHitSkipRopeEvent;
        TriggerDetection.SkipRopeReset += SkipRopeResetEvent;
        TouchManager.PatternRecognized += PatternRecognizedEvent;
        EventMgr.GameRestart += OnResetBtn;
    }

    // Listens for player input
    void PlaySkipping()
    {
        dogAnimator.SetTrigger("Skip");
        tapCount += 1;
    }

    void SliderFunction()
    {
        sliderValue = skipRope.angle * 2 / 360;
        if (sliderValue > 1)
        {
            sliderValue = 2 - sliderValue;
        }
        slider.value = sliderValue;

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
        analogTimer.fillAmount = (maxGameTime - timer) / maxGameTime;
    }

    // Disable life GUI object
    void ReduceLife(int index)
    {
        livesRef[index].SetActive(false);
    }

    // gameOver Function
    void GameOver()
    {
        touchMat.SetActive(false);
        gameStart = false;    
        gameOverPannel.SetActive(true);
        ropeRef.GetComponent<SkippingRope>().rotateRope = false;
        scoreText.gameObject.transform.parent.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        maxCombo = tapCount < 1 ? 0 : maxCombo;
        gameOverText.text = "Score: " + score + "\n\n" + "Max Combo: "
            + maxCombo + "\n\nTotal Taps: " + tapCount; // Game Play Stats
    }

    // Display instruction to start game
    void DisplayInstruction()
    {
        scoreText.text = "Tap to Start!!";
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
                comboText.text = comboCount + "X";
            }
            else
            {
                comboChain = true;
            }
        }
        else
        {
            lastHit = false;
            maxCombo = comboCount >= maxCombo ? comboCount : maxCombo;
            comboCount = 1;
            comboText.text = "";
        }
        scoreText.text = "Score: " + score;
        skipRope.skipSpeed = ropeSpeeds[Random.Range(0, 8)];
    }

    //Event Handler for PatternRecognized Event
    void PatternRecognizedEvent(SwipeRecognizer.TouchPattern pattern)
    {
        //		if(pattern==SwipeRecognizer.TouchPattern.singleTap && gameStart)
        if (pattern != SwipeRecognizer.TouchPattern.hold)
        {
            Debug.Log("Tap");
            if(!gameStart)
            {
                OnPlayBtn();
            }
            else
                PlaySkipping();
        }
    }

    #endregion EventHandlers

    #region BtnCallbacks

    // Play Btn Callback
    public void OnPlayBtn()
    {
        gameStart = true;
        timer = 0;
        ropeRef.GetComponent<SkippingRope>().rotateRope = true;
        scoreText.text = "Score: 0";
    }

    // Reset Btn Callback
    public void OnResetBtn()
    {

        dogLives = dogMaxLives;
        timer = 0;
        gameOverPannel.SetActive(false);
        ropeRef.GetComponent<SkippingRope>().ResetPosition();
        scoreText.gameObject.transform.parent.gameObject.SetActive(true);
        comboText.gameObject.SetActive(true);
        slider.gameObject.SetActive(true);
        comboCount = 1;
        comboChain = false;
        score = 0;
        maxCombo = 0;
        tapCount = 0;
        slider.value = 0;
        foreach (var gameObj in livesRef)
        {
            gameObj.SetActive(true);
        }
        scoreText.text = "";
        touchMat.SetActive(true);
        DisplayInstruction();
    }

    // Home Btn Callback
    public void OnHomeBtn()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }

    #endregion BtnCallbacks
}