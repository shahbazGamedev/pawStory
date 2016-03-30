    using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HudManager : MonoBehaviour 
{
	private static HudManager instance = null;

	public GameObject pauseScreen, gameOverScreen, gameWonScreen;

	void OnEnable()
	{
		EventManager.SceneStart += OnSceneStart;
		EventManager.SceneEnd += OnSceneEnd;
		EventManager.GamePaused += OnGamePaused;
		EventManager.GameResumed += OnGameResumed;
		EventManager.GameRestart += OnGameRestart;
	}


	void OnDisable()
	{
		 
		EventManager.SceneStart -= OnSceneStart;
		EventManager.SceneEnd -= OnSceneEnd;
		EventManager.GamePaused -= OnGamePaused;
		EventManager.GameResumed -= OnGameResumed;
		EventManager.GameRestart -= OnGameRestart;
	}

	public void Awake()
	{
		instance = this;
		/*if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != instance)
				Destroy(this.gameObject);
		}*/
	}
	// Use this for initialization
	void Start () 
	{
	
	}
	 
	void OnSceneStart()
	{
		
	}

	void OnSceneEnd()
	{
		pauseScreen.SetActive (false);
		gameOverScreen.SetActive (false);
	}

	void OnGamePaused()
	{
		pauseScreen.SetActive (true);
		gameOverScreen.SetActive (false);
	}

	void OnGameResumed()
	{
		pauseScreen.SetActive (false);
		gameOverScreen.SetActive (false);
	}


	void OnGameRestart()
	{
		pauseScreen.SetActive (false);
		gameOverScreen.SetActive (false);
	}

	public void OnPauseButtonClicked()
	{
		Debug.Log("Hudmanager on game pause");

		//gameWonScreen.SetActive (false);
		GameManager.Instance.GamePause(); 
		 
	}

	public void OnResumeButtonClicked()
	{
		
		//gameWonScreen.SetActive (false);
		GameManager.Instance.GameResume();
	}

	public void OnRestartButtonClicked()
	{
		
		//gameWonScreen.SetActive (false);
		GameManager.Instance.GameRestart();
	}

	public void OnMenuButtonClicked()
	{
		
		//gameWonScreen.SetActive (false);
		//GameManager.Instance.SceneEnd();
		Application.LoadLevel ("MainMenu");

		// load main menu
	}

	public void OnSFXButtonClicked()
	{
		
	}

	public void OnMusicButtonClicked()
	{
		
	}


	 
}
