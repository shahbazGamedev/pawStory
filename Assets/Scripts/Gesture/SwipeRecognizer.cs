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
	public static bool RecogonizeSwipe (Vector2 startPoint, Vector2 endPoint, List<Vector2> swipeData, out TouchPattern pattern)
	{
		pattern = TouchPattern.tryAgain;
		TouchPattern patternLocal;

		float xComponent;
		float yComponent;
		float angle;

		xComponent= (-startPoint.x + endPoint.x);
		yComponent = (-startPoint.y + endPoint.y);

		// returns 0 on right, 180 on left, 90 on top and -90 on bottom
		angle = Mathf.Atan2 (yComponent, xComponent) * Mathf.Rad2Deg;

		// Mapping angle to 8 directions 0 - 180
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

		// Mapping angle to 8 directions 0 - -180
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

		// Check if gesture is circle
		if (RecognizeCircleSwipe (startPoint, endPoint, swipeData, out patternLocal))
			pattern = patternLocal;

		// Return false if none recognized
		if (pattern == TouchPattern.tryAgain)
			return false;
		
		else
			return true;
	}

	static bool RecognizeCircleSwipe (Vector2 startPoint, Vector2 endPoint, List<Vector2> swipeData, out TouchPattern pattern)
	{
		pattern = TouchPattern.tryAgain;

		float totalAngle=0f;
		float deltaAngle=0f;

		// Sample the swipe data into multiple segments and find midpoint
		int incrementValue = swipeData.Count / 8;
		Vector2 midPoint = (swipeData[swipeData.Count/2]+endPoint) * 0.5f;

		Vector2 currentVector;
		Vector2 previousVector;

		previousVector = startPoint - midPoint; // Transform the vector from screen origin to calculated midpoint

		// Preliminery check to avoid unwanted calculations
		if(AngleBetweenVectors (previousVector,(swipeData[swipeData.Count/8]-midPoint))>15)
		{
			
			// Calculate angle for each sample and find total angle
			for(int i=0;i<swipeData.Count;i+=incrementValue)
			{
				currentVector = swipeData [i] - midPoint;
				deltaAngle = AngleBetweenVectors (previousVector, currentVector);
				totalAngle += deltaAngle;
				previousVector = currentVector;
				//Debug.Log (deltaAngle);
			}

			//  Check if circle is atleast 240 out of 360
			if(totalAngle>240)
			{
				previousVector = startPoint - midPoint;
				currentVector=swipeData[swipeData.Count/4]-midPoint;

				// Check for direction of circle
				//float angle = Mathf.Atan2 (currentVector.y-previousVector.y, currentVector.x-previousVector.x) * Mathf.Rad2Deg;
				float angle = currentVector.x * previousVector.y - currentVector.y * previousVector.x > 0 ? -1 : 1;

				if(angle>=0)
				{
					pattern = TouchPattern.antiClockwiseCircle;
					return true;
				}

				else
				{
					pattern = TouchPattern.clockwiseCircle;
					return true;
				}
			}
		}

		//Debug.Log ("false");
		return false;
	}

	// Calculate angle between two vectors
	static float AngleBetweenVectors(Vector2 previousVector, Vector2 currentVector)
	{

		float deltaAngle = Vector2.Angle (previousVector, currentVector) ;
		return deltaAngle;
	}
}
