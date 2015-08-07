/**
Script Author : Vaikash 
Description   : Takes care of collisions in Agility
**/

using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

	public collisionTypes objectID;
	public int triggerID; // starts from 1
	public static int previousID;

	public enum collisionTypes
	{
		hurdleJump,
		hurdleSlide,
		powerTurbo,
		powerSlow,
		waypoint
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Manage Collision
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			if(objectID==collisionTypes.waypoint)
			{
				if(triggerID!=previousID)
					other.gameObject.GetComponent <EllipseMovement> ().FireDogMovedNextPartition ();
				previousID = triggerID;
				return;
			}

			if(objectID==collisionTypes.hurdleJump || objectID==collisionTypes.hurdleSlide)
			{
				// Reduce dog movement speed
				Debug.Log ("Hit hurdle");
				other.gameObject.GetComponent <EllipseMovement>().RunCoroutine (20);
				AgilityManager.instanceRef.hurdlesCollided += 1;
				Destroy (gameObject.transform.parent.gameObject);
			}
			else if(objectID==collisionTypes.powerTurbo)
			{
				// Dog movement speed boost
				Debug.Log ("Turbo Mode");
				other.gameObject.GetComponent <EllipseMovement>().RunCoroutine (28);
				Destroy (gameObject.transform.parent.gameObject);
			}
			else if(objectID==collisionTypes.powerSlow)
			{
				// Slow game speed
				Debug.Log ("Bullet Time");
				other.gameObject.GetComponent <EllipseMovement>().RunCoroutine (18);
				Destroy (gameObject.transform.parent.gameObject);
			}
		}
	}
}
