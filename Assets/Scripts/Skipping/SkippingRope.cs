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
    private float angleDiff=236.2f;

	//Vector3 startPos;
	Quaternion startRot;

	// Use this for initialization
	void Start () {
        rope.transform.rotation = new Quaternion(0, 0, 90f,0f);

        Debug.Log("Angle : " + angle);

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
	public void RotateSkipRope()
	{
        /*
		angle -= Time.deltaTime * skipSpeed;
		if (angle < 0f)
            angle = 360f;
        rope.transform.rotation = Quaternion.Euler(angle, 0f, 90f);

        */
        if(angle < 0f)
            angle=360f;

        rope.transform.rotation = Quaternion.Euler(angle - angleDiff, 0f, 90f);
    }

    // Resets gameObject's position
    public void ResetPosition()
	{
		rope.transform.rotation = startRot;
		angle = rope.transform.rotation.eulerAngles.x;
	}
}
