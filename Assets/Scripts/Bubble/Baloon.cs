using UnityEngine;
using System.Collections;

public class Baloon : MonoBehaviour {
	public GameObject floor;
	public float maxDistance;
	public float distance;
	public GameObject food;
	public Transform target;
	public Transform startingPos;
	public float speed;
	public float smooth;


	GlobalValues gValues;

	// Use this for initialization
	void Start () {
		smooth=10f;
		maxDistance = 10;
		floor = GameObject.FindGameObjectWithTag("floor");
		gValues = GlobalValues.instanceRef;

	
	}
	
	// Update is called once per frame
	void FixedUpdate () {


			MoveMent();


	distance = Vector3.Distance (this.gameObject.transform.position, floor.transform.position);
		if(distance>maxDistance)
		{
			gValues.baloonsAtScene -= 1;
			Destroy (this.gameObject);
		}
	}
	void OnMouseDown()
	{
		Instantiate(food,transform.position,Quaternion.identity);
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
	void MoveMent()
	{
		float t = 0.0f;
		t+=Time.deltaTime;
		Vector3 startingPos = transform.position;
		transform.position = Vector3.MoveTowards(startingPos, target.position,t*smooth);

	}



}
