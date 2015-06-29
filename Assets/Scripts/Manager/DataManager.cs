using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using SimpleJSON;
using AssemblyCSharp;
using App42Json;
using System;


[System.Serializable]
public class PuppyDBase
{
	public string 	PuppyName;//    {get; set;}
	public string 	PuppyLevel;//	 {get; set;}
	public string 	PuppyColor;//	 {get; set;}
}

 
public class DataManager : MonoBehaviour 
{
	 
	StorageService storageService;// =  App42API.BuildStorageService (); // Initialising Storage Service.
	StorageResponse callBack = new StorageResponse ();

	public string userName ,dbname, collectionName; 
	public List<PuppyDBase> puppyDBase;

	public Dictionary<string, string> dDict = new Dictionary <string,string >();

	//string jsonTest = "{\"PuppyName\":\"pup\",\"PuppyLevel\":\"12\",\"PuppyColor\":\"brown\"}";

	public JSONClass jsonC = new JSONClass();

	public void Awake()
	{

		App42Log.SetDebug(true); 
		App42API.Initialize("2c2f08398a4fb2273f474b4dfcc0cc3aa2ff2a78c4db8a0937991080d8b9751f","71444279f46016e07ba2e42a379ab186b7cbd2649491a150223fb96439ad331b");  
		 
	}

	// Use this for initialization
	void Start () 
	{

		jsonC.Add("UserName","pradee");
		jsonC.Add("PuppyName",puppyDBase[0].PuppyName);
		jsonC.Add("PuppyLevel",puppyDBase[0].PuppyLevel);
		jsonC.Add("PuppyColor",puppyDBase[0].PuppyColor);
		Debug.Log(jsonC);

	}


	public void InsertData()
	{
	
		storageService =  App42API.BuildStorageService ();
		storageService.InsertJSONDocument (dbname, collectionName, jsonC, callBack);
	}

	public void RetrieveData()
	{
		storageService =  App42API.BuildStorageService ();
		storageService.FindDocumentByKeyValue(dbname,collectionName,"PuppyName","mani",new UnityCallBack());
	}

	public class UnityCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Storage storage = (Storage) response;  
			IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
			for(int i=0;i <jsonDocList.Count;i++)  
			{     
				App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
				App42Log.Console("jsonDoc is " + jsonDocList[i].GetJsonDoc());  

				JObject obj = JSON.Parse(jsonDocList[i].GetJsonDoc());
				Debug.Log(obj);
				Dictionary<string ,object> dict = new Dictionary<string, object>();
				Debug.Log(obj["UserName"]);
			}    
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
