/**
Script Author : Vaikash 
Description   : Event Listener for dog slide animation
**/

using UnityEngine;
using System.Collections;

public class AnimEventListener : MonoBehaviour {

	void SlideAnimComplete()
	{
		AgilityManager.instanceRef.ResetColliderSize ();
	}
}
