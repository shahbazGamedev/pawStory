using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseRotation : MonoBehaviour {
	public GameObject dogRef;
	float angle;
	float preAngle;
	int value;
	Vector2 normalizedPositions;
	public bool gameStart;


	void Start () 
	{
	
	}
	

	void Update () 
	{
		if(gameStart)
		{
		normalizedPositions = new Vector2((Input.mousePosition.x/Screen.width-0.5f), ((Input.mousePosition.y/Screen.height)-0.5f));
		angle = Mathf.Atan2(normalizedPositions.y, normalizedPositions.x)*Mathf.Rad2Deg; 
		transform.eulerAngles = new Vector3( 0,0,angle);
		if(angle<0)
		{
			angle += 360;
			//Debug.Log (angle);
		}

		if(angle-preAngle>270)
		{
			value+=1;
			Debug.Log (value);
			dogRef.GetComponent<TugOfWarManager>().Movement();
		}
		else
		{
			dogRef.GetComponent<TugOfWarManager>().BackMovement();
		}
		preAngle=angle;
		}
	}
	}
