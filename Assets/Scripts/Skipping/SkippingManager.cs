using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkippingManager : MonoBehaviour
{

	#region Variables

	public static SkippingManager instanceRef;
	public GameObject dogRef;
	public GameObject[] livesRef;

	public int dogLives;
	public float scoreIncrement;
	public int comboMultiplierCap;
	public float sliderSpeed;

	bool gameStart;
	bool comboChain;
	bool lastHit;

	int comboCount;
	float score; // Always int

	Animator dogAnimator;
	Rigidbody dogRigidBody;

	// UI components 
	public Slider slider;
	public Text scoreText;
	public Text comboText;

	#endregion

	void Awake()
	{
		dogAnimator =  dogRef.GetComponent<Animator>();
		dogRigidBody = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () 
	{
		instanceRef = this;
		gameStart = true;
		comboCount = 1;

		scoreText.text = "Score: " + score;

		// Add Event Listener
		TriggerDetection.DogHitSkipRope += DogHitSkipRopeEvent;
		TriggerDetection.SkipRopeReset += SkipRopeResetEvent;
	}

	// Update is called once per frame
	void Update () 
	{
		SliderFunction();
		PlaySkipping();
	}

	// Decouple Event Listeners
	void OnDisable()
	{
		TriggerDetection.DogHitSkipRope -= DogHitSkipRopeEvent;
		TriggerDetection.SkipRopeReset -= SkipRopeResetEvent;
	}

	// Listens for player input // TODO
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

	#region EventHandlers

	// Event Handler for DogHitSkipRope
	void DogHitSkipRopeEvent()
	{
		dogLives -= 1;
		comboChain = false;
		comboCount = 1;
		lastHit = true;
		if(dogLives<0)
		{
			// GameOver
		}
	}

	// Event Handler for SkipRopeReset
	void SkipRopeResetEvent()
	{
		if (!lastHit) 
		{
			score += scoreIncrement * comboCount > comboMultiplierCap ? 10 : comboCount;
			if (comboChain) 
			{
				comboCount += 1;
				comboText.text=comboCount+"X";
			}
			else
			{
				comboChain = true;
			}
		}
		else
		{
			lastHit = false;
			comboCount = 1;
			comboText.text="";
		}
		scoreText.text = "Score: " + score;
	}

	#endregion
}
