using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.app42Event;

public class EventTracker : MonoBehaviour 
{

	public static EventTracker instance = null;


	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		App42API.EnableEventService (true);
		App42API.EnableAppStateEventTracking(true);  
	}

	public void TrackEvent(string eventName)
	{
		Dictionary<String,object> properties = new Dictionary<string, object> ();  
		properties.Add ("UserID" ,GlobalVariables.userID);
		properties.Add ("Name" ,UserManager.instance.userData.name);
		EventService eventService = App42API.BuildEventService ();
		eventService.TrackEvent (eventName,properties,new EventTrackCallBack());
	}


	public class EventTrackCallBack : App42CallBack
	{
		public void OnSuccess(object response)  
		{  
			App42Response app42Response = (App42Response) response;     
			App42Log.Console("App42Response Is : " + app42Response); 
			Debug.Log ("event registered");
		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}
	// Update is called once per frame
	void Update () {
	
	}
}
