using UnityEngine;
using System.Collections;

public class HudMgr : MonoBehaviour 
{
	public GameObject Btn_01_Home;
	public GameObject Btn_01_Back;
	public GameObject Btn_01_Pause;
	public GameObject Btn_01_Resume;

	public GameObject BottomPanel;
	public GameObject Shop_Menu;
	public GameObject Pause_Menu;
    public GameObject Stats_Menu;

	void Start () 
	{
		BottomPanel.SetActive (false);
		Shop_Menu.SetActive (false);
		Pause_Menu.SetActive (false);
        Stats_Menu.SetActive(false);
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


	void OnLevelWasLoaded(int val)
	{
		string curScene = GameMgr.Inst.GetCurScene ();
		OnSceneLoaded (curScene);
	}


	#region TopPanel
	public void OnButton_01()
	{
		string curScene = GameMgr.Inst.GetCurScene ();
		if(curScene.Equals(GlobalConst.Scene_TournamentSelection))
		{
			GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
		}
		else if (curScene.Equals (GlobalConst.Scene_DiscDogs) ||
		         curScene.Equals (GlobalConst.Scene_DockJump) ||
		         curScene.Equals (GlobalConst.Scene_Tracking) ||
		         curScene.Equals (GlobalConst.Scene_Obedience) ||
		         curScene.Equals (GlobalConst.Scene_ColorLesson) ||
                 curScene.Equals (GlobalConst.Scene_Skipping) ||
                 curScene.Equals (GlobalConst.Scene_Tugofwar) ||
                 curScene.Equals (GlobalConst.Scene_Agility) ||
		         curScene.Equals (GlobalConst.Scene_Bubble)) 
		{
			if(GameMgr.Inst.IsGamePaused())
				EventMgr.OnGameResume();
			else
				EventMgr.OnGamePause();
		}
	}
	
	
	public void OnDogClick()
	{
        OnOpenStatsMenu();
	}


	public void OnSettings()
	{
	}
	
	
	public void OnShop()
	{
		OnOpenShopMenu ();
	}
	#endregion TopPanel


	#region EventHandling
	void OnSceneLoaded(string curScene)
	{
		if (curScene.Equals (GlobalConst.Scene_MainMenu)) {	
			Btn_01_Home.SetActive (true);
			Btn_01_Back.SetActive (false);
			Btn_01_Pause.SetActive (false);
			Btn_01_Resume.SetActive (false);
		} else if (curScene.Equals (GlobalConst.Scene_TournamentSelection)) {
			Btn_01_Home.SetActive (false);
			Btn_01_Back.SetActive (true);
			Btn_01_Pause.SetActive (false);
			Btn_01_Resume.SetActive (false);
		} else {
			Btn_01_Home.SetActive (false);
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
		OnOpenPauseMenu ();
	}


	void OnGameResume()
	{
		Btn_01_Back.SetActive (false);
		Btn_01_Pause.SetActive (true);
		Btn_01_Resume.SetActive (false);
		HideBottomPanel ();
	}
	#endregion EventHandling


	#region BottomPanel
	public void OnOpenShopMenu()
	{
		if (!GameMgr.Inst.IsGamePaused ()) 
		{
			EventMgr.OnGamePause();
		}
		BottomPanel.SetActive (true);
		Shop_Menu.SetActive (true);
		Pause_Menu.SetActive (false);
        Stats_Menu.SetActive(false);
	}


	public void OnCloseShopMenu()
	{
		EventMgr.OnGameResume ();
	}


	public void OnOpenPauseMenu()
	{
		BottomPanel.SetActive (true);
		Shop_Menu.SetActive (false);
		Pause_Menu.SetActive (true);
        Stats_Menu.SetActive(false);
	}


	public void OnClosePauseMenu()
	{
		EventMgr.OnGameResume ();
	}


	void HideBottomPanel()
	{
		BottomPanel.SetActive (false);
		Shop_Menu.SetActive (false);
        Pause_Menu.SetActive(false);
        Stats_Menu.SetActive(false);
	}


	public void OnResume()
	{
		EventMgr.OnGameResume ();
	}


	public void OnRestart()
	{
		OnGameResume ();
		EventMgr.OnGameRestart ();
	}


	public void OnHome()
	{
		OnGameResume ();
		GameMgr.Inst.LoadScene (GlobalConst.Scene_MainMenu);
	}


    public void OnOpenStatsMenu()
    {
        BottomPanel.SetActive(true);
        Shop_Menu.SetActive(false);
        Pause_Menu.SetActive(false);
        Stats_Menu.SetActive(true);
    }

	#endregion BottomPanel

}

