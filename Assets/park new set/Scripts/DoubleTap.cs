using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DoubleTap : MonoBehaviour {

    public SwipeRecognizer.TouchDataCollection[] touchDataCollection;
    
    public bool skipRecognizer;

    SwipeRecognizer.TouchPattern pattern;
    SwipeRecognizer.TouchPattern gestureCache;

    public delegate void TouchEventBroadcastSystem(SwipeRecognizer.TouchPattern touchPattern);
    public static event TouchEventBroadcastSystem PatternRecognized;

    // Use this for initialization
    void Start () {
        pattern = SwipeRecognizer.TouchPattern.reset;
    }
	
	// Update is called once per frame
	void Update () {
        if (pattern != SwipeRecognizer.TouchPattern.reset)
        {
            if (PatternRecognized != null)
                PatternRecognized(pattern);
            gestureCache = pattern;
            // Debug.Log (gestureCache);
            pattern = SwipeRecognizer.TouchPattern.reset;
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

                    //Debug.Log("DoubleClick");
                    DogNavigation.instRef.doubleTapPos = pointData.position;

                    pattern = SwipeRecognizer.TouchPattern.doubleTap;
                    //Debug.Log ("dTap");

                }
                else
                {
                    //pattern = SwipeRecognizer.TouchPattern.singleTap;
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
}
