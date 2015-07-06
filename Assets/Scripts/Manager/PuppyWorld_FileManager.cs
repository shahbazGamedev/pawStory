using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using UnityEngine.SocialPlatforms;
using AssemblyCSharp;
using SimpleJSON;

 

// Class used to do all the file releated operations. Response was sent to FileManagerResponseCallBack

public class PuppyWorld_FileManager : MonoBehaviour 
{

	public List<PuppyDBase> puppyDBase;
	public JSONClass jsonC = new JSONClass();
	public Dictionary<string, string> dDict = new Dictionary <string,string >();


	// Use this for initialization
	void Start ()
	{
		 
	}


	public void PreparePuppyFile()
	{
		jsonC.Add("UserName",PuppyWorld_GlobalVariables.instance.userFBID);
		jsonC.Add("PuppyName",puppyDBase[0].PuppyName);
		jsonC.Add("PuppyLevel",puppyDBase[0].PuppyLevel);
		jsonC.Add("PuppyColor",puppyDBase[0].PuppyColor);
		Debug.Log(jsonC);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}


public class PuppyWorld_FileManagerResponseCallBack : App42CallBack
{
	
	public void OnSuccess(object response)  
	{  
		
	}  
	public void OnException(Exception e)  
	{  
		App42Log.Console("Exception : " + e);  
	}  


	public static string response;
}
