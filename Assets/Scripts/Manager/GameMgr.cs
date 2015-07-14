using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour {

	private static GameMgr _instance = null;
	public static GameMgr instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameMgr>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	// Scene Managements
	public static string Scene_LoadingScene = "LoadingScene";
	public static string Scene_MainMenu = "MainMenu";
	public static string Scene_TournamentSelection = "TournamentSelection";

	void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}


	void Init()
	{
	}


	void Start ()
	{
	}


	void Update ()
	{
	}


	public void LoadScene(string newScene)
	{
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
		Application.LoadLevel(newScene);
	}
}
