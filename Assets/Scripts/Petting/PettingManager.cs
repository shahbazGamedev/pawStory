/**
Script Author : Vaikash
Description   : Game Manager for Petting
**/

using UnityEngine;
using System.Collections;



public class PettingManager : MonoBehaviour {

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

    delegate void StateHandle();
    StateHandle PuppyHandle;

    #endregion Var

    void Start () {

        instRef = this;
        // Add event listeners - TouchInput
        TouchManager.PatternRecognized += PatternRecognizedEvent;
        // EventMgr.GameRestart += OnResetBtn;
    }

    // Decouple Event Listeners
    void OnDisable()
    {
        TouchManager.PatternRecognized -= PatternRecognizedEvent;
        // EventMgr.GameRestart -= OnResetBtn;
    }


    void Update () {

        if(PuppyHandle!=null)
        {
            PuppyHandle();
        }
	}

    #region PuppyStateHandlers

    void Idle()
    {

    }

    void Tickling()
    {

    }

    void HoldTwoFoot()
    {
       
    }

    void Stretch()
    {

    }

    void PlayBall()
    {

    }

    void RollOnFloor()
    {

    }

    void PickOneFoot()
    {

    }

    void MoveAround()
    {

    }

    void Eating()
    {

    }

    void Medicine()
    {

    }

    void Sleep()
    {

    }

    void WakeUp()
    {

    }

    void Bath()
    {

    }

    #endregion PuppyStateHandlers

    #region EventHandlers

    //Event Handler for PatternRecognized Event
    void PatternRecognizedEvent(SwipeRecognizer.TouchPattern pattern)
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
                }
                if (layerName == "Dog")
                {
                    Debug.Log(pattern);
                    PuppyHandle = HoldTwoFoot;
                    puppyAnim.SetInteger("PuppyState", 1);
                    puppyState = Petting.holdTwoFoot;
                }
            }
        }
        if(puppyState == Petting.holdTwoFoot && pattern == SwipeRecognizer.TouchPattern.swipeUp)
        {
            puppyAnim.SetTrigger("TwoLegJump");
        }
        if (pattern == SwipeRecognizer.TouchPattern.hold)
        {
            PuppyHandle = PickOneFoot;
            puppyAnim.SetInteger("PuppyState", 2);
            puppyState = Petting.pickOneFoot;
        }
        if(pattern == SwipeRecognizer.TouchPattern.pinchOut)
        {
            PuppyHandle = Stretch;
            puppyAnim.SetTrigger("Stretch");
            puppyState = Petting.stretch;
        }
        if (pattern == SwipeRecognizer.TouchPattern.swipeDown && puppyState != Petting.rollOnFloor)
        {
            PuppyHandle = RollOnFloor;
            puppyAnim.SetInteger("PuppyState", 3);
            puppyState = Petting.rollOnFloor;
        }
        if ((pattern == SwipeRecognizer.TouchPattern.swipeLeft || pattern == SwipeRecognizer.TouchPattern.swipeRight) && puppyState == Petting.rollOnFloor)
        {
            //PuppyHandle = RollOnFloor;
            puppyAnim.SetTrigger("Rotate");
            //puppyState = Petting.rollOnFloor;
        }
        if (pattern == SwipeRecognizer.TouchPattern.swipeUp && puppyState == Petting.rollOnFloor)
        {
            //PuppyHandle = RollOnFloor;
            puppyAnim.SetInteger("PuppyState", 0);
            //puppyState = Petting.rollOnFloor;
        }

    }

    #endregion EventHandlers
}
