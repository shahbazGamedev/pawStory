using UnityEngine;
using System.Collections;

public class PowerUpRotate : MonoBehaviour {

	public float speed;
	Vector3 rotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rotation = transform.rotation.eulerAngles;
		rotation += (Time.deltaTime * speed) * new Vector3 (0f, 1f, 0f);
		transform.rotation = Quaternion.Euler (rotation);
	}
}
