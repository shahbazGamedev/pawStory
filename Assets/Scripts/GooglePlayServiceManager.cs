using UnityEngine;

using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AchievementNameID
{
	public string achievementName;
	public string achievementID;
}

public class GooglePlayServiceManager : MonoBehaviour 
{
	//public static GooglePlayServiceManager instance;

	//public List<AchievementNameID> achievementNameID;


	//void Awake()
	//{
	//	instance = this;
	//}

	//// Use this for initialization
	//void Start ()
	//{
	//	Debug.Log("init called in google play service manager...");
	//	//PlayGameServices.init("46818076463",false);
	//	InitCall();	 
	//}

	//public void InitCall()
	//{
		
	//	//if(!PlayGameServices.isSignedIn())
	//	PlayGameServices.authenticate();
	//}


	//public void PostScoreToLeaderBoard()
	//{
	//	Debug.Log("Posting score");
	//	PlayGameServices.submitScore("CgkIr87MtK4BEAIQAA",(long)GlobalVariables.distanceCovered);
	//}
	//public void ShowLeaderBoard()
	//{
	//	Debug.Log("Showing Leaderboard");
	//	PlayGameServices.reloadAchievementAndLeaderboardData();
	//	PlayGameServices.showLeaderboard("CgkIr87MtK4BEAIQAA");

	//}

	//public void ShowAchievements()
	//{
	//	Debug.Log("Showing Achievement");
	//	PlayGameServices.showAchievements();
	//}

	//public void UnlockAchievement(string achievementName)
	//{

	//	for(int i =0;i<achievementNameID.Count;i++)
	//	{
	//		if(achievementNameID[i].achievementName == achievementName)
	//		{
	//			Debug.Log("Displaying achievements : "+ achievementNameID[i].achievementID);
	//			PlayGameServices.unlockAchievement(achievementNameID[i].achievementID,true);
	//		}
	//	}



	//}

	//// Update is called once per frame
	//void Update () 
	//{
	
	//}
}
