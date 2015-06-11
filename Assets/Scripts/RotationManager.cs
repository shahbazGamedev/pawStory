using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class RotationManager : MonoBehaviour {
	Vector3 targetRotation;

	float horizontal;
	float vertical;

	public Vector3 forward;
	public Vector3 backward;
	public Vector3 left;
	public Vector3 right;
//
//	float verticalLimit;
//	float horizontalLimit;

	public float rotationRate;

	// Use this for initialization
	void Start () {
		forward=new Vector3(0f,0f,0f);
		backward=new Vector3(0f,180f,0f);
		left=new Vector3(0f,-90f,0f);
		right=new Vector3(0f,90f,0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		vertical=CrossPlatformInputManager.GetAxis("Vertical");
		horizontal=CrossPlatformInputManager.GetAxis("Horizontal");
		if(horizontal<0)
		{
			targetRotation=left;
		}
		if(horizontal>0)
		{
			targetRotation=right;
		}
		if(vertical>0.3&&(horizontal<0.5||horizontal>-0.5))
		{
			targetRotation=forward;
		}
		if(vertical<-0.3&&(horizontal<0.5||horizontal>-0.5))
		{
			targetRotation=backward;
		}

	}

	void FixedUpdate()
	{

		if(this.transform.rotation!=Quaternion.Euler(targetRotation))
		{
			this.transform.rotation=Quaternion.Slerp(this.transform.rotation,Quaternion.Euler(targetRotation),rotationRate);
		}
	}
}
