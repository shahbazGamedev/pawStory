using UnityEngine;
using System.Collections;

public class HudMgr : MonoBehaviour 
{
	public GameObject Btn_01_Back;
	public GameObject Btn_01_Pause;
	public GameObject Btn_01_Resume;


	void Start () 
	{
	}


	void Update () 
	{
	}


	void OnEnable()
	{
		EventMgr.SceneLoaded += OnSceneLoaded;
		EventMgr.GamePause += OnGamePause;
		EventMgr.GameResume += OnGameResume;
	}


	void OnDisable()
	{
		EventMgr.SceneLoaded -= OnSceneLoaded;
		EventMgr.GamePause -= OnGamePause;
		EventMgr.GameResume -= OnGameResume;
	}


	public void OnButton_01()
	{
		string curScene = GameMgr.instance.GetCurScene ();
		if(curScene.Equals(GlobalConst.Scene_TournamentSelection))
		{
			GameMgr.instance.LoadScene(GlobalConst.Scene_MainMenu);
		}
		else if (curScene.Equals (GlobalConst.Scene_DiscDogs) ||
			curScene.Equals (GlobalConst.Scene_DockJump) ||
			curScene.Equals (GlobalConst.Scene_Obedience) ||
			curScene.Equals (GlobalConst.Scene_Tracking) ||
			curScene.Equals (GlobalConst.Scene_ColorLesson)) 
		{
			if(GameMgr.instance.IsGamePaused())
				EventMgr.OnGameResume();
			else
				EventMgr.OnGamePause();
		}
	}
	
	
	public void OnDogClick()
	{
	}


	public void OnSettings()
	{
	}
	
	
	public void OnShop()
	{
	}


	void OnLevelWasLoaded(int val)
	{

		string curScene = GameMgr.instance.GetCurScene ();
		OnSceneLoaded (curScene);
	}


	void OnSceneLoaded(string curScene)
	{
		if (curScene.Equals (GlobalConst.Scene_MainMenu)) {	
			Btn_01_Back.SetActive (true);
			Btn_01_Pause.SetActive (false);
			Btn_01_Resume.SetActive (false);
		} else if (curScene.Equals (GlobalConst.Scene_TournamentSelection)) {
			Btn_01_Back.SetActive (true);
			Btn_01_Pause.SetActive (false);
			Btn_01_Resume.SetActive (false);
		} else {
			Btn_01_Back.SetActive (false);
			Btn_01_Pause.SetActive (true);
			Btn_01_Resume.SetActive (false);
		}
	}


	void OnGamePause()
	{
		Btn_01_Back.SetActive (false);
		Btn_01_Pause.SetActive (false);
		Btn_01_Resume.SetActive (true);
	}


	void OnGameResume()
	{
		Btn_01_Back.SetActive (false);
		Btn_01_Pause.SetActive (true);
		Btn_01_Resume.SetActive (false);
	}


}

