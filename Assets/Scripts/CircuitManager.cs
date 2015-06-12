using UnityEngine;
using System.Collections;

public class CircuitManager : MonoBehaviour {
	public float a;
	public float b;
	public float alpha;
	public float centerX;
	public float centerY;
	public float moveSpeed;

	public Vector3 focalPoint1;
	public Vector3 focalPoint2;

	public float forwardDist;
	public Vector3 forwardDirection;

	public Transform target;

	float x;
	float y;
	// Use this for initialization
	void Start () {
		target=this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {

		if(alpha>=360)
			alpha=0;
		alpha+=Time.deltaTime*moveSpeed;
		x = centerX + a * Mathf.Cos(alpha*0.0174f);
		y = centerY + b * Mathf.Sin(alpha*0.0174f);
		GetComponent<Rigidbody>().MovePosition(new Vector3(x,0.21f,y));

		float forwardAlpha=alpha+forwardDist;
		if(forwardAlpha>=360)
			forwardAlpha-=360;
		x = centerX + a * Mathf.Cos(forwardAlpha*0.0174f);
		y = centerY + b * Mathf.Sin(forwardAlpha*0.0174f);
		transform.LookAt(new Vector3(x, 0.21f, y));

	}

}
