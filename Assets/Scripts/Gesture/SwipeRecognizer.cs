/**
Script Author : Vaikash 
Description   : Swipe Recognizer
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic ;

public class SwipeRecognizer {

	public enum TouchPattern // Possible gestures
	{
		swipeUp, swipeDown, swipeLeft, swipeRight, clockwiseCircle, antiClockwiseCircle, tryAgain
	};

	/// <summary>
	/// Recogonizes the swipe.
	/// </summary>
	/// <returns><c>true</c>, if swipe was recogonized, <c>false</c> otherwise.</returns>
	/// <param name="startPoint">Start point.</param>
	/// <param name="endPoint">End point.</param>
	/// <param name="dragData">Drag data.</param>
	/// <param name="pattern">Pattern.</param>
	// Called at end of drag to recogonize the gesture
	public static bool RecogonizeSwipe(Vector2 startPoint, Vector2 endPoint, List<Vector2> dragData, out TouchPattern pattern)
	{
		pattern = TouchPattern.tryAgain;
		float swipeMinDist=2.0f;
		float swipeDelta;
		swipeDelta = startPoint.magnitude - endPoint.magnitude;
		if (Mathf.Abs (swipeDelta) > swipeMinDist) {
			// Its a swipe
			if (swipeDelta > 0) {
				// Left and Bottom
				if (Mathf.Abs (startPoint.x - endPoint.x) > Mathf.Abs ((startPoint.y - endPoint.y))) {
					// Its swipeLeft
					pattern=TouchPattern.swipeLeft;
				} 
				else {
					// Its swipeDown
					pattern=TouchPattern.swipeDown;
				}
			} 
			else {
				// Right and Top
				if (Mathf.Abs (startPoint.x - endPoint.x) > Mathf.Abs ((startPoint.y - endPoint.y))) {
					// Its swipeRight
					pattern=TouchPattern.swipeRight;
				} 
				else {
					// Its swipeUp
					pattern=TouchPattern.swipeUp;
				}
			}
		}
		if (pattern == TouchPattern.tryAgain)
			return false;
		else
			return true;
	}
}
