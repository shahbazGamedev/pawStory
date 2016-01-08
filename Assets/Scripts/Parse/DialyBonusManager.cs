using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

using Parse;
public class DialyBonusManager : MonoBehaviour 
{

	DateTime localTime;
	DateTime serverTime;

	public delegate void GetServeTime(); // declaring delegates.
	public GetServeTime ServerCallBackFn;


	void Awake()
	{

		localTime = DateTime.UtcNow;
		Debug.Log("LocalTime :" + DateTime.UtcNow);
		 

	}


	public void CallServerTime(GetServeTime callBackFn)
	{
		
	}


	// Use this for initialization
	void Start () 
	{
		//CheckForDialyBonus();
		StartCoroutine(CallFunc());

	}

	private IEnumerator CallFunc()
	{
		var task = ParseCloud.CallFunctionAsync<DateTime>("hello", new Dictionary<string, object>());
		while (!task.IsCompleted)
		{
			yield return null;
		}

		if (task.IsFaulted || task.IsCanceled)
		{
			Debug.LogError("failed");
			foreach(Exception e in task.Exception.InnerExceptions)
			{
				Debug.LogError(e.Message);
			}
		}
		else
		{
			Debug.Log("success");

			serverTime = task.Result;
			Debug.Log("server time" + serverTime); 

			TimeSpan diff = localTime.Subtract(serverTime);
			Debug.Log(diff.Hours);

			// if diff in hours is positive give a reward..
		}
	}




	public void CheckForDialyBonus()
	{
		string stringDate = PlayerPrefs.GetString("PlayDate");
		DateTime oldDate =  Convert.ToDateTime(stringDate);
		DateTime newDate = System.DateTime.Now;
		Debug.Log("LastDay: " + oldDate);
		Debug.Log("CurrDay: " + newDate);

		TimeSpan difference = newDate.Subtract(oldDate);
		if(difference.Days >= 1){
			Debug.Log("New Reward!");
			string newStringDate = Convert.ToString(newDate);
			PlayerPrefs.SetString("PlayDate", newStringDate);
			//giveGift();
		}
	}


	void GetTimeFromParseCloud()
	{
		//Parse.ParseCloud.CallFunctionAsync
	}
	// Update is called once per frame
	void Update () {
	
	}
}
