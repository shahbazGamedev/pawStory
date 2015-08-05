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
	void OnCollsionEnter(Collider other)
	{
		if(other.gameObject.tag=="Player")
		{
			if(objectID==collisionTypes.hurdleJump || objectID==collisionTypes.hurdleSlide)
			{
				// Reduce dog movement speed
				Debug.Log ("Hit hurdle");
			}
			else if(objectID==collisionTypes.powerTurbo)
			{
				// Dog movement speed boost
				Debug.Log ("Turbo Mode");
			}
			else if(objectID==collisionTypes.powerSlow)
			{
				// Slow game speed
				Debug.Log ("Bullet Time");
			}
		}
	}
}
