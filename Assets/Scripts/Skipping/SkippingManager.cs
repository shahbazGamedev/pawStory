/**
Script Author : Vaikash 
Description   : Skipping Game Manager
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkippingManager : MonoBehaviour
{

	#region Variables

	public static SkippingManager instanceRef;
	public GameObject dogRef;
	public GameObject ropeRef;
	public GameObject[] livesRef;

	public int dogLives;
	public float scoreIncrement;
	public int comboMultiplierCap;
	public float sliderSpeed;

	bool gameStart;
	bool comboChain;
	bool lastHit;

	int comboCount;
	int maxCombo;
	float score; // Always int
	float tapCount;

	Animator dogAnimator;
	Rigidbody dogRigidBody;

	// UI components 
	public Slider slider;
	public Text scoreText;
	public Text comboText;
	public Text gameOverText;
	public GameObject gameOverPannel;

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
		dogLives = 3;
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
			tapCount += 1;
		}
	}


	void SliderFunction()
	{
		slider.value =  Mathf.PingPong(Time.time *sliderSpeed * 2,1);
	}

	// Disable life GUI object
	void ReduceLife(int index)
	{
		livesRef [index].SetActive (false);
	}

	// gameOver Function
	void GameOver()
	{
		gameOverPannel.SetActive (true);
		ropeRef.GetComponent <SkippingRope>().rotateRope=false;
		scoreText.gameObject.transform.parent.gameObject.SetActive (false);
		comboText.gameObject.SetActive (false);
		slider.gameObject.SetActive (false);
		gameOverText.text = "Score: " + score + "\n\n" + "Max Combo: " 
			+ maxCombo + "\n\nTotal Taps: " + tapCount; // Game Play Stats
	}

	#region Coroutines


	#endregion

	#region EventHandlers

	// Event Handler for DogHitSkipRope
	void DogHitSkipRopeEvent()
	{
		dogLives -= 1;
		if (dogLives >= 0) {
			ReduceLife (dogLives);
		} else
			dogLives = 0;
		comboChain = false;
		//comboCount = 1;
		lastHit = true;
		if(dogLives<=0)
		{
			GameOver ();
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
			maxCombo = comboCount >= maxCombo ? comboCount : maxCombo;
			comboCount = 1;
			comboText.text="";
		}
		scoreText.text = "Score: " + score;
	}

	#endregion

	#region BtnCallbacks

	// Reset Btn Callback
	public void ResetBtn()
	{
		
	}

	// Home Btn Callback
	public void HomeBtn()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}

	#endregion
}
