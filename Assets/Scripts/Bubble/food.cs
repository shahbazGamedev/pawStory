using UnityEngine;
using System.Collections;

public class food : MonoBehaviour {

   
	DogMovementBubble dogMove;


	// Use this for initialization
	void Start () 
	{

		dogMove=FindObjectOfType<DogMovementBubble>();
	  
	}


	// Update is called once per frame
	void Update ()
	{
	
	}


	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag=="Player")
		{

		//	Debug.Log ("bool working");
			dogMove.ScoreSystem();
			dogMove.isCollect=false;
			Destroy(this.gameObject);
		}
		else
		{
			Debug.Log("yes");
			dogMove.isCollect=true;
			dogMove.target=transform;
		}
}
}