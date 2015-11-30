/**
Script Author : Vaikash 
Description   : Destroy on hit and to calculate score
**/
using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour {
	BasketSpawner spawnerRef;

	// Use this for initialization
	void Start () {
		spawnerRef = FindObjectOfType <BasketSpawner> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision other)
	{
		// Destroy Basket on Collision
		if(other.gameObject.tag=="Ball")
		{
//			Debug.Log ("Yay");
			spawnerRef.score += 1;
			Destroy (this.gameObject);
		}
	}
}
