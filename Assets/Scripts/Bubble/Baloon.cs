using UnityEngine;
using System.Collections;

public class Baloon : MonoBehaviour {
	public GameObject floor;
	public float maxDistance;
	public float distance;
	public GameObject food;
	public Transform target;

	public float speed;

	GlobalValues gValues;

	// Use this for initialization
	void Start () {
		maxDistance = 20;
		floor = GameObject.FindGameObjectWithTag("floor");
		gValues = GlobalValues.instanceRef;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		distance = Vector3.Distance (this.gameObject.transform.position, floor.transform.position);
		if(distance>maxDistance)
		{
			gValues.baloonsAtScene -= 1;
			Destroy (this.gameObject);
		}
	}
	void OnMouseDown()
	{
		Instantiate(food,new Vector3(0,10,0),Quaternion.identity);
		//float step = speed * Time.deltaTime;
		//transform.position = Vector3.MoveTowards(transform.position, target.position, step);
	}
//	void OnBecameInvisible()
//	{
//		//Debug.LogError ("I m invincible");
//		GManager gameManager = GManager.instanceRef;
//		gameManager.baloonsAtScene -= 1;
//		Destroy (this.gameObject);
//
//	}
}
