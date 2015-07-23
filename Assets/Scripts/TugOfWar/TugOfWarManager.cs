using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TugOfWarManager : MonoBehaviour 
{
	public GameObject dogRef;

	private Animator dogAnimator;
	private Rigidbody dogRigidBody;


	void Awake()
	{
		dogAnimator =  dogRef.GetComponent<Animator>();
		dogRigidBody = this.GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () 
	{
	
	}

	public void TrackMouseMovement(BaseEventData data)
	{
		 
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log(pointerData);
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
}
