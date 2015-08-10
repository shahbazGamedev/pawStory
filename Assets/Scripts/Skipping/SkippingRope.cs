using UnityEngine;
using System.Collections;

public class SkippingRope : MonoBehaviour {

	public float angle;
	public float skipSpeed;

	// Use this for initialization
	void Start () {
		angle = transform.rotation.eulerAngles.x;
	}
	
	// Update is called once per frame
	void Update () {
		angle += Time.deltaTime * skipSpeed;
		transform.rotation=Quaternion.Euler (angle, 0f, 0f);
	}

}
