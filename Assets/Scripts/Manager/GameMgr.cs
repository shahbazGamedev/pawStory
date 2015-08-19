using UnityEngine;
using System.Collections;

public enum GameStates
{ 
    None,
    Menu,
    Pause,
    Play,
    GameWon,
    GameLost,
    Tutorial,
    Store,
};

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

    GameStates prevGameState = GameStates.Play;
    GameStates curGameState = GameStates.Play;

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
#if !UNITY_EDITOR
        if (focusStatus)
        {
            EventMgr.OnGamePause();
            isPaused = true;
        } 
#endif
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
        ChangeState(GameStates.Pause);
	}


	void OnGameResume()
	{
		isPaused = false;
		Time.timeScale = 1;
        ChangeState(GameStates.Play);
    }


	void OnRestartGame()
	{
		if(isPaused)
			OnGameResume ();
	}
	#endregion GameHandles


    #region GameState
    void ChangeState(GameStates newGameState)
    {
        prevGameState = curGameState;
        curGameState = newGameState;
    }


    GameStates GetGameState()
    {
        return curGameState;
    }

    #endregion GameState

}

