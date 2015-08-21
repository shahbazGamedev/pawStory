using UnityEngine;
using System.Collections;

public class EventMgr
{
	public delegate void VoidString (string val);
	public delegate void VoidVoid ();
	public delegate void VoidVector3 (Vector3 val);

	public static event VoidString SceneLoaded;
	public static event VoidVoid GamePause;
	public static event VoidVoid GameResume;
	public static event VoidVoid GameRestart;

	public static event VoidVector3 SetPos;

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



	public static void OnGameRestart(){
        
		if (GameRestart != null){
			GameRestart();
		}
	}


	public static void OnSetPos(Vector3 val){
		if (SetPos != null) {
			SetPos(val);
		}

	}

}
