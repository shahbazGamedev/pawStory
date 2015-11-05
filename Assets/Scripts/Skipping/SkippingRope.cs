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
    public GameObject rope;

	//Vector3 startPos;
	Quaternion startRot;

	// Use this for initialization
	void Start () {
		angle = rope.transform.rotation.eulerAngles.x;
		startRot = rope.transform.rotation;
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
		angle -= Time.deltaTime * skipSpeed;
		if (angle < 0f)
            angle = 360f;
        rope.transform.rotation = Quaternion.Euler(angle, 0f, 90f);
	}

	// Resets gameObject's position
	public void ResetPosition()
	{
		rope.transform.rotation = startRot;
		angle = rope.transform.rotation.eulerAngles.x;
	}
}
