using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void Start () 
	{
	}


	void Update () 
	{
	}


	public void OnDogSelection()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_PuppySelection);
	}


    public void OnTraining()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_PuppyTraining);
    }


    public void OnGames()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_TournamentSelection);
    }

}
		
