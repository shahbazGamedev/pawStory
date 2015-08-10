using UnityEngine;
using System.Collections;

public class TriggerDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="target")
		{
			Debug.Log ("Hit pass");
		}
	}
}
