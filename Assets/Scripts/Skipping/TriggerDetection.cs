/**
Script Author : Vaikash 
Description   : Manages Trigger in Skipping Game
**/

using UnityEngine;
using System.Collections;

public class TriggerDetection : MonoBehaviour {
	
	public delegate void DogSkipping();
	public static event DogSkipping DogHitSkipRope;
	public static event DogSkipping SkipRopeReset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="target" && this.gameObject.tag=="Finish")
		{
			Debug.Log ("Rope reset");
			if(SkipRopeReset!=null)
			{
				SkipRopeReset ();
			}
		}
		else if(other.gameObject.tag=="target" && this.gameObject.tag=="Player")
		{
			Debug.Log ("Hit pass");
			if(DogHitSkipRope!=null)
			{
				DogHitSkipRope ();
			}
		}

	}
}
