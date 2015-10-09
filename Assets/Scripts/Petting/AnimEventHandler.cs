﻿/**
Script Author : Vaikash
Description   : Anim Event Listener - Petting
**/

using UnityEngine;
using System.Collections;

public class AnimEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void IdleStartFired()
    {
        Debug.Log("Fired");
        PettingManager.instRef.puppyState = PettingManager.Petting.idle;
        PettingManager.PuppyHandle = PettingManager.instRef.Idle;
    }
}
