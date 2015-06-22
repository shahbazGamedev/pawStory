/**
Script Author : Vaikash 
Description   : Swipe Recognizer
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeRecognizer
{

	public enum TouchPattern // Possible gestures
	{
		swipeUp,
		swipeDown,
		swipeLeft,
		swipeRight,

		swipeUpRight,
		swipeUpLeft,
		swipeDownRight,
		swipeDownLeft,

		clockwiseCircle,
		antiClockwiseCircle,
		tryAgain,
		reset
	};

	/// <summary>
	/// Recogonizes the swipe.
	/// </summary>
	/// <returns><c>true</c>, if swipe was recogonized, <c>false</c> otherwise.</returns>
	/// <param name="startPoint">Start point.</param>
	/// <param name="endPoint">End point.</param>
	/// <param name="dragData">Drag data.</param>
	/// <param name="pattern">Returns predefined TouchPattern if successful else tryAgain .</param>
	// Called at end of drag to recogonize the gesture
	public static bool RecogonizeSwipe (Vector2 startPoint, Vector2 endPoint, List<Vector2> dragData, out TouchPattern pattern)
	{
		pattern = TouchPattern.tryAgain;
		float xComponent;
		float yComponent;
		float angle;
		xComponent= (-startPoint.x + endPoint.x);
		yComponent= (-startPoint.y + endPoint.y);
		angle = Mathf.Atan2 (yComponent, xComponent) * Mathf.Rad2Deg; // 0 on right, 180 on left, 90 on top and -90 on bottom
		//Debug.Log (angle);
		if(angle>0)
		{
			if(angle > 75 && angle <= 105)
			{
				pattern = TouchPattern.swipeUp;
			}
			else if(angle > 15 && angle <= 75)
			{
				pattern = TouchPattern.swipeUpRight;
			}
			else if(angle >= 0 && angle <= 15)
			{
				pattern = TouchPattern.swipeRight;
			}
			else if(angle > 105 && angle <= 165)
			{
				pattern = TouchPattern.swipeUpLeft;
			}
			else
			{
				pattern = TouchPattern.swipeLeft;
			}
		}
		else
		{
			if(angle < -75 && angle >= -105)
			{
				pattern = TouchPattern.swipeDown;
			}
			else if(angle < -15 && angle >= -75)
			{
				pattern = TouchPattern.swipeDownRight;
			}
			else if(angle <= 0 && angle >= -15)
			{
				pattern = TouchPattern.swipeRight;
			}
			else if(angle < -105 && angle >= -165)
			{
				pattern = TouchPattern.swipeDownLeft;
			}
			else
			{
				pattern = TouchPattern.swipeLeft;
			}
		}

		if (pattern == TouchPattern.tryAgain)
			return false;
		else
			return true;
	}

//	bool RecognizeCircleSwipe (Vector2 startPoint, Vector2 endPoint, List<Vector2> dragData, out TouchPattern pattern)
//	{
//		const float MINIMUM_SQR_DISTANCE = 0.01f;
//		int incrementValue = dragData.Count / 8;
//		Vector2 midPoint = (endPoint - startPoint) * 0.5f;
//		Vector2 currentVector;
//		Vector2 previousVector = startPoint - midPoint;
//		float totalAngle;
//		float deltaAngle;
//		float value;
//		for(int i=0;i<dragData.Count;i+=incrementValue)
//		{
//			currentVector = dragData[i] - midPoint;
//			value = Mathf.Min (1, Vector2.Dot (previousVector, currentVector) / (previousVector.magnitude * currentVector.magnitude));
//			deltaAngle=Mathf.Acos (value);
//			totalAngle += deltaAngle;
//		}
//		if(totalAngle>300 || totalAngle<-300)
//		{
//			if(totalAngle>300)
//			{
//				pattern = TouchPattern.antiClockwiseCircle;
//			}
//			else
//			{
//				pattern = TouchPattern.clockwiseCircle;
//			}
//		}
//
//		return true;
//	}
}
