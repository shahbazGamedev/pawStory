using UnityEngine;
using System.Collections;
using System;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using UnityEngine.SocialPlatforms;
using AssemblyCSharp;

// Class used to do all the file releated operations. Response was sent to FileManagerResponseCallBack

public class PuppyWorld_FileManager : MonoBehaviour 
{

	// Use this for initialization
	void Start ()
	{
		 
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
