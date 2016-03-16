using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;    
 

public class GameManager : MonoBehaviour 
{
	private static GameManager instance = null;
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<GameManager>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}


	private bool gamePause =false;
	private string loadedLevel="";


	public string LoadedLevel
	{
		get
		{
			return this.loadedLevel;
		}

		set
		{
			this.loadedLevel = value;
		}
	}


	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != instance)
				Destroy(this.gameObject);
		}
	}

	void OnEnable()
	{
		App42Log.SetDebug(true);
		App42API.Initialize(GlobalVariables.APIKEY,GlobalVariables.SECRETKEY);
		Debug.Log ("App42 Init");

		EventManager.OnSceneStart ();
		 
	}


	void OnDisable()
	{
		 
	 
	}


	// Use this for initialization
	void Start () 
	{
		
		/*if(GlobalVariables.userID!=null)
		UserManager.instance.CheckForUser ();*/
	}
	

	public void LoadLevel(string levelName)
	{
		Application.LoadLevelAsync(levelName);
	}

	public void LoadLevel(int levelNumber)
	{
		Application.LoadLevelAsync(levelNumber);
	}
	 
	public void GamePause()
	{
		if(!gamePause)
		{
			gamePause = true;
			Debug.Log("Game is paused");
			EventManager.OnGamePaused();
			Time.timeScale = 0.0f;
			 
		}
	}

	public void GameResume()
	{
		if(gamePause)
		{
			gamePause =false;
			Debug.Log("Game resumed");
			EventManager.OnGameResumed();
			Time.timeScale = 1.0f;
		}
	}


	public void GameRestart()
	{
		Debug.Log("Game restarted");
		EventManager.OnGameRestart();
		Time.timeScale = 1.0f;
	}

	public void UpdateUI()
	{
		EventManager.OnUpdateUI();
	}

	public void SceneStart()
	{
		EventManager.OnSceneStart();
	}

	public void SceneEnd()
	{
		EventManager.OnSceneEnd();
	}



	// Update is called once per frame
	void Update () 
	{

	}

}
