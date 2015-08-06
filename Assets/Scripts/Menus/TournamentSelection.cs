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
		GameMgr.instance.LoadScene (GlobalConst.Scene_DiscDogs);
	}


	public void OnGame_02()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_DockJump);
	}


	public void OnGame_03()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_Tracking);
	}


	public void OnGame_04()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_Obedience);
	}


	public void OnGame_05()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_ColorLesson);
	}
	
	
	public void OnGame_06()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_Agility);
	}
	
	
	public void OnGame_07()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_Bubble);
	}
	
	
	public void OnGame_08()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_Tugofwar);
	}


}
