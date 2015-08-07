using UnityEngine;
using System.Collections;

public class Baloon : MonoBehaviour {
	public GameObject Food;
	public int ID;
	public int FoodType;
	float LifeTime;
	float Speed;
	float curTime;

	
	void Start () 
	{
		LifeTime = 4;
		Speed = 3;
	}


	void FixedUpdate ()
	{
		curTime += Time.deltaTime;
		if(curTime > LifeTime)
		{
			Destroy (this.gameObject);
		}
		else
		{
			transform.position += new Vector3(0, 1, 0) * Speed * Time.deltaTime;
		}
	}


	void OnMouseDown()
	{
		if(ID == 3)
			Instantiate(Food, transform.position, Quaternion.identity);
	}


}
