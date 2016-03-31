using UnityEngine;
using System.Collections;

public class ThrowingObjects : MonoBehaviour
{
	public static ThrowingObjects instRef;
	public float power;
	string layerName;
	Vector3 endPos;
	Vector3 force;
	Vector3 startPos;
	Vector3 direction;
    Rigidbody rb;
	public bool foul;


	void Awake()
	{
		instRef = this;
	}


	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}


	void OnEnable()
	{
		EventMgr.GameRestart += OnReset;
	}


	void OnDisable()
	{
		EventMgr.GameRestart -= OnReset;
	}
		

	void  OnMouseDown ()
	{
		rb.isKinematic = false;
		startPos = Input.mousePosition;
		startPos.z = transform.position.z - Camera.main.transform.position.z;
		startPos = Camera.main.ScreenToWorldPoint(startPos);
	}


	void OnReset()
	{
		transform.position = startPos;
		rb.isKinematic = true;
	}


	void  OnMouseUp ()
	{
		if (ColorTrainingMgr.instRef.isMoving == false && ColorTrainingMgr.instRef.canThrow) 
		{
			endPos = Input.mousePosition;
			endPos.z = transform.position.z - Camera.main.transform.position.z;
			endPos = Camera.main.ScreenToWorldPoint (endPos);

			force = endPos - startPos;
			force.z = force.magnitude;
			force.Normalize ();

			rb.AddForce (force * power);
		}

		if (endPos.z > startPos.z)
		{
			ColorTrainingMgr.instRef.isMoving = true;
		}
		else if(ColorTrainingMgr.instRef.canThrow == true)
		{
			ColorTrainingMgr.instRef.falseTap = true;
		}

		//ColorTrainingMgr.instRef.colorPanelUI.SetActive (false);
	}


	void OnCollisionEnter(Collision col)
	{
		layerName = LayerMask.LayerToName (col.gameObject.layer);
		switch (layerName) 
		{
		case "Floor":
            ColorTrainingMgr.instRef.currentObjectToCollect = this.gameObject;
            ColorTrainingMgr.instRef.targetPos = this.transform.position; 
			ColorTrainingMgr.instRef.colorPanelUI.SetActive (false);
			rb.velocity = Vector3.zero;
			break;
		}
	}
}
