using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HudManager : MonoBehaviour 
{


	void OnEnable()
	{
		  
	}


	void OnDisable()
	{
		 

	}

	// Use this for initialization
	void Start () 
	{
	
	}
	 

	public void OnPauseButtonClicked()
	{
		Debug.Log("Hudmanager on game pause");
		GameManager.Instance.GamePause(); 
		 
	}

	public void OnResumeButtonClicked()
	{
		GameManager.Instance.GameResume();
	}

	public void OnRestartButtonClicked()
	{
		GameManager.Instance.GameRestart();
	}

	public void OnMenuButtonClicked()
	{
		GameManager.Instance.SceneEnd();
		// load main menu
	}

	public void OnSFXButtonClicked()
	{
		
	}

	public void OnMusicButtonClicked()
	{
		
	}


	 
}
