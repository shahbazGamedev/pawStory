using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System.Net;
/*using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using SimpleJSON;*/
//using AssemblyCSharp;
//using App42Json;
using System;

 
[System.Serializable]
public class PuppyDBase
{
	public string 	PuppyName;//    {get; set;}
	public string 	PuppyLevel;//	 {get; set;}
	public string 	PuppyColor;//	 {get; set;}
}

// Class used to do all the file releated operations. Response was sent to FileManagerResponseCallBack

public class PuppyWorld_FileManager : MonoBehaviour 
{
	/*public static PuppyWorld_FileManager instance;

	public List<PuppyDBase> puppyDBase;
	public List<PuppyDBase> successDase;

	public JSONClass jsonC = new JSONClass();
	public Dictionary<string, string> dDict = new Dictionary <string,string >();

	StorageService storageService ;//= new StorageService();

	PuppyWorld_FileManagerResponseCallBack storageCallBack = new PuppyWorld_FileManagerResponseCallBack();

	#if UNITY_EDITOR
	public static bool Validator (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{return true;}
	#endif

	void Awake()
	{
		instance = this;
		  
	}

	// Use this for initialization
	void Start ()
	{
		 
	}


	public void AddToFile()
	{
		jsonC.Add("UserName",PuppyWorld_GlobalVariables.instance.userFBID);
		jsonC.Add("PuppyName",puppyDBase[0].PuppyName);
		jsonC.Add("PuppyLevel",puppyDBase[0].PuppyLevel);
		jsonC.Add("PuppyColor",puppyDBase[0].PuppyColor);
		Debug.Log(jsonC);
		storageService =  App42API.BuildStorageService ();
		storageService.InsertJSONDocument (PuppyWorld_GlobalVariables.instance.dbName, PuppyWorld_GlobalVariables.instance.collectionName, jsonC, storageCallBack);
	}

	public void CheckForFile()
	{
		storageService =  App42API.BuildStorageService ();
		storageService.FindDocumentById(PuppyWorld_GlobalVariables.instance.dbName,PuppyWorld_GlobalVariables.instance.collectionName,PuppyWorld_GlobalVariables.instance.userFBID,storageCallBack);

 	}

	public void RetrieveFile()
	{
		storageService =  App42API.BuildStorageService ();
		storageService.FindDocumentByKeyValue(PuppyWorld_GlobalVariables.instance.dbName,PuppyWorld_GlobalVariables.instance.collectionName,"UserName",PuppyWorld_GlobalVariables.instance.userFBID,storageCallBack);
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}


public class PuppyWorld_FileManagerResponseCallBack : App42CallBack
{
	private string result = "";
	private string error = "";
	
	public void OnSuccess(object response)  
	{  
		result = response.ToString();
		Storage storage = (Storage) response;  
		IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
		for(int i=0;i <jsonDocList.Count;i++)  
		{     
			App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
			App42Log.Console("jsonDoc is " + jsonDocList[i].GetJsonDoc());  
			
			JObject obj = JSON.Parse(jsonDocList[i].GetJsonDoc());
			Debug.Log(obj);
			//Dictionary<string ,object> dict = new Dictionary<string, object>();
			Debug.Log(obj["UserName"]);
		    PuppyWorld_FileManager.instance.successDase[0].PuppyName = obj["PuppyName"] ;
			PuppyWorld_FileManager.instance.successDase[0].PuppyColor = obj["PuppyColor"] ;
			PuppyWorld_FileManager.instance.successDase[0].PuppyLevel = obj["PuppyLevel"] ;
		}    
		
	}  
	public void OnException(Exception e)  
	{  
		//error =e.ToString();
		App42Log.Console("Exception : " + e);  
		App42Exception ex = (App42Exception)e;  
		int appErrorCode = ex.GetAppErrorCode();  
		int httpErrorCode = ex.GetHttpErrorCode();  
		if(appErrorCode == 2606)  
		{  
			 
			error ="FileNotFound";
			PuppyWorld_FileManager.instance.AddToFile();
			// Handle here for Bad Request (Document Id '4faa3f1ac68df147a' is not valid.)  
		}  
		
	}  
	
	
	public string GetResult ()
	{
		return result;
	}	

	public string GetErrorCode()
	{
		return error;
	}
	*/
}
