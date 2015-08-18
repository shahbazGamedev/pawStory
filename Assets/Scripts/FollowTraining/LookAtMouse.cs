using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

public class LookAtMouse : MonoBehaviour
{
	public float speed;
	public GameObject headRef;

	void FixedUpdate () 
	{
		Vector3 upAxis = new Vector3(0,0,-1);
	    Vector3 mouseScreenPosition = Input.mousePosition;
		mouseScreenPosition.z = transform.position.z;
		Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
		headRef.transform.LookAt(mouseWorldSpace, upAxis);
		headRef.transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);
	}
	}
