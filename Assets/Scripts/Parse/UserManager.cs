using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using com.shephertz.app42.paas.sdk.csharp; 
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.user; 
using com.shephertz.app42.paas.sdk.csharp.social; 


[System.Serializable]
public class UserData
{
	public string userID;
	public string accessToken;
	public string name;
	public string firstName;
	public string lastName;
	public string userGender;
	public string userPicURL;

	public DateTime dateTimeForDialyBonus;
	 
}


public class UserManager : MonoBehaviour 
{
	public static UserManager instance = null;

	public UserData userData;

	public string setUserDataJson,getUserDataJson ,userDataJson;

	public GameObject loadingScreen;

	public GameObject facebookLoginPanel;

	public GameObject menuScreenPanel;

	public Text userName;

	public string expirationTimeStamp;
	public bool isUserExists;


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


	void Awake()
	{
		
	}

	void OnSceneStart()
	{
		Debug.Log ("starting");
		Debug.Log (App42API.GetLoggedInUser ());
	}


	void OnSceneEnd()
	{

	}

	 
	// Use this for initialization
	void Start () 
	{
		Debug.Log ("User manager Start");
	}

 
	public void CheckForUserDocFromStorage()
	{
		Debug.Log ("Checking for User Doc From Storage");
		StorageService storageService = App42API.BuildStorageService();   
		storageService.FindDocumentByKeyValue (GlobalVariables.dataBaseName,GlobalVariables.collectionName,"userID",App42API.GetLoggedInUser (),new CheckForUserDocFromStorageCallBack());

	}

	public class CheckForUserDocFromStorageCallBack : App42CallBack  
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
				UserManager.instance.userData = JsonUtility.FromJson<UserData> (jsonDocList [i].GetJsonDoc ());
			}
			Debug.Log ("Doc found");
			//EventTracker.instance.TrackEvent ("Doc Found");
			MenuManager.instance.GetUserFBProfilePic ();
			MenuManager.instance.HideFbLoginPanel ();

		 
		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			Debug.Log ("User doc from storage Not Found! Create a User with FBCredentials");
			MenuManager.instance.ShowFbLoginPanel ();
			MenuManager.instance.HideLoadingScreen ();
			 
		}  
	}  


	 
	public void CreateUserWithFaceBook()
	{
		SocialService socialService = App42API.BuildSocialService ();
		//only place to use global var userID
		socialService.LinkUserFacebookAccount (App42API.GetLoggedInUser (),GlobalVariables.userAccessToken,new FacebookUserCallBack());
		MenuManager.instance.HideFbLoginPanel ();
		MenuManager.instance.ShowLoadingScreen ("Creating User....");
		 
	}

	public class FacebookUserCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			com.shephertz.app42.paas.sdk.csharp.social.Social social = (com.shephertz.app42.paas.sdk.csharp.social.Social) response;      
			App42Log.Console("userName is" + social.GetUserName());   
			App42Log.Console("fb Access Token is" + social.GetFacebookAccessToken());  
			Debug.Log ("User Created");
			UserManager.instance.userData.userID = App42API.GetLoggedInUser ();
			UserManager.instance.userData.accessToken = GlobalVariables.userAccessToken;
			UserManager.instance.userData.name = social.GetFacebookProfile ().GetName ();
			UserManager.instance.userData.firstName = social.GetFacebookProfile ().GetFirstName ();
			UserManager.instance.userData.lastName = social.GetFacebookProfile ().GetLastName ();
			UserManager.instance.userData.userGender = social.GetFacebookProfile ().GetGender ();
			UserManager.instance.userData.userPicURL = social.GetFacebookProfile ().GetPicture ();

			UserManager.instance.CreateUserProfileInStorage ();
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}  
	 
	public void CreateUserProfileInStorage()
	{
		StorageService storageService = App42API.BuildStorageService ();
		userDataJson = JsonUtility.ToJson (userData);
		storageService.InsertJSONDocument (GlobalVariables.dataBaseName, GlobalVariables.collectionName, userDataJson, new CreateUserProfileCallBack ());

	}
	public class CreateUserProfileCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Storage storage = (Storage) response;  
			IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
			for(int i=0;i <jsonDocList.Count;i++)  
			{     
				App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
			}   

			Debug.Log ("User Saved to DB");
			 
			MenuManager.instance.HideLoadingScreen ();
			MenuManager.instance.HideFbLoginPanel ();
		 	MenuManager.instance.GetUserFBProfilePic ();

		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
			MenuManager.instance.ShowLoadingScreen ("Error Connecting to Server...");
		}  
	}
	 
	 
	public class CreatePuppy1CallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			Storage storage = (Storage) response;  
			IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();   
			for(int i=0;i <jsonDocList.Count;i++)  
			{     
				App42Log.Console("objectId is " + jsonDocList[i].GetDocId());  
			}   

			Debug.Log ("Puppy Saved to DB");
			// navigate to next scene
		}  

		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}

	 

	 
}
