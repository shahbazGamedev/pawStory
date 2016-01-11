using UnityEngine;
using System.Collections;

public class TournamentSelection : MonoBehaviour
{
	void Start ()
	{
	}


	void Update ()
	{
	}


	public void OnGame_01()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_DiscDogs);
	}


	public void OnGame_02()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_DockJump);
	}


	public void OnGame_03()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_Tracking);
	}


	public void OnGame_04()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_Obedience);
	}


	public void OnGame_05()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_ColorLesson);
	}
	
	
	public void OnGame_06()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_Agility);
	}
	
	
	public void OnGame_07()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_Bubble);
	}
	
	
	public void OnGame_08()
	{
		GameMgr.Inst.LoadScene (GlobalConst.Scene_Tugofwar);
	}


    public void OnGame_09()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_Skipping);
    }

    public void OnGame_10()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_ComboJump);
    }


    public void OnGame_11()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_InternationalTournament);
    }


    public void OnHome()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }

}
