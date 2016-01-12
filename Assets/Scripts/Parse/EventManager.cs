using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour 
{

	public delegate void Action();

	public static event Action SceneStart, GamePaused, GameResumed, UpdateUI ,GameRestart, SceneEnd;

	public delegate void ServerAction();

	public static event ServerAction GetFromServer;

	//public static event ServerAction 

	public static void OnSceneStart()
	{
		if(SceneStart !=null)
		{
			 
			SceneStart();
		}
	}

	public static void OnGamePaused()
	{
		if(GamePaused!=null)
		{
			 
			GamePaused();
		}
	}

	public static void OnGameResumed()
	{
		if(GameResumed!=null)
		{
			 
			GameResumed();
		}
	}

	public static void OnUpdateUI()
	{
		if(UpdateUI !=null)
		{
			UpdateUI();
		}
	}


	public static void OnGameRestart()
	{
		if(GameRestart !=null)
		{
			 
			GameRestart();
		}
	}
	 
	public static void OnSceneEnd()
	{
		if(SceneEnd!=null)
		{
			SceneEnd();
		}
	}
}
