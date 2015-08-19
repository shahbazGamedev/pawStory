/**
Script Author : Vaikash 
Description   : Catch Training - Trigger to catch ball
**/

using UnityEngine;
using System.Collections;

public class CatchTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//trigger for catch
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="Ball")
		{
			CatchTrainer.instRef.isHoldingBall = true;
			Destroy (other.gameObject);
		}
	}
}
