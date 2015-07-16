using UnityEngine;
using System.Collections;

public class EventMgr
{
	public delegate void VoidString (string val);
	public delegate void VoidVoid ();

	public static event VoidString SceneLoaded;
	public static event VoidVoid GamePause;
	public static event VoidVoid GameResume;


	public static void OnSceneLoaded(string val)
	{
		if (SceneLoaded != null) 
		{
			SceneLoaded(val);
		}
	}


	public static void OnGamePause()
	{
		if (GamePause != null) 
		{
			GamePause();
		}
	}


	public static void OnGameResume()
	{
		if (GameResume != null) 
		{
			GameResume();
		}
	}
}
