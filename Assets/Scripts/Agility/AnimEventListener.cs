using UnityEngine;
using System.Collections;

public class AnimEventListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SlideAnimComplete()
	{
		AgilityManager.instanceRef.ResetColliderSize ();
	}
}
