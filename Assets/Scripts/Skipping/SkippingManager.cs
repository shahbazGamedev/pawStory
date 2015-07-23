using UnityEngine;
using System.Collections;

public class SkippingManager : MonoBehaviour
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

	void PlaySkipping()
	{
		if(Input.GetMouseButtonDown(0))
		{
			dogAnimator.SetTrigger("Skip");
			 
		}
	}

	// Update is called once per frame
	void Update () 
	{
		PlaySkipping();
	}
}
