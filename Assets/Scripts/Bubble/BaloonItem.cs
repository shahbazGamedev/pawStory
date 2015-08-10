using UnityEngine;
using System.Collections;

public class BaloonItem : MonoBehaviour {

	int itemType = 0;

	void Start () 
	{	
	}
	

	void Update () 
	{	
	}


	void OnCollisionEnter(Collision collision)	{
		if (collision.contacts.Length > 1 && itemType == 0) {
			ContactPoint contact = collision.contacts[0];
			//Debug.DrawRay(contact.point, contact.normal * 10, Color.yellow, 5f);
			EventMgr.OnSetPos(contact.point);
		}
	}


}


