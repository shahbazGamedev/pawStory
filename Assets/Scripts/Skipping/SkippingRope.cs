/**
Script Author : Vaikash 
Description   : Skipping Rope Animator
**/

using UnityEngine;
using System.Collections;

public class SkippingRope : MonoBehaviour {
	
	public bool rotateRope;
	public float angle;
	public float skipSpeed;

	//Vector3 startPos;
	Quaternion startRot;

	// Use this for initialization
	void Start () {
		angle = transform.rotation.eulerAngles.x;
		startRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(rotateRope)
		{
			RotateSkipRope ();
		}
	}

	// Update Angle
	void RotateSkipRope()
	{
		angle += Time.deltaTime * skipSpeed;
		if (angle > 360)
			angle = 1f;
		transform.rotation=Quaternion.Euler (angle, 180f, 180f);
	}

	// Resets gameObject's position
	public void ResetPosition()
	{
		transform.rotation = startRot;
		angle = transform.rotation.eulerAngles.x;
	}
}
