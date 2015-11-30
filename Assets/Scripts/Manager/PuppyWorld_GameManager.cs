using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/*using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using UnityEngine.SocialPlatforms;*/
//using AssemblyCSharp;
//using SimpleJSON;


public class PuppyWorld_GameManager : MonoBehaviour 
{

	public static PuppyWorld_GameManager instance = null;

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start () 
	{
		/*App42Log.SetDebug(true); 
		App42API.Initialize(PuppyWorld_GlobalVariables.instance.APIKEY,PuppyWorld_GlobalVariables.instance.SECRETKEY);  
		Debug.Log("App42 Registered");*/
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
