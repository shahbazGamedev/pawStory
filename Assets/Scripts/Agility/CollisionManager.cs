/**
Script Author : Vaikash 
Description   : Takes care of collisions in Agility
**/

using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

	public collisionTypes objectID;

	public enum collisionTypes
	{
		hurdleJump,
		hurdleSlide,
		powerTurbo,
		powerSlow
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Manage Collision
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag=="Player")
		{
			if(objectID==collisionTypes.hurdleJump || objectID==collisionTypes.hurdleSlide)
			{
				// Reduce dog movement speed
				Debug.Log ("Hit hurdle");
				Destroy (gameObject);
			}
			else if(objectID==collisionTypes.powerTurbo)
			{
				// Dog movement speed boost
				Debug.Log ("Turbo Mode");
				other.gameObject.GetComponent <EllipseMovement>().RunCoroutine (33);
				Destroy (gameObject);
			}
			else if(objectID==collisionTypes.powerSlow)
			{
				// Slow game speed
				Debug.Log ("Bullet Time");
				other.gameObject.GetComponent <EllipseMovement>().RunCoroutine (11);
				Destroy (gameObject);
			}
		}
	}
}
