using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp; 
using com.shephertz.app42.paas.sdk.csharp.storage; 
using com.shephertz.app42.paas.sdk.csharp.timer;

[System.Serializable]
public class Utility
{
	public string userID;
	public int numberOfCoins;
	public string dateTimeLastOpened;
}

public class UtilityManager : MonoBehaviour 
{


	public static UtilityManager instance =null;

	public Utility utility;

	public string utilityJson;

	string severTimeString;
	public DateTime localTime, serverTime;


	string testTimer ="TestServerTime";
	long timeInSeconds =360;

	void Awake()
	{
		instance = this;
		//CheckForDialyBonus ();
	}


	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Utility Local Time " + DateTime.UtcNow.ToLocalTime().ToString ());


		 
	}


	public void StartTimer()
	{
		Debug.Log ("Starting Timer");
		TimerService timerService =  App42API.BuildTimerService();  
		timerService.StartTimer(testTimer, App42API.GetLoggedInUser (), new TimerCallBack());  
		//timerService.CreateOrUpdateTimer(testTimer, timeInSeconds, new TimerCallBack());   
		//timerService.IsTimerActive(testTimer, App42API.GetLoggedInUser (), new TimerCallBack());   
	}

	public class CreateTimerCallBack :App42CallBack
	{
		public void OnSuccess(object response)  
		{  
			Timer timer = (Timer)response;  
			App42Log.Console("Timer Name is: "+timer.GetName());  
			App42Log.Console("Time is: "+timer.GetTimeInSeconds());  
		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}

	public class TimerCallBack :App42CallBack
	{
		public void OnSuccess(object response)  
		{  
			Timer timer = (Timer)response;  
			App42Log.Console("Timer Name is: "+timer.GetName());  
			App42Log.Console("UserName is:" + timer.GetUserName());  
			App42Log.Console("Current time is:" + timer.GetCurrentTime());  
			App42Log.Console("Start time is:" + timer.GetStartTime());  
			App42Log.Console("End time is:" + timer.GetEndTime());  
		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}


	public void CreateOrSaveUtilityToStorage()
	{
		StorageService storageService = App42API.BuildStorageService ();
		utility.userID = App42API.GetLoggedInUser ();
		utility.dateTimeLastOpened = DateTime.UtcNow.ToLocalTime().ToString ();
		Debug.Log ("Utility Local Time " + DateTime.UtcNow.ToLocalTime().ToString ());
		utilityJson = JsonUtility.ToJson (utility);
		storageService.SaveOrUpdateDocumentByKeyValue(GlobalVariables.dataBaseName, GlobalVariables.collectionUtility,"userID",App42API.GetLoggedInUser (), utilityJson, new SaveUtilityJsonCallBack ());

	}

	public class SaveUtilityJsonCallBack : App42CallBack
	{

		public void OnSuccess(object response)  
		{  
			Storage storage = (Storage) response;  
			IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
			for(int i=0;i <jsonDocList.Count;i++)  
			{     
				App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
			}   

			Debug.Log ("Utility Saved to DB");
			// navigate to next scene

		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}

	public void LoadUtilityFromStorage()
	{
		Debug.Log ("Checking for User Utility Doc From Storage");
		StorageService storageService = App42API.BuildStorageService();  
		Debug.Log ("Logged in User Id is  "+ App42API.GetLoggedInUser());
		Query query = QueryBuilder.Build ("userId", App42API.GetLoggedInUser (), Operator.EQUALS);

		storageService.FindDocumentByKeyValue (GlobalVariables.dataBaseName,GlobalVariables.collectionUtility,"userID",App42API.GetLoggedInUser (),new LoadUtilityDocFromStorageCallBack());

	}


	public class LoadUtilityDocFromStorageCallBack : App42CallBack
	{
		public void OnSuccess(object response)  
		{  
			Storage storage = (Storage) response;  
			IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
			for(int i=0;i <jsonDocList.Count;i++)  
			{     
				App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
				App42Log.Console("jsonDoc is " + jsonDocList[i].GetJsonDoc()); 

				App42Log.Console("jsonDoc is " + jsonDocList[i].docId); 

				UtilityManager.instance.utility = JsonUtility.FromJson < Utility > (jsonDocList [i].GetJsonDoc ());

			}
			Debug.Log ("Utility Doc found");

			//Load to next scene after a wait

		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			Debug.Log ("Utility doc from storage Not Found! Create a new doc");

		}  

	}



	public void CheckForDialyBonus()
	{
		//severTimeString = utility.dateTimeLastOpened;
		serverTime = Convert.ToDateTime (utility.dateTimeLastOpened).ToLocalTime ();
		localTime = DateTime.UtcNow.ToLocalTime ();

		TimeSpan diff = localTime.Subtract (serverTime);
		Debug.Log (diff.Days);
		if(diff.Days>=1)
		{
			Debug.Log ("Present him the gift");

			// store the current time to the server.
			string newServerTime = Convert.ToString (localTime);
			utility.dateTimeLastOpened = newServerTime;
			CreateOrSaveUtilityToStorage ();
		}

		else
		{
			Debug.Log ("Check back later!");
		}


	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
