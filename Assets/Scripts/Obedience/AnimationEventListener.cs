/**
Script Author : Vaikash 
Description   : Animation Event Listener for IdleLook2Sides
**/

using UnityEngine;
using System.Collections;

public class AnimationEventListener : MonoBehaviour {
	public GameObject Manager;
	ObedienceManager obedienceManager;
	// Use this for initialization
	void Start () {
		obedienceManager = Manager.GetComponent <ObedienceManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void IdleStartFired()
	{
		if (obedienceManager.registerAnimEvent) {
			obedienceManager.nextInstruct = true;
			obedienceManager.registerAnimEvent = false;
		}
	}
}
