using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using com.shephertz.app42.paas.sdk.csharp; 
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.user; 
using com.shephertz.app42.paas.sdk.csharp.social; 

// 
public enum DogType
{
	Beagle = 1,
	Bulldog,
	Chihuahua,
	GermanShepard,
}

[System.Serializable]
public class Beagle
{
	[HideInInspector]
	public string userID;// = App42API.GetLoggedInUser ();
	 
	public string dogName;
	public int dogLevel;

	public float energy;  
	public float happiness;
	public float tiredness;
	public float foodLevel;
	public float waterLevel;

	public float health;
	public float xp;

	public float walkSpeed;
	public float runSpeed;

	public float jumpHeight;

	public bool isSleeping, isTired, isHungry, isThirsty, isHappy, isPlaying;
}

[System.Serializable]
public class Bulldog
{
	[HideInInspector]
	public string userID;// = App42API.GetLoggedInUser ();

	public string dogName;
	public int dogLevel;

	public float energy;  
	public float happiness;
	public float tiredness;
	public float foodLevel;
	public float waterLevel;

	public float health;
	public float xp;

	public float walkSpeed;
	public float runSpeed;

	public float jumpHeight;

	public bool isSleeping, isTired, isHungry, isThirsty, isHappy, isPlaying;
}

[System.Serializable]
public class Chihuahua
{
	[HideInInspector]
	public string userID;// = App42API.GetLoggedInUser ();

	public string dogName;
	public int dogLevel;

	public float energy;  
	public float happiness;
	public float tiredness;
	public float foodLevel;
	public float waterLevel;

	public float health;
	public float xp;

	public float walkSpeed;
	public float runSpeed;

	public float jumpHeight;

	public bool isSleeping, isTired, isHungry, isThirsty, isHappy, isPlaying;
}

[System.Serializable]
public class GermanShepard
{
	[HideInInspector]
	public string userID;// = App42API.GetLoggedInUser ();

	public string dogName;
	public int dogLevel;

	public float energy;  
	public float happiness;
	public float tiredness;
	public float foodLevel;
	public float waterLevel;

	public float health;
	public float xp;

	public float walkSpeed;
	public float runSpeed;

	public float jumpHeight;

	public bool isSleeping, isTired, isHungry, isThirsty, isHappy, isPlaying;
}




 

public class PuppyManager : MonoBehaviour 
{
	
	public static PuppyManager instance = null;
 
	public Beagle beagle;
	public Bulldog bullDog;
	public Chihuahua chihuahua;
	public GermanShepard germanShepard;

	public string userID;
	public string beagleJson, bullDogJson, chihuahuaJson, germanShepardJson;
	 


	void OnEnable()
	{
		instance = this;
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
 
		Debug.Log("Events are registered :- PuppyManager");
  
		 
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

	}


	public void SaveDogDocToStorage(DogType dogType)
	{
		StorageService storageService = App42API.BuildStorageService ();
		 
		switch(dogType)
		{
		case DogType.Beagle:
			beagleJson = JsonUtility.ToJson (beagle);
			storageService.SaveOrUpdateDocumentByKeyValue(GlobalVariables.dataBaseName, GlobalVariables.collectionBeagleName,"userID",App42API.GetLoggedInUser (), beagleJson, new SaveDogJsonCallBack ());

			break;

		case DogType.Bulldog:
			bullDogJson = JsonUtility.ToJson (bullDog);
			storageService.SaveOrUpdateDocumentByKeyValue(GlobalVariables.dataBaseName, GlobalVariables.collectionBulldogName,"userID",App42API.GetLoggedInUser (), bullDogJson, new SaveDogJsonCallBack ());

			break;

		case DogType.Chihuahua:
			chihuahuaJson = JsonUtility.ToJson (chihuahua);
			storageService.SaveOrUpdateDocumentByKeyValue(GlobalVariables.dataBaseName, GlobalVariables.collectionChihuahuaName,"userID",App42API.GetLoggedInUser (), chihuahuaJson, new SaveDogJsonCallBack ());

			break;

		case DogType.GermanShepard:
			germanShepardJson = JsonUtility.ToJson (germanShepard);
			storageService.SaveOrUpdateDocumentByKeyValue(GlobalVariables.dataBaseName, GlobalVariables.collectionGermanShepardName,"userID",App42API.GetLoggedInUser (), germanShepardJson, new SaveDogJsonCallBack ());

			break;
		}
	}
	 

	public class SaveDogJsonCallBack : App42CallBack
	{
		public void OnSuccess(object response)  
		{  
			Storage storage = (Storage) response;  
			IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
			for(int i=0;i <jsonDocList.Count;i++)  
			{     
				App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
			}   

			Debug.Log ("Dog Saved to DB");
			// navigate to next scene

		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}
 
	public void SaveBeagleDocToStorage()
	{
		StorageService storageService = App42API.BuildStorageService ();
		beagle.userID = App42API.GetLoggedInUser ();
		beagleJson = JsonUtility.ToJson (beagle);
		storageService.SaveOrUpdateDocumentByKeyValue(GlobalVariables.dataBaseName, GlobalVariables.collectionBeagleName,"userID",App42API.GetLoggedInUser (), beagleJson, new SaveBeagleJsonCallBack ());

	}

	public class SaveBeagleJsonCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Storage storage = (Storage) response;  
			IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
			for(int i=0;i <jsonDocList.Count;i++)  
			{     
				App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
			}   

			Debug.Log ("Beagle Saved to DB");
			// navigate to next scene
			 
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}


	public void LoadDogDocFromStorage(DogType dogType)
	{
		Debug.Log ("Checking for User Doc From Storage");
		StorageService storageService = App42API.BuildStorageService();  
		Debug.Log ("Logged in User Id is  "+ App42API.GetLoggedInUser());
		Query query = QueryBuilder.Build ("userId", App42API.GetLoggedInUser (), Operator.EQUALS);

		switch(dogType)
		{
		case DogType.Beagle:
			storageService.FindDocumentByKeyValue (GlobalVariables.dataBaseName,GlobalVariables.collectionBeagleName,"userID",App42API.GetLoggedInUser (),new LoadBeagleDocFromStorageCallBack());
			break;

		case DogType.Bulldog:
			storageService.FindDocumentByKeyValue (GlobalVariables.dataBaseName,GlobalVariables.collectionBulldogName,"userID",App42API.GetLoggedInUser (),new LoadBullDogDocFromStorageCallBack());
			break;

		case DogType.Chihuahua:
			storageService.FindDocumentByKeyValue (GlobalVariables.dataBaseName,GlobalVariables.collectionChihuahuaName,"userID",App42API.GetLoggedInUser (),new LoadChihuahuaDocFromStorageCallBack());
			break;

		case DogType.GermanShepard:
			storageService.FindDocumentByKeyValue (GlobalVariables.dataBaseName,GlobalVariables.collectionGermanShepardName,"userID",App42API.GetLoggedInUser (),new LoadgermanShepardDocFromStorageCallBack());
			break;
		}
	}

	public class LoadBeagleDocFromStorageCallBack : App42CallBack  
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

				PuppyManager.instance.beagle = JsonUtility.FromJson < Beagle > (jsonDocList [i].GetJsonDoc ());

			}
			Debug.Log ("Beagle Doc found");


			//Load to next scene after a wait

		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			Debug.Log ("Beagle doc from storage Not Found! Create a new doc");

		}  
	} 


	public class LoadBullDogDocFromStorageCallBack:App42CallBack
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

				PuppyManager.instance.bullDog = JsonUtility.FromJson < Bulldog > (jsonDocList [i].GetJsonDoc ());

			}
			Debug.Log ("BullDog Doc found");


			//Load to next scene after a wait

		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			Debug.Log ("Bull Dog doc from storage Not Found! Create a new doc");

		}  
	}
	public class LoadChihuahuaDocFromStorageCallBack : App42CallBack  
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

				PuppyManager.instance.chihuahua = JsonUtility.FromJson < Chihuahua > (jsonDocList [i].GetJsonDoc ());

			}
			Debug.Log ("Beagle Doc found");


			//Load to next scene after a wait

		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			Debug.Log ("Chihuahua doc from storage Not Found! Create a new doc");

		}  
	} 

	public class LoadgermanShepardDocFromStorageCallBack : App42CallBack  
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

				PuppyManager.instance.germanShepard = JsonUtility.FromJson < GermanShepard > (jsonDocList [i].GetJsonDoc ());

			}
			Debug.Log ("Beagle Doc found");


			//Load to next scene after a wait

		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			Debug.Log ("Chihuahua doc from storage Not Found! Create a new doc");

		}  
	} 

	public void LoadForBeagleDocFromStorage()
	{
		Debug.Log ("Checking for User Doc From Storage");
		StorageService storageService = App42API.BuildStorageService();  
		Debug.Log ("Logged in User Id is  "+ App42API.GetLoggedInUser());
		Query query = QueryBuilder.Build ("userId", App42API.GetLoggedInUser (), Operator.EQUALS);
		//storageService.FindDocumentsByQuery (GlobalVariables.dataBaseName,GlobalVariables.collectionBeagleName,query,new LoadBeagleDocFromStorageCallBack());
		storageService.FindDocumentByKeyValue (GlobalVariables.dataBaseName,GlobalVariables.collectionBeagleName,"userID",App42API.GetLoggedInUser (),new LoadBeagleDocFromStorageCallBack());
	}



	public void SaveData()
	{
		Debug.Log ("Saving Beagle Data");
		SaveBeagleDocToStorage ();
		//SaveDogDocToStorage (DogType.Bulldog);
	}

	public void LoadData()
	{
		Debug.Log ("Loading Beagle Data");
		LoadForBeagleDocFromStorage ();
		//LoadDogDocFromStorage (DogType.Chihuahua);
	}
			 
}
