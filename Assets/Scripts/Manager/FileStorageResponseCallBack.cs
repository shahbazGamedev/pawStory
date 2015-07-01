using UnityEngine;
using System.Collections;
using System;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using UnityEngine.SocialPlatforms;
using AssemblyCSharp;

public class FileStorageResponseCallBack :  MonoBehaviour,App42CallBack
{
 
	public void OnSuccess(object response)  
	{  
		
	}  
	public void OnException(Exception e)  
	{  
		App42Log.Console("Exception : " + e);  
	}  

}
