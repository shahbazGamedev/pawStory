using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseRotation : MonoBehaviour {
	float angle;
	Vector2 normalizedPositions;
	public bool isTouched;
	float preAngle;
	float delta;
	float CurrnentAngle;
	float totalAngle;

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

		if(angle<0)
		{
			angle += 360;
			Debug.Log (angle);
		}
		CurrnentAngle=angle;
		delta=CurrnentAngle-angle;
		if(delta>300)
		{
			dogRef.GetComponent<DogMovementTugOfWar>().Movement();
		}


//		}
//		else
//		{
//		    transform.eulerAngles = new Vector3( 0,0,angle);
//			dogRef.GetComponent<DogMovementTugOfWar>().BackMovement();
//		}


	}


}
