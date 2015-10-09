/**
Script Author : Vaikash
Description   : Game Manager for Petting
**/

using UnityEngine;
using System.Collections;

public class PettingManager : MonoBehaviour
{

    #region Var
    public static PettingManager instRef;

    public enum Petting
    {
        idle, // Default State
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
    public Vector2 doubleTapPos;
    public bool tapOnFloor;

    float time;

    delegate void StateHandle();
    StateHandle PuppyHandle;

    #endregion Var



    public void Awake()
    {
        instRef = this;

        
    }

    void Start()
    {
        time = 0;

        // Add event listeners - TouchInput
        TouchManager.PatternRecognized += PatternRecognizedEvent;
        DogManager.instRef.ResetComplete += ResetMoveFlag;
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

    void Idle()
    {
        Timer();
        if(time>10f)
        {
            // random behavior
        }
    }

    void Tickling()
    {
        Timer();
    }

    void HoldTwoFoot()
    {
        Timer();
        if(time>10f)
        {
            puppyAnim.SetInteger("PuppyState", 0);
            ResetTimer();
            puppyState = Petting.idle;
        }
    }

    void Stretch()
    {
        Timer();

    }

    void PlayBall()
    {
        Timer();
    }

    void RollOnFloor()
    {
        Timer();
    }

    void PickOneFoot()
    {
        Timer();
    }

    void MoveAround()
    {
        Timer();
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
                //tapOnFloor = false;
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
                        // need to reset to idle
                    }
                }
            }

            else if (pattern == SwipeRecognizer.TouchPattern.hold)
            {

                puppyAnim.SetInteger("PuppyState", 2);
                puppyState = Petting.pickOneFoot;
                ResetTimer();
                PuppyHandle = PickOneFoot;
                // need to reset to idle
            }

            else if (pattern == SwipeRecognizer.TouchPattern.pinchOut)
            {
                puppyAnim.SetTrigger("Stretch");
                puppyState = Petting.stretch;
                ResetTimer();
                PuppyHandle = Stretch;
                // need to reset to idle
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
        puppyState = Petting.idle;
    }



    #endregion EventHandlers


    #region Coroutines

    IEnumerator ResetFlagAferSecs(float time, bool resetAnim)
    {
        yield return new WaitForSeconds(time);
        if (resetAnim)
        {
            puppyAnim.SetInteger("PuppyState", 0);
        }
        puppyState = Petting.idle;
        yield return null;
    }


    #endregion Coroutines
}
