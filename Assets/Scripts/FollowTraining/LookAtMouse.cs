using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

public class LookAtMouse : MonoBehaviour

{
	public float speed;
	public GameObject headRef;
	public GameObject dog;

	void FixedUpdate () 
	{
		Vector3 upAxis = new Vector3(0,0,-1);
	    Vector3 mouseScreenPosition = Input.mousePosition;
		mouseScreenPosition.z = transform.position.z;
		Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
		transform.LookAt(mouseWorldSpace, upAxis);
		Debug.Log(mouseWorldSpace);
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);
	    if(mouseWorldSpace.y<0)
		{
			mouseWorldSpace.y=0;
		}
		}
	}