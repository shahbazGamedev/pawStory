/**
Script Author : Srivatsan 
Description   : Color Lesson Dog Movements
**/
using UnityEngine;
using System.Collections;

public class DogMovementColorLesson : MonoBehaviour {
	public GameObject Dog;
	public GameObject Red;
	public GameObject Blue;
	public GameObject Green;
	public GameObject Yellow;
	public float distance;
	public Vector3 direction;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClick()
	{
		if(Vector3.Distance(Dog.transform.position,Red.transform.position)==0f)
		{

			direction = Red.transform.position - Dog.transform.position;
			distance = direction.magnitude;
		}
	}
}
