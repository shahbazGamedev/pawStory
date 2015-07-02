/**
Script Author : Vaikash 
Description   : Destroy on hit and to calculate score
**/
using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other)
	{
		// Destroy Basket on Collision
		if(other.gameObject.tag=="Ball")
		{
			Debug.Log ("Yay");
			Destroy (this.gameObject);
		}
	}
}
