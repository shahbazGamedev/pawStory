using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseRotation : MonoBehaviour {
	float angle;
	Vector2 normalizedPositions;
	public bool isTouched;


	public GameObject dogRef;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () 
	{


	    normalizedPositions = new Vector2((Input.mousePosition.x/Screen.width-0.5f), ((Input.mousePosition.y/Screen.height)-0.5f));
		angle = Mathf.Atan2(normalizedPositions.y, normalizedPositions.x)*Mathf.Rad2Deg; 
		transform.eulerAngles = new Vector3( 0,0,angle);
		Debug.Log (angle);
		if(angle<=50)
		{
			dogRef.GetComponent<DogMovementTugOfWar>().Movement();
		}
			else
			dogRef.GetComponent<DogMovementTugOfWar>().BackMovement();
//		}
//		else
//		{
//		    transform.eulerAngles = new Vector3( 0,0,angle);
//			dogRef.GetComponent<DogMovementTugOfWar>().BackMovement();
//		}


	}


}
