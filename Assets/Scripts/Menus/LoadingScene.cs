using UnityEngine;
using System.Collections.Generic;

public class LoadingScene : MonoBehaviour {

	float curTime = 0.0f;
	float screenTime = 2.0f;
	int curScreen = 0;
	public List<GameObject> Screens; 
	void Start ()
	{	
		UpdateScreen ();
	}

	void Update () 
	{
		curTime += Time.deltaTime;
		if (curTime > screenTime) 
		{
			curTime = 0;
			curScreen++;
			if(curScreen >= Screens.Count)
			{
				GameMgr.Inst.LoadScene (GlobalConst.Scene_MainMenu);
			}
			else
			{
				UpdateScreen();
			}
		}
	}


	void UpdateScreen()
	{
		for (int i = 0; i < Screens.Count; i++)
		{
			Screens[i].SetActive(false);
		}
		Screens [curScreen].SetActive (true);
	}
}
