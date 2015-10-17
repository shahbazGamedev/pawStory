/**
Script Author : Vaikash 
Description   : Process Touch Input
**/

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;

using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public Text touch1;
    public Text touch2;


    public static TouchManager instRef;

    public GameObject touchMat;
    public bool detectHold;
    public bool detectPinchOut;
    public bool setTapPos;
    public bool detectTickling;
    bool skipRecognizer;

    public SwipeRecognizer.TouchDataCollection[] touchDataCollection;

    SwipeRecognizer.TouchPattern pattern;
    SwipeRecognizer.TouchPattern gestureCache;

    public delegate void TouchEventBroadcastSystem(SwipeRecognizer.TouchPattern touchPattern);
    public static event TouchEventBroadcastSystem PatternRecognized;

    public void Awake()
    {
        instRef = this;
    }

    // Use this for initialization
    void Start()
    {
        pattern = SwipeRecognizer.TouchPattern.reset;
    }

    // Update is called once per frame
    void Update()
    {
        if (pattern != SwipeRecognizer.TouchPattern.reset)
        {
            if (PatternRecognized != null)
                PatternRecognized(pattern);
            gestureCache = pattern;
            // Debug.Log (gestureCache);
            pattern = SwipeRecognizer.TouchPattern.reset;
        }
        if (detectHold)
        {
            // Touch Timer for hold down gesture
            if (touchDataCollection[1].isActive)
                touchDataCollection[1].holdTime += Time.deltaTime;
            if (touchDataCollection[2].isActive)
                touchDataCollection[2].holdTime += Time.deltaTime;

            if (touchDataCollection[1].isActive && !touchDataCollection[2].isActive)
            {
                // Min. hold time of 1 sec
                if (touchDataCollection[1].holdTime > 1 && touchDataCollection[1].swipeData.Count <= 3)
                {
                    pattern = SwipeRecognizer.TouchPattern.hold;
                    if (setTapPos)
                    {
                        PettingManager.instRef.doubleTapPos = touchDataCollection[1].startPoint;
                    }
                    touchDataCollection[1].holdTime = 0;
                    //touchDataCollection[1].isActive = false;
                }
                // move gesture disabled
                //				else if(swipeDataCollection [1].swipeData.Count>3)
                //				{
                //					pattern = SwipeRecognizer.TouchPattern.move;
                //					swipeDataCollection [1].isActive = false;
                //				}
            }
        }

        if (detectPinchOut)
        {
            if (touchDataCollection[1].isActive && touchDataCollection[2].isActive && !skipRecognizer)
            {
                Debug.Log(touchDataCollection[1].swipeDelta);


                if (touchDataCollection[1].swipeDelta > 15)
                {
                    Debug.Log("PinchOut");

                    skipRecognizer = true;
                    pattern = SwipeRecognizer.TouchPattern.pinchOut;

                }
            }
        }
    }

    // Reset previous touch input
    void SwipeReset()
    {
        pattern = SwipeRecognizer.TouchPattern.reset;
    }


    // Reset particular swipe data
    void dataReset(int pointerID)
    {
        touchDataCollection[pointerID].swipeData.Clear();
    }

    #region EventTriggers

    // Event trigger - BeginDrag
    public void OnBeginDrag(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        
    }

    // Event trigger - EndDrag
    public void OnEndDrag(BaseEventData data)
    {

        var pointData = (PointerEventData)data;
        touchDataCollection[-pointData.pointerId].endPoint = pointData.position;

        if (pointData.pointerId == -1 && !touchDataCollection[2].isActive && !skipRecognizer)
        {
            SwipeRecognizer.RecogonizeSwipe(touchDataCollection[-pointData.pointerId], out pattern, false);
        }
        dataReset(-pointData.pointerId);

        if (!touchDataCollection[1].isActive && !touchDataCollection[2].isActive)
            skipRecognizer = false;

        if (detectTickling)
        {
            PettingManager.instRef.tickle = false;
        }
    }

    // Event trigger - Drag
    public void OnDrag(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        // Debug.Log(pointData.pointerId);
        touchDataCollection[-pointData.pointerId].swipeData.Add(pointData.position);
        touchDataCollection[-pointData.pointerId].swipeDelta += pointData.delta.magnitude;
        touchDataCollection[-pointData.pointerId].swipeDeltaPetting = pointData.delta.magnitude;
        //if (pointData.pointerId == -1)
        //    touch1.text = "" + pointData.position;
        //else if (pointData.pointerId == -2)
        //    touch2.text = "" + pointData.position;

        if (detectTickling)
        {
            var tick=touchDataCollection[-pointData.pointerId].swipeDelta / (Screen.dpi > 0 ? Screen.dpi : 240);
            //Debug.Log(tick);
            if (tick > 6 && !touchDataCollection[2].isActive)
            {
                skipRecognizer = true;
                PettingManager.instRef.tickle = true;
                Debug.Log("Tickle");
            }
        }


    }

    // Event trigger - Pointer Click
    public void OnClickEnd(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        if (pointData.pointerId == -1 && !touchDataCollection[2].isActive && !skipRecognizer)
        {
            // check for tap 
            if (touchDataCollection[-pointData.pointerId].swipeData.Count <= 1)
            {
                // check for double tap
                if (pointData.clickCount == 2)
                {
                    if (setTapPos)
                    {
                        PettingManager.instRef.doubleTapPos = pointData.position;
                    }
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
    }

    // Event Trigger - Pointer Down
    public void OnPointerDown(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        touchDataCollection[-pointData.pointerId].isActive = true;
        touchDataCollection[-pointData.pointerId].startPoint = pointData.position;
    }

    // Event Trigger - Pointer Up
    public void OnPointerUp(BaseEventData data)
    {
        var pointData = (PointerEventData)data;
        //Debug.Log(pointData.pointerId);
        touchDataCollection[-pointData.pointerId].isActive = false;
        touchDataCollection[-pointData.pointerId].holdTime = 0;
        touchDataCollection[-pointData.pointerId].swipeDelta = 0;
    }

    #endregion
}
