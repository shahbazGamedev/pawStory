/**
Script Author : Vaikash 
Description   : Lap Trigger Script
**/

using UnityEngine;
using System.Collections;

public class LapTrigger : MonoBehaviour {

	EllipseMovement dogMove;

	// Use this for initialization
	void Start () {
		dogMove = FindObjectOfType <EllipseMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="Player")
			dogMove.FireLapTriggeredEvent ();
	}
}
