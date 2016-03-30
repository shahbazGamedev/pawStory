/**
Script Author : harish
Description   : Running Training
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunningTrainingNew : MonoBehaviour 

{
	// Variables
	public GameObject panelGameOver;
	public GameObject panelGameScreen;
	public GameObject touchMat;
	public Text textTimer;
	public Text textGameOver;

	Animator dogAnim;
	public Slider runSlider;
	public Renderer threadmill;

	public float startingValue;
	public float inputValue;
	public float decrementValue;
	public float maxLevelTime;

	float dogSpeed;
	float threadMillSpeed;
	float levelTimer;

	bool gameStart;

	void Start () 
	{
		dogSpeed = startingValue;
		runSlider.value = startingValue;
		gameStart = true;
		panelGameOver.SetActive(false);
		dogAnim = GetComponent<Animator> ();
		touchMat.SetActive (true);
	}

	void Update () 
	{
		GameOver ();
		levelTimer += Time.deltaTime;
		textTimer.text = " Time : " + (int)levelTimer;
		DogMovement ();
	
	}

	void OnEnable() 
	{
        EventManager.GameRestart += OnRestartGame;
		EventManager.GamePaused += OnPauseGame;
	    EventManager.GameResumed += OnGameResume;
	}
		
	void OnDisable() 
	{
        EventManager.GameRestart -= OnRestartGame;
        EventManager.GamePaused -= OnPauseGame;
        EventManager.GameResumed -= OnGameResume;
    }

	// Even Trigger
	public void OnPointerDown()
	{
		dogSpeed += inputValue;
		runSlider.value += inputValue;	
	}

	void DogMovement()
	{
		if (gameStart == true) 
		{
			dogAnim.SetFloat("Walk",dogSpeed);

			float offset = Time.time * threadMillSpeed;
			threadmill.material.SetTextureOffset ("_MainTex", new Vector2 (0,offset));
	
			dogSpeed -= decrementValue*Time.deltaTime;
			runSlider.value -= decrementValue*Time.deltaTime;

			if (dogSpeed > 0f)
			{
				threadMillSpeed = 0.5f;
			} 
			if (dogSpeed > 0.3f)
			{
				threadMillSpeed = 1f;
			}
			if (dogSpeed > 0.6f)
			{
				threadMillSpeed = 1.5f;
			}
		}
	}

	public void OnRestartGame()
	{
		Time.timeScale = 1;
		touchMat.SetActive (true);
		levelTimer = 0f;
		dogSpeed = startingValue;
		runSlider.value = startingValue;
		gameStart = true;
		dogAnim.SetFloat ("Walk",dogSpeed);
		panelGameOver.SetActive(false);
		panelGameScreen.SetActive (true);
	}

	public void OnPauseGame()
	{
		Time.timeScale = 0;
		touchMat.SetActive (false);
	}

	public void OnGameResume()
	{
		touchMat.SetActive (true);
        Time.timeScale = 1f;
	}
		

	public void MainMenu()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}

	void GameOver()
	{
		if (dogSpeed < 0 || levelTimer >= maxLevelTime) 
		{
			panelGameOver.SetActive(true);
			textGameOver.text = "Training Sesson Failed!!!";
			panelGameScreen.SetActive(false);
			Time.timeScale = 0;
		}

		if (dogSpeed >= 1)
		{
			panelGameOver.SetActive(true);
			textGameOver.text = "Training Sesson Sucessful!!!";
			Time.timeScale = 0;
			panelGameScreen.SetActive(false);
		}
	}	 
}
