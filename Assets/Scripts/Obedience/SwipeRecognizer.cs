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
		singleTap,
		doubleTap,
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
		doubleCircle,

		move,
		hold,

		pinchOut,

		reset,
		tryAgain

	};

	public struct Swipe
	{
		public Vector2 swipeStartPos;
		public Vector2 swipeEndPos;
		public float swipeAngle;
		public float swipeLength;
		public TouchPattern pattern;
	};

	/// <summary>
	/// Recogonizes the swipe.
	/// </summary>
	/// <returns><c>true</c>, if swipe was recogonized, <c>false</c> otherwise.</returns>
	/// <param name="startPoint">Start point.</param>
	/// <param name="endPoint">End point.</param>
	/// <param name="swipeData">Drag data.</param>
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
		int incrementValue;

		Vector2 currentVector;
		Vector2 previousVector;
		Vector2 midPoint;

		if(swipeData.Count<6)
		{
			return false;
		}

		// Sample the swipe data into multiple segments - lag fix
		if (swipeData.Count < 12)
			incrementValue = 1;
		else if (swipeData.Count < 48)
			incrementValue = 4;
		else
			incrementValue = swipeData.Count/12;

		// Sets midpoint based on circle or double circle
		if(Vector2.Distance (endPoint,swipeData[swipeData.Count/2])>Vector2.Distance (endPoint,swipeData[swipeData.Count/4]))
		{
			midPoint = (swipeData[swipeData.Count/2]+endPoint) * 0.5f;
		}
		else
		{
			midPoint = (swipeData[swipeData.Count/4]+endPoint) * 0.5f;
		}

		previousVector = startPoint - midPoint; // Transform the vector from screen origin to calculated midpoint


		// Preliminery check to avoid unwanted calculations
		if(AngleBetweenVectors (previousVector,(swipeData[swipeData.Count/3]-midPoint))>25)
		{
			//Debug.Log ("totalAngle");
			// Calculate angle for each sample and find total angle
			for(int i=0;i<swipeData.Count;i+=incrementValue)
			{
				currentVector = swipeData [i] - midPoint;
				deltaAngle = AngleBetweenVectors (previousVector, currentVector);
				totalAngle += deltaAngle;
				previousVector = currentVector;
				//Debug.Log (deltaAngle);
			}
			//Debug.Log (totalAngle);
			//  Check if circle is atleast 240 out of 360
			if(totalAngle>240)
			{
				previousVector = startPoint - midPoint;
				currentVector=swipeData[swipeData.Count/4]-midPoint;

				// Check for direction of circle
				//float angle = Mathf.Atan2 (currentVector.y-previousVector.y, currentVector.x-previousVector.x) * Mathf.Rad2Deg;
				float angle = currentVector.x * previousVector.y - currentVector.y * previousVector.x > 0 ? -1 : 1;

				//Debug.Log (totalAngle);
				if (totalAngle > 600) 
				{
					pattern = TouchPattern.doubleCircle;
					return true;
				}
				
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

	/// <summary>
	/// Recogonizes the swipe.
	/// </summary>
	/// <returns><c>true</c>, if swipe was recogonized, <c>false</c> otherwise.</returns>
	/// <param name="startPoint">Start point.</param>
	/// <param name="endPoint">End point.</param>
	/// <param name="swipeData">Swipe data.</param>
	/// <param name="swipe">Swipe.</param>
	/// <param name="circle">If set to <c>false</c> skips circle swipe check.</param>
	// Overloading to use struct swipe as out parameter
	public static bool RecogonizeSwipe (Vector2 startPoint, Vector2 endPoint, List<Vector2> swipeData, out Swipe swipe, bool circle)
	{
		TouchPattern pattern = TouchPattern.tryAgain;
		TouchPattern patternLocal;
		Vector2 transformedVector;

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
		if(circle)
		{
			if (RecognizeCircleSwipe (startPoint, endPoint, swipeData, out patternLocal))
			{
				pattern = patternLocal;
			}
		}

		// fill the out argument - Swipe struct
		swipe.pattern = pattern;
		swipe.swipeStartPos = startPoint;
		swipe.swipeEndPos = endPoint;

		// calculate swipe angle as required for quateranion.eulers
		transformedVector = endPoint - startPoint;
		swipe.swipeAngle = Vector2.Angle (Vector2.up, transformedVector);
		swipe.swipeAngle *= transformedVector.x * Vector2.up.y - transformedVector.y * Vector2.up.x > 0 ? 1 : -1;

		if (swipe.swipeAngle < 0)
			swipe.swipeAngle += 360;
		
		// calculate swipe length to be used as force factor for dog jump (jump and shoot module)
		//swipe.swipeLength = Vector2.Distance (startPoint, endPoint);
		swipe.swipeLength = Vector2.Distance (startPoint, endPoint)/Screen.dpi;
		swipe.swipeLength *= 160;

		// Return false if none recognized
		if (pattern == TouchPattern.tryAgain)
			return false;

		else
			return true;
	}

	/// <summary>
	/// Recogonizes the swipe - Adopted for Obedience Module.
	/// </summary>
	/// <returns><c>true</c>, if swipe was recogonized, <c>false</c> otherwise.</returns>
	/// <param name="swipeData">Swipe data - Struct.</param>
	/// <param name="pattern">Pattern.</param>
	public static bool RecogonizeSwipe (ObedienceManager.SwipeDataCollection swipeData, out TouchPattern pattern) 
	{
		pattern = TouchPattern.tryAgain;
		TouchPattern patternLocal;

		float xComponent;
		float yComponent;
		float angle;

		xComponent= (-swipeData.startPoint.x + swipeData.endPoint.x);
		yComponent = (-swipeData.startPoint.y + swipeData.endPoint.y);

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
		if (RecognizeCircleSwipe (swipeData.startPoint, swipeData.endPoint, swipeData.swipeData, out patternLocal))
			pattern = patternLocal;

		// Return false if none recognized
		if (pattern == TouchPattern.tryAgain)
			return false;

		else
			return true;
	}


	// Calculate angle between two vectors
	static float AngleBetweenVectors(Vector2 previousVector, Vector2 currentVector)
	{
		float deltaAngle = Vector2.Angle (previousVector, currentVector) ;
		return deltaAngle;
	}
}
