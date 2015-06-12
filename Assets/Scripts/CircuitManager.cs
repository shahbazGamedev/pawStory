using UnityEngine;
using System.Collections;

public class CircuitManager : MonoBehaviour {
	public float a;
	public float b;
	public float alpha;
	public float cx;
	public float cy;
	public float moveSpeed;
	Vector3 forwardDirection;
	public float forwardDist;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(alpha>=360)
			alpha=0;
		alpha+=Time.deltaTime*moveSpeed;
		float x = cx + a * Mathf.Cos(alpha/6.28f);
		float y = cy + b * Mathf.Sin(alpha/6.28f);
		GetComponent<Rigidbody>().MovePosition(new Vector3(x,0.21f,y));
		float forwardaAlpha=alpha+forwardDist;
		if(forwardaAlpha>=360)
			forwardaAlpha-=360;
		x = cx + a * Mathf.Cos(forwardaAlpha/6.28f);
		y = cy + b * Mathf.Sin(forwardaAlpha/6.28f);
		forwardDirection=new Vector3(x,0.21f,y);
		transform.rotation= Quaternion.LookRotation(forwardDirection);

	}
}
