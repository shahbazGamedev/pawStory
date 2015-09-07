/**
Script Author : Vaikash
Description   : Game Manager for Obedience
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObedienceManager : MonoBehaviour
{
    #region Var
    public Text instructions;
    public Text roundInfo; // (changed) displays score
    public Text gameOverText;
    public Text messageText;
    public Image analogTimer;
    public GameObject gameOverPanel;
    public GameObject gestureMat; // Image UI element on which event triggers are attached
    public GameObject[] directionRef;
    public float instructionWaitTime;
    public bool registerAnimEvent; // If true listens for trigger from idle animation
    public bool nextInstruct;
    public int maxChances;
    public int scoreIncrement;
    public float angularVelocity;
    public float jumpForce;
    public int range;

    float timer;
    bool gameOn;
    bool catchUserInput; // Bool for catchUserInput coroutine
    bool combo;
    bool isCoroutineON; // True when catchCombos coroutine is running
    bool isDogSiting;
    bool waitForReset;
    bool instructON;
    bool interrupt;
    int chance;
    int points;
    int randomNumber;

    Animator dogAnim;

    SwipeRecognizer.TouchPattern pattern;
    SwipeRecognizer.TouchPattern presentGesture;
    SwipeRecognizer.TouchPattern gestureCache;

    GameObject dogRef;
    DogManager dogManager;
    Vector3 startPosition;
    Quaternion startRotation;

    [System.Serializable]
    public struct GestureCollection
    {
        public string key;
        public SwipeRecognizer.TouchPattern value1;
        public SwipeRecognizer.TouchPattern value2;
    }

    // Emulates hash-table in editor
    public GestureCollection[] gestureCollection;

    [System.Serializable]
    public struct SwipeDataCollection
    {
        public string name;
        public int touchID;
        public bool isActive;
        public float holdTime;
        public Vector2 startPoint;
        public Vector2 endPoint;
        public List<Vector2> swipeData;
    }

    // Emulates hash-table in editor
    public SwipeDataCollection[] swipeDataCollection;

    #endregion Var

    // Use this for initialization
    void Start()
    {
        Init();
        startPosition = dogRef.transform.position;
        startRotation = dogRef.transform.rotation;
        OnRestart();
    }

    // Update is called once per frame
    void Update()
    {
        if (catchUserInput)
        {
            // Code executes when swipe is identified
            if (pattern != SwipeRecognizer.TouchPattern.reset)
            {
                CheckUserInput();
            }
        }

        // Touch Timer for hold down gesture
        if (swipeDataCollection[1].isActive)
            swipeDataCollection[1].holdTime += Time.deltaTime;
        if (swipeDataCollection[2].isActive)
            swipeDataCollection[2].holdTime += Time.deltaTime;

        SyncAnimation();
        Timer();
    }

    // Checks non combo conditions
    void CheckUserInput()
    {
        // Separate routines for combos and non combos!
        if (randomNumber != 15) // Include Nos which has combos
        {
            Debug.Log(pattern);
            if (pattern == presentGesture)
            {
                Invoke("NotifyCorrectInput", 1);
                catchUserInput = false;
                points += scoreIncrement;
                gestureCache = pattern;
                DeactivateGestureMat();
                roundInfo.text = "Score: " + points * scoreIncrement;
            }
            else // if wrong gesture
            {
                if (true)
                {
                    if (pattern == SwipeRecognizer.TouchPattern.singleTap || pattern == SwipeRecognizer.TouchPattern.tryAgain /*|| pattern == SwipeRecognizer.TouchPattern.swipeLeft || pattern == SwipeRecognizer.TouchPattern.swipeRight*/) // Uncomment if need arises
                    {
                        SwipeReset();
                        Invoke("NotifyTryAgain", 0.2f);
                        return;
                    }
                }
                gestureCache = pattern;
                catchUserInput = false;
                Invoke("NotifyWrongInput", 1);
                DeactivateGestureMat();
            }
        }
        else if (!isCoroutineON) // Prevent stacking of coroutine
        {
            isCoroutineON = true;
            StartCoroutine(CatchCombos());
        }
    }

    // Sync animation of dog based on player input
    void SyncAnimation()
    {
        switch (gestureCache)
        {
            case SwipeRecognizer.TouchPattern.reset:
                {
                    break;
                }
            case SwipeRecognizer.TouchPattern.swipeUp:
                {
                    if (isDogSiting)
                    {
                        nextInstruct = false;
                        registerAnimEvent = true;
                        dogAnim.SetTrigger("Stand");
                        gestureCache = SwipeRecognizer.TouchPattern.reset;
                        isDogSiting = false;
                    }
                    else
                    {
                        nextInstruct = false;
                        registerAnimEvent = true;
                        gestureCache = SwipeRecognizer.TouchPattern.reset;
                    }
                    break;
                }
            case SwipeRecognizer.TouchPattern.swipeDown:
                {
                    nextInstruct = true;
                    //registerAnimEvent = true;
                    dogAnim.SetTrigger("Sit");
                    isDogSiting = true;
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.swipeUpLeft:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    //dogAnim.SetTrigger ("Jump");
                    StartCoroutine(DogJump(gestureCache));
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.swipeUpRight:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    //dogAnim.SetTrigger ("Jump");
                    StartCoroutine(DogJump(gestureCache));
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.clockwiseCircle:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    dogAnim.SetTrigger("ClockWise");
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.antiClockwiseCircle:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    dogAnim.SetTrigger("AntiClockWise");
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.doubleCircle:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    dogAnim.SetTrigger("TailChase");
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.doubleTap:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    dogAnim.SetBool("StandOn", true);
                    StartCoroutine(ResetAnimBool("StandOn", 4));
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.hold:
                {
                    if (randomNumber == 5)
                    {
                        nextInstruct = false;
                        registerAnimEvent = true;
                        dogAnim.SetTrigger("Jump");
                        dogRef.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, 0f) * jumpForce, ForceMode.Impulse);
                        gestureCache = SwipeRecognizer.TouchPattern.reset;
                    }
                    else
                    {
                        nextInstruct = false;
                        registerAnimEvent = true;
                        dogAnim.SetTrigger("Jump");
                        gestureCache = SwipeRecognizer.TouchPattern.reset;
                    }
                    break;
                }
            case SwipeRecognizer.TouchPattern.swipeDownLeft:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    //dogAnim.SetBool("Mirror", false);
                    //dogAnim.SetBool("MirStand", true);
                    //dogAnim.SetTrigger("Sleep");
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
            case SwipeRecognizer.TouchPattern.swipeDownRight:
                {
                    nextInstruct = false;
                    registerAnimEvent = true;
                    //dogAnim.SetBool("Mirror", true);
                    //dogAnim.SetBool("MirStand", false);
                    //dogAnim.SetTrigger("Sleep");
                    gestureCache = SwipeRecognizer.TouchPattern.reset;
                    break;
                }
        }
    }

    // Notifies wrong input
    void NotifyWrongInput()
    {
        messageText.text = "Wrong";
        Invoke("ClearMessage", 2f);
    }

    // Notifies try again
    void NotifyTryAgain()
    {
        messageText.text = "Try Again";
        Invoke("ClearMessage", 2f);
    }

    // Notifies correct input
    void NotifyCorrectInput()
    {
        messageText.text = "Excellent!";
        Invoke("ClearMessage", 2f);
    }

    // De-notifies message
    void ClearMessage()
    {
        messageText.text = "";
    }

    // Disable touchInput
    public void DeactivateGestureMat()
    {
        SwipeReset();
        catchUserInput = false;
        gestureMat.SetActive(false);
    }

    // Reset previous touch input
    void SwipeReset()
    {
        pattern = SwipeRecognizer.TouchPattern.reset;
    }

    // Reset particular swipe data
    void dataReset(int pointerID)
    {
        swipeDataCollection[pointerID].swipeData.Clear();
    }

    // Timer function
    void Timer()
    {
        if (gameOn)
        {
            timer += Time.deltaTime;
            if (timer > instructionWaitTime)
            {
                timer = 5;
                //nextInstruct = true;
            }
        }
        else
        {
            analogTimer.transform.parent.gameObject.SetActive(false);
        }
        //		timerNotification.text = (int)timer + " / " + (int)instructionWaitTime;
        analogTimer.fillAmount = (5 - timer) / 5;
    }

    // Flag for issuing next instruction
    void GotoNextInstruction()
    {
        if (waitForReset)
        {
            nextInstruct = true;
            Debug.Log("Fired");
        }
    }

    // Decouple listener from event
    void OnDisable()
    {
        dogManager.ResetComplete -= GotoNextInstruction;
        EventMgr.GameRestart -= OnRestart;
    }

    // Initialize at start of game
    void Init()
    {
        dogRef = GameObject.FindGameObjectWithTag("Player");
        dogManager = dogRef.GetComponent<DogManager>();
        dogAnim = dogRef.GetComponentInChildren<Animator>();

        // Add listener to reset complete event
        dogManager.ResetComplete += GotoNextInstruction;
        EventMgr.GameRestart += OnRestart;
        analogTimer.transform.parent.gameObject.SetActive(true);
    }

    #region Coroutines

    //  Checks combo conditions
    IEnumerator CatchCombos()
    {
        while (catchUserInput)
        {
            yield return new WaitForEndOfFrame(); // Wait for fixed update
            if (pattern != SwipeRecognizer.TouchPattern.reset)
            {
                Debug.Log(pattern);
                if (combo)
                {
                    if (pattern == gestureCollection[randomNumber].value2)
                    {
                        instructions.text = "2x Combo Success";
                        points += scoreIncrement;
                        catchUserInput = false;
                        combo = false;
                        DeactivateGestureMat();
                        roundInfo.text = "Score: " + points * scoreIncrement;
                    }
                    else // if wrong gesture
                    {
                        instructions.text = "Wrong";
                        DeactivateGestureMat();
                    }
                }
                else
                {
                    if (pattern == presentGesture && !combo)
                    {
                        instructions.text = "1x Success";
                        combo = true;
                        SwipeReset();
                        StartCoroutine(DetectHold());
                    }
                    else // if wrong gesture
                    {
                        instructions.text = "Wrong";
                        DeactivateGestureMat();
                    }
                }
            }
        }
        isCoroutineON = false;
        yield return null;
    }

    // Sets random instruction at fixed intervals
    IEnumerator Instruct()
    {
        instructON = true;
        while (gameOn)
        {
            //Debug.Log("A");
            if (chance >= maxChances) // if chances depleted gameOver
            {
                yield return new WaitForSeconds(2);
                DeactivateGestureMat();
                gameOn = false;
                gameOverPanel.SetActive(true);
                gameOverText.text = "Score: " + points * scoreIncrement;
            }
            else  // else put a random instruction
            {
                if (!registerAnimEvent)
                {
                    nextInstruct = true;
                }

                // Wait till animation is complete
                yield return StartCoroutine(WailTillIdleAnimation());

                chance += 1;
                if (randomNumber == 0 && isDogSiting || isDogSiting)
                {
                    randomNumber = 1;
                    
                }
                else
                {
                    int prevRandomvalue = randomNumber;
                    // Generate random number that is not same as previous or 1 (prevent instruction stand before sit)
                    while (randomNumber == prevRandomvalue || randomNumber == 1)
                    {
                        randomNumber = Random.Range(0, range);
                    }
                }

                //randomNumber = 8;
                presentGesture = gestureCollection[randomNumber].value1;
                instructions.text = gestureCollection[randomNumber].key;

                SwipeReset(); // reset previous swipe data
                catchUserInput = true;
                timer = 0;

                gestureMat.SetActive(true); // Turn on user input if off

                // reset hold time
                swipeDataCollection[1].holdTime = 0;
                swipeDataCollection[2].holdTime = 0;

                //				if (randomNumber == 5 || randomNumber == 11) // Change condition to check hold gesture if need arises
                //				{
                StartCoroutine(DetectHold());
                //				}
                nextInstruct = false;

                yield return StartCoroutine(InterruptableWait(instructionWaitTime));
                //yield return new WaitForSeconds(instructionWaitTime);
                //yield return null;
            }
        }
        instructON = false;
        yield return null;
    }

    // Wait till timer that can be interrupted
    IEnumerator InterruptableWait(float time)
    {
        var t=0f;
        while(t<time)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
            if(interrupt)
            {
                interrupt = false;
                break;
            }
        }
        yield return null;
    }

    // Checks for touch pattern hold and move - only when combo is true
    IEnumerator DetectHold()
    {
        while (catchUserInput)
        {
            yield return new WaitForFixedUpdate();
            if (swipeDataCollection[1].isActive && !swipeDataCollection[2].isActive)
            {
                // Min. hold time of 1 sec
                if (swipeDataCollection[1].holdTime > 1 && swipeDataCollection[1].swipeData.Count <= 3)
                {
                    pattern = SwipeRecognizer.TouchPattern.hold;
                    swipeDataCollection[1].holdTime = 0;
                    swipeDataCollection[1].isActive = false;
                }
                // move gesture disabled
                //				else if(swipeDataCollection [1].swipeData.Count>3)
                //				{
                //					pattern = SwipeRecognizer.TouchPattern.move;
                //					swipeDataCollection [1].isActive = false;
                //				}
            }
        }
        yield return null;
    }

    // Resets animation state after wait time
    IEnumerator ResetAnimBool(string animName, int wait)
    {
        yield return new WaitForSeconds(wait);
        dogAnim.SetBool(animName, false);
    }

    // Makes the dog jump in the desired direction
    IEnumerator DogJump(SwipeRecognizer.TouchPattern touchPattern)
    {
        waitForReset = true;
        if (touchPattern == SwipeRecognizer.TouchPattern.swipeUpLeft) // Left Jump
        {
            Quaternion targetRotation = directionRef[0].transform.rotation;
            while (dogRef.transform.rotation != targetRotation)
            {
                yield return new WaitForFixedUpdate();
                Quaternion deltaRotation = Quaternion.RotateTowards(dogRef.transform.rotation, targetRotation, angularVelocity * Time.deltaTime);
                dogRef.transform.rotation = deltaRotation;
            }
            dogAnim.SetTrigger("Jump");
            dogRef.GetComponent<Rigidbody>().AddForce(targetRotation * (new Vector3(0f, 1f, 1f)) * jumpForce, ForceMode.Impulse);
        }
        else // Right Jump
        {
            Quaternion targetRotation = directionRef[1].transform.rotation;
            while (dogRef.transform.rotation != targetRotation)
            {
                yield return new WaitForFixedUpdate();
                Quaternion deltaRotation = Quaternion.RotateTowards(dogRef.transform.rotation, targetRotation, angularVelocity * Time.deltaTime);
                dogRef.transform.rotation = deltaRotation;
            }
            dogAnim.SetTrigger("Jump");
            dogRef.GetComponent<Rigidbody>().AddForce(targetRotation * (new Vector3(0f, 1f, 0.7f)) * jumpForce, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(2);

        // Returns the dog to starting position
        StartCoroutine(dogManager.MoveToPosition(startPosition, startRotation));
        yield return null;
    }

    // check for idle animation start - Animation Event listener
    IEnumerator WailTillIdleAnimation()
    {
        while (!nextInstruct)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    #endregion Coroutines

    #region EventTriggers

    // Event trigger - BeginDrag
    public void OnBeginDrag(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        swipeDataCollection[-pointData.pointerId].startPoint = pointData.position;
    }

    // Event trigger - EndDrag
    public void OnEndDrag(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        swipeDataCollection[-pointData.pointerId].endPoint = pointData.position;

        if (pointData.pointerId == -1 && !swipeDataCollection[2].isActive)
        {
            if (!combo)
            {
                SwipeRecognizer.RecogonizeSwipe(swipeDataCollection[-pointData.pointerId], out pattern);
            }
        }
        dataReset(-pointData.pointerId);
    }

    // Event trigger - Drag
    public void OnDrag(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        swipeDataCollection[-pointData.pointerId].swipeData.Add(pointData.position);
    }

    // Event trigger - Pointer Click
    public void OnClickEnd(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        if (pointData.pointerId == -1 && !swipeDataCollection[2].isActive)
        {
            // check for tap
            if (swipeDataCollection[-pointData.pointerId].swipeData.Count <= 1)
            {
                // check for double tap
                if (pointData.clickCount == 2)
                {
                    pattern = SwipeRecognizer.TouchPattern.doubleTap;
                    //Debug.Log ("dTap");
                }
                else
                {
                    pattern = SwipeRecognizer.TouchPattern.singleTap;
                    //Debug.Log ("sTap");
                }
            }
        }
        swipeDataCollection[-pointData.pointerId].isActive = false;
        swipeDataCollection[-pointData.pointerId].holdTime = 0;
    }

    // Event Trigger - Pointer Down
    public void OnPointerDown(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        swipeDataCollection[-pointData.pointerId].isActive = true;
    }

    // Event Trigger - Pointer Up
    public void OnPointerUp(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        swipeDataCollection[-pointData.pointerId].isActive = false;
        swipeDataCollection[-pointData.pointerId].holdTime = 0;
    }

    #endregion EventTriggers

    #region ButtonCallbacks

    // Back button
    public void OnMainMenu()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }

    // Restart button
    public void OnRestart()
    {
        interrupt = true;
        CancelInvoke();
        waitForReset = false;
        pattern = SwipeRecognizer.TouchPattern.reset;
        gameOn = true;
        nextInstruct = true;
        combo = false;
        chance = 0;
        points = 0;
        combo = false;
        //timer = 0;
        gameOverPanel.SetActive(false);
        analogTimer.transform.parent.gameObject.SetActive(true);
        roundInfo.text = "Score: 0";
        dogRef.transform.position = startPosition;
        dogRef.transform.rotation = startRotation;
        dogAnim.SetTrigger("Idle");
        if (!instructON)
        {
            StartCoroutine(Instruct());
        }
    }

    #endregion ButtonCallbacks
}