﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void Start () 
	{
	}

	void Update () 
	{
	}

	public void OnPlayBtn()
	{
		GameMgr.instance.LoadScene (GameMgr.Scene_TournamentSelection);
	}
}
		
