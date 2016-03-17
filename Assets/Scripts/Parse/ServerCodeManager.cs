using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.customcode;
using SimpleJSON; 

/*[System.Serializable]
public class ShopData 
{
	/*public string sD;
	public  StoreItemsAmount itemAmount;
	//public StoreItemsAmount itemAmount;
}
[System.Serializable]
public class StoreItemsAmount
{
	public string item2;
	public string item1;
	 
}*/

[System.Serializable]
public class shopData
{
	public string item1;//{ get; set; }
	public string item2;// { get; set; }
}

[System.Serializable]
public class RootObject
{
	 
	public shopData shopData;// { get; set; }
}

public class ServerCodeManager : MonoBehaviour 
{
	public static ServerCodeManager instance = null;

	public RootObject rootObject;

	String name = "CustomCodeTest";  


	void OnEnable()
	{
		
		EventManager.SceneStart += OnSceneStart;
		EventManager.SceneEnd += OnSceneEnd;
	}


	void OnDisable()
	{
		EventManager.SceneStart -= OnSceneStart;
		EventManager.SceneEnd -= OnSceneEnd;
	}


	void OnSceneStart()
	{
		instance = this;
	}


	void OnSceneEnd()
	{

	}



	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () 
	{
		CallCustomCode ();
	}

	public void CallCustomCode()
	{
		JSONClass  jsonBody = new JSONClass ();  
		jsonBody.Add("Company", "Shephertz");  
		CustomCodeService customCodeService = App42API.BuildCustomCodeService();   
		customCodeService.RunJavaCode(name, jsonBody, new CustomCodeCallBack());   

	}
	 

	public class CustomCodeCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			//JSONData jData = (JSONData)response;

			//Debug.Log ("Json data" + jData);

			//App42Log.Console("Success : " + response);  

			JSONClass  objecti = (JSONClass )response;  
			App42Log.Console("objectName is : " + objecti["shopData"]);  
			App42Log.Console("Success : " + response);  


			//PuppyManager.instance.beagle = JsonUtility.FromJson < Beagle > (jsonDocList [i].GetJsonDoc ());
			 
			ServerCodeManager.instance.rootObject = JsonUtility.FromJson<RootObject> (response.ToString ());

		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}  
	 
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
