using UnityEngine;
using System.Collections;

public class GameOverTemplate : MonoBehaviour {

	void Start () {
	
	}
	

	void Update () {
	
	}

    public void OnHome()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }

    public void OnRestart()
    {
        EventMgr.OnGameRestart();
    }
}
