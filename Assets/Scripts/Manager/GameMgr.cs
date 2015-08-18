using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour 
{
	private static GameMgr instance = null;
	public static GameMgr Inst
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<GameMgr>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

	// Scene Managements
	string prevScene = GlobalConst.Scene_LoadingScene;
	string curScene = GlobalConst.Scene_LoadingScene;

	bool isPaused = false;

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
		EventMgr.SceneLoaded += OnSceneLoaded;
		EventMgr.GamePause += OnGamePause;
		EventMgr.GameResume += OnGameResume;
		EventMgr.GameRestart += OnRestartGame;
	}
	
	
	void OnDisable()
	{
		EventMgr.SceneLoaded += OnSceneLoaded;
		EventMgr.GamePause -= OnGamePause;
		EventMgr.GameResume -= OnGameResume;
		EventMgr.GameRestart -= OnRestartGame;
	}


	void Start ()
	{
	}


	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if(isPaused)
			{
				EventMgr.OnGameResume();
				isPaused = false;
			}
			else
			{
				EventMgr.OnGamePause();
				isPaused = true;
			}
		}
	}


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            EventMgr.OnGamePause();
            isPaused = true;
        }        
    }

    
    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            EventMgr.OnGamePause();
            isPaused = true;
        } 
    }



	void Init()
	{
	}
	
	#region SceneManagement
	public void LoadScene(string newScene)
	{
		if(isPaused)
			OnGameResume ();
		prevScene = curScene;
		curScene = newScene;
		Application.LoadLevel(newScene);
	}

	// waitTime : in seconds
	public void LoadScene(string newScene, float waitTime)
	{
		StartCoroutine (LoadSceneAfter(newScene, waitTime));
	}

	// waitTime : in seconds
	IEnumerator LoadSceneAfter(string newScene, float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		if(isPaused)
			OnGameResume ();
		prevScene = curScene;
		curScene = newScene;
		Application.LoadLevel(newScene);
	}


	public string GetCurScene()
	{
		return curScene;
	}


	void OnSceneLoaded(string newScene)
	{
		if (!curScene.Equals (newScene)) 
		{
			prevScene = curScene;
			curScene = newScene;
		}
	}
	#endregion SceneManagement


	#region GameHandles
	public bool IsGamePaused()
	{
		return isPaused;
	}


	void OnGamePause()
	{
		isPaused = true;
		Time.timeScale = 0;
	}


	void OnGameResume()
	{
		isPaused = false;
		Time.timeScale = 1;
	}


	void OnRestartGame()
	{
		if(isPaused)
			OnGameResume ();
	}
	#endregion GameHandles
}

