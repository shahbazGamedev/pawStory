using UnityEngine;
using System.Collections;

public class Baloon : MonoBehaviour {
	public GameObject BaloonItem;
	public int ID;
	public float Speed;
	public float Distance;
	public bool isAlive = false;

	
	void Start () 
	{
		Speed = 3;
		Distance = 10;
	}


	void FixedUpdate ()
	{
		if (isAlive) {
			Vector3 distanceTravelled = new Vector3 (0, 1, 0) * Speed * Time.deltaTime;
			transform.position += distanceTravelled;
			Distance -= distanceTravelled.magnitude;
			if(Distance < 0)
			{
				isAlive = false;
			}
		}
	}


	void OnMouseDown()
	{
		gameObject.SetActive (false);
		if (ID == 0 || ID == 1) {
			Rigidbody rb = BaloonItem.GetComponent<Rigidbody> () as Rigidbody;
			rb.useGravity = true;
			BaloonItem.SetActive (true);
			BaloonItem.transform.parent = transform.parent;
			isAlive = false;
		}
	}


	public void SetBaloonData(GameObject newBaloon, int newID, bool newState)
	{
		BaloonItem = newBaloon;
		ID = newID;
		isAlive = newState;
		newBaloon.transform.parent = transform;
		newBaloon.transform.position = transform.position;
		BaloonItem.SetActive (false);
	}

}
