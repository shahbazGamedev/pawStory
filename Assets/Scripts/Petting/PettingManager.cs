/**
Script Author : Vaikash
Description   : Game Manager for Petting
**/

using UnityEngine;
using System.Collections;
using System.Linq;

public class PettingManager : MonoBehaviour
{

    #region Var

    public static PettingManager instRef;

    public enum Petting
    {
        idle, // Default State
        puppyReact,
        tickling,
        holdTwoFoot,
        stretch,
        playBall,
        rollOnFloor,
        pickOneFoot,
        moveAround,

        // Later Event Based Activities

        eating,
        medicine,
        sleep,
        wakeUp,
        bath
    }

    public Petting puppyState;
    public Animator puppyAnim;
    public GameObject ballRef;
    Rigidbody ballRb;
    public Vector2 doubleTapPos;
    //public bool tapOnFloor;
    public bool tickle;
    public bool playBall;
    public bool holdBall;
    public bool puppyReactBall;
    public float idleTime;

    public float time;

    bool setPrevData;
    public float prevTouch;
    public float ballMoveDelta;

    bool skipResetToIdle;
    Vector3 startPos;
    Vector3 targetPosBall;

    public delegate void StateHandle();
    public static StateHandle PuppyHandle;

    #endregion Var

    public void Awake()
    {
        instRef = this;
        ballRb = ballRef.GetComponent<Rigidbody>();
    }

    void Start()
    {
        time = 0;
        PuppyHandle = Idle;
        setPrevData = true;
        startPos = DogManager.instRef.dogReference.transform.position;


        // Add event listeners - TouchInput
        TouchManager.PatternRecognized += PatternRecognizedEvent;
        DogManager.instRef.ResetComplete += ResetMoveFlag;
        EventMgr.GameRestart += OnReset;
        // EventMgr.GameRestart += OnResetBtn;
    }

    // Decouple Event Listeners
    void OnDisable()
    {
        TouchManager.PatternRecognized -= PatternRecognizedEvent;
        DogManager.instRef.ResetComplete -= ResetMoveFlag;
        // EventMgr.GameRestart -= OnResetBtn;
    }


    void Update()
    {
        if (PuppyHandle != null)
        {
            PuppyHandle();
        }
    }

    #region PuppyStateHandlers

    public void Idle()
    {
        Timer();
        if(tickle)
        {
            Debug.Log("tick");
            puppyAnim.SetTrigger("Stretch");
            puppyState = Petting.tickling;
            ResetTimer();
            PuppyHandle = Tickling;
            // need to reset to idle
        }
        if(time>idleTime)
        {
            // TODO
            // Random Puppy Behavior from a list
            ResetTimer();
            puppyAnim.SetTrigger("Idle01");
            puppyState = Petting.puppyReact;
            PuppyHandle = PuppyReact;
        }
    }

    void PuppyReact()
    {
        Timer();
        if(time>10f)
        {
            ResetTimer();
            PuppyHandle = Idle;
        }

    }

    void Tickling()
    {
        Timer();
        if(!tickle)
        {
            ResetToIdle();
        }
    }

    void HoldTwoFoot()
    {
        Timer();
        if(time>5f)
        {
            ResetToIdle();
        }
    }

    void Stretch()
    {
        Timer();
    }

    void PlayBall()
    {
        Timer();
        ballRb.isKinematic = true;
        if (TouchManager.instRef.touchDataCollection[1].isActive)
        {
            //if (setPrevData)
            //{
            //    prevTouch = TouchManager.instRef.touchDataCollection[1].startPoint.x;
            //    setPrevData = false;
            //}

            targetPosBall = ballRef.transform.position;

            //ballMoveDelta = TouchManager.instRef.touchDataCollection[1].swipeData.Last().x-setPrevData;
            //ballRef.transform.position += new Vector3(ballMoveDelta, 0, 0);

            var screenPoint = new Vector3( TouchManager.instRef.touchDataCollection[1].swipeData.LastOrDefault().x,  TouchManager.instRef.touchDataCollection[1].swipeData.LastOrDefault().y, 0f);


            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out hit, 300f))
            {
                string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (layerName == "Back" || layerName == "Toys")
                {
                    var worldPoint = hit.point + (Vector3.up * 0.01f);
                    targetPosBall.x = worldPoint.x;
                    targetPosBall.y = worldPoint.y;
                    ballRb.MovePosition(targetPosBall);
                }
            }


            if (puppyReactBall)
            {
                puppyReactBall = false;
                puppyAnim.SetTrigger("Jump");
                skipResetToIdle = true;
                ResetTimer();
            }
        }
        else
        {
            // reset dog state
            ballRb.isKinematic = false;
            setPrevData = true;
            ResetToIdle();
        }

    }

    void RollOnFloor()
    {
        Timer();
        if (time > 5f)
        {
            ResetToIdle();
        }
    }

    void PickOneFoot()
    {
        Timer();
        if (time > 5f)
        {
            ResetToIdle();
        }
    }

    void MoveAround()
    {
        Timer();
        if (time > 10f)
        {
            ResetToIdle();
        }
    }

    void Eating()
    {
        Timer();
    }

    void Medicine()
    {
        Timer();
    }

    void Sleep()
    {
        Timer();
    }

    void WakeUp()
    {
        Timer();
    }

    void Bath()
    {
        Timer();
    }

    void Timer()
    {
        time += Time.deltaTime;
    }

    void ResetTimer()
    {
        time = 0;
    }

    public void ResetToIdle()
    {
        if (!skipResetToIdle)
        {
            puppyAnim.SetInteger("PuppyState", 0);
            ResetTimer();
            puppyState = Petting.idle;
            PuppyHandle = Idle;
        }
        else
            skipResetToIdle = false;
    }

    #endregion PuppyStateHandlers

    #region EventHandlers

    //Event Handler for PatternRecognized Event
    void PatternRecognizedEvent(SwipeRecognizer.TouchPattern pattern)
    {
        Debug.Log(pattern);

        if(puppyState == Petting.idle)
        {
            if (pattern == SwipeRecognizer.TouchPattern.doubleTap)
            {
                PuppyHandle = MoveAround;
                puppyState = Petting.moveAround;

                var screenPoint = new Vector3(doubleTapPos.x, doubleTapPos.y, 0f);
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(screenPoint);
                if (Physics.Raycast(ray, out hit, 300f))
                {
                    string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    if (layerName == "Toys")
                    {
                        var worldPoint = hit.point + (Vector3.up * 0.01f);

                        // code for move dog to target
                        StartCoroutine(DogManager.instRef.MoveToPosition(worldPoint, Quaternion.identity));

                        puppyState = Petting.moveAround;
                        ResetTimer();
                        PuppyHandle = MoveAround;
                    }
                    else if (layerName == "Dog")
                    {
                        puppyAnim.SetInteger("PuppyState", 1);
                        puppyState = Petting.holdTwoFoot;
                        ResetTimer();
                        PuppyHandle = HoldTwoFoot;
                    }
                }
            }

            else if (pattern == SwipeRecognizer.TouchPattern.hold)
            {

                if (playBall)
                {
                    var screenPoint = new Vector3(doubleTapPos.x, doubleTapPos.y, 0f);
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(screenPoint);
                    if (Physics.Raycast(ray, out hit, 300f))
                    {
                        string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                        if (layerName == "Toys")
                        {
                            Debug.Log("PlayBall");
                            var worldPoint = hit.point + (Vector3.up * 0.01f);

                            holdBall = true;

                            puppyState = Petting.playBall;
                            ResetTimer();
                            PuppyHandle = PlayBall;
                        }
                        else
                        {
                            puppyAnim.SetInteger("PuppyState", 2);
                            puppyState = Petting.pickOneFoot;
                            ResetTimer();
                            PuppyHandle = PickOneFoot;
                            // need to reset to idle
                        }
                    }
                }
                else
                {
                    puppyAnim.SetInteger("PuppyState", 2);
                    puppyState = Petting.pickOneFoot;
                    ResetTimer();
                    PuppyHandle = PickOneFoot;
                    // need to reset to idle
                }
            }

            else if (pattern == SwipeRecognizer.TouchPattern.pinchOut)
            {
                puppyAnim.SetTrigger("Stretch");
                puppyState = Petting.stretch;
                ResetTimer();
                PuppyHandle = Stretch;
            }

            else if (pattern == SwipeRecognizer.TouchPattern.swipeDown)
            {
                puppyAnim.SetInteger("PuppyState", 3);
                ResetTimer();
                PuppyHandle = RollOnFloor;
                puppyState = Petting.rollOnFloor;
            }
        }

        else if(puppyState == Petting.rollOnFloor)
        {
            if ((pattern == SwipeRecognizer.TouchPattern.swipeLeft || pattern == SwipeRecognizer.TouchPattern.swipeRight))
            {
                puppyAnim.SetTrigger("Rotate");
                ResetTimer();
                puppyState = Petting.rollOnFloor;
            }
            else if (pattern == SwipeRecognizer.TouchPattern.swipeUp)
            {
                puppyAnim.SetInteger("PuppyState", 0);
                ResetTimer();
                puppyState = Petting.idle;
            }
        }

        else if(puppyState == Petting.holdTwoFoot)
        {
            if ( pattern == SwipeRecognizer.TouchPattern.swipeUp)
            {
                puppyAnim.SetTrigger("TwoLegJump");
                ResetTimer();
            }
            else if (pattern == SwipeRecognizer.TouchPattern.swipeDown)
            {
                puppyAnim.SetInteger("PuppyState", 0);
                ResetTimer();
                PuppyHandle = Idle;
                puppyState = Petting.idle;
            }
        }
    }

    void ResetMoveFlag()
    {
        ResetToIdle();
    }

    #endregion EventHandlers


    #region Coroutines

    IEnumerator ResetFlagAferSecs(float time, bool resetAnim)
    {
        yield return new WaitForSeconds(time);
        //if (resetAnim)
        //{
        //    puppyAnim.SetInteger("PuppyState", 0);
        //}
        //puppyState = Petting.idle;
        ResetToIdle();
        yield return null;
    }

    IEnumerator Reset()
    {
        yield return StartCoroutine(DogManager.instRef.MoveToPosition(startPos, Quaternion.identity));
        ResetToIdle();
    }
    #endregion Coroutines


    #region BtnCallbacks

    void OnReset()
    {

        StartCoroutine(Reset());

    }

    #endregion BtnCallbacks
}
