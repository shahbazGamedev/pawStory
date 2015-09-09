/**
Script Author : Vaikash 
Description   : Makes the object to blink
**/

using UnityEngine;
using System.Collections;

public class MarkerBlink : MonoBehaviour {

	public float cycleTime;
	Color originalColor;

	void Start()
	{
		originalColor = GetComponent <Renderer> ().material.color;
		cycleTime=Mathf.Abs(cycleTime);
		if(cycleTime<=0f)
			cycleTime=1.0f;
	}

	void LateUpdate()
	{
		var timer = Time.time / cycleTime;
		timer = Mathf.Abs ((timer - Mathf.Floor (timer)) * cycleTime - 1);
		GetComponent <Renderer>().material.color = Color.Lerp( originalColor, Color.red, timer );
	}
}
