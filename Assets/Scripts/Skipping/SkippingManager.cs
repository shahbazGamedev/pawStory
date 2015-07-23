using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkippingManager : MonoBehaviour
{

	public GameObject dogRef;
	
	private Animator dogAnimator;
	private Rigidbody dogRigidBody;
	
	// UI components 

	public Slider slider;
	public float sliderSpeed;

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


	void SliderFunction()
	{
		slider.value =  Mathf.PingPong(Time.time *sliderSpeed * 2,1);
	}


	// Update is called once per frame
	void Update () 
	{
		SliderFunction();
		PlaySkipping();
	}
}
