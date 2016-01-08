using UnityEngine;
using System.Collections;

// 

public class PuppyManager : MonoBehaviour 
{


	public static PuppyManager instance = null;


	 

	void Awake()
	{
		instance = this;
	}

	void OnEnable()
	{
		Debug.Log("Events are registered :- PuppyManager");
		 
		EventManager.GamePaused += OnGamePaused;
		 
	}


	void OnDisable()
	{
		Debug.Log("Events are deregistered :- PuppyManager");

		EventManager.GamePaused -= OnGamePaused;
		 
	}

	// Use this for initialization
	void Start () 
	{

	}


	 
	void OnGamePaused()
	{
		Debug.Log("game is paused and values are updated in puppymanager");
	}

	 
	
	 
}
