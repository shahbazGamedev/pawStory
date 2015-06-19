using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{


	public static GameManager gameManager = null;

	public bool isPaused 
	{
		get 
		{
			return isPaused;
		}
		set
		{
			if(isPaused)
			{
				Time.timeScale = 0.0f;
				isPaused = true;
			}
			else
			{
				Time.timeScale =1.0f;
				isPaused = false;
			}
		}
	}

	void Awake()
	{
		gameManager = this;
		DontDestroyOnLoad(this);
	}



	// Use this for initialization
	void Start () 
	{
	
	}


	 
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
