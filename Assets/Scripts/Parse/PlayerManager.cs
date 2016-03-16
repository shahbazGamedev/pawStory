using UnityEngine;
using System.Collections;
using System;
/*using Parse;

using System.Threading.Tasks;*/

using com.shephertz.app42.paas.sdk.csharp;    
using com.shephertz.app42.paas.sdk.csharp.user; 
using com.shephertz.app42.paas.sdk.csharp.social;  

public class PlayerData
{
	public string playerName;
	public string playerFBAccessToken;
	public string playerEmail;
	public string playerLevel;
	public string playerNumberOfPets;

}

[Serializable]
public class FBUserData
{
	public string name;
	public string id;
}

[Serializable]
public class AccessToken
{
	public string accessToken;
}


public class PlayerManager : MonoBehaviour 
{
	
	public static  PlayerManager instance;

	public FBUserData userData;
	public string userId;
	public string accessToken;
	public string expirationTimeStamp;
	public bool isUserExists;

	public void Awake()
	{
		instance = this;

	}

	public void Start()
	{
		CheckForUser ();
	}


	public void StoreUserData(string result)
	{
		userData = JsonUtility.FromJson<FBUserData>(result);
	}

	public void CheckForUser()
	{
		UserService userService = App42API.BuildUserService ();

		userService.GetUser (App42API.GetLoggedInUser (), new UserCallBack ());

	}

	public class UserCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  

		}  
		public void OnException(Exception e)  
		{  
			Debug.Log ("User doesnt Exists - Create user with FBCredentials");
			PlayerManager.instance.isUserExists = false;
			FBManager.instance.CallFBInit ();
		}  
	}  


	public void CreateUserUsingFaceBook()
	{
		SocialService socialService = App42API.BuildSocialService ();
		socialService.LinkUserFacebookAccount (userId,accessToken,new FacebookUserCallBack());
	}
	 
	public class FacebookUserCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			com.shephertz.app42.paas.sdk.csharp.social.Social social = (com.shephertz.app42.paas.sdk.csharp.social.Social) response;      
			App42Log.Console("userName is" + social.GetUserName());   
			App42Log.Console("fb Access Token is" + social.GetFacebookAccessToken());  
		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}  



	// Use this for initialization
	 


	/*IEnumerator CreateUser()
	{
		Debug.Log("creating player ");
		 

		Task<ParseUser> logInTask = ParseFacebookUtils.LogInAsync(userId, accessToken,System.DateTime.Now);

		 
		Debug.Log("created user ");
		yield return new WaitForEndOfFrame();


	}*/
	/*public void CreateUpdateParseUser()
	{

		Debug.Log("creating user ");
		 
		 
		//Task signUpTask = user.SignUpAsync();

		 

		StartCoroutine(CreateUser());
		 
		//ParseUserManagement.instance.SaveData();

		//user["player"] = PlayerManager.instance.userId;

		//ParseUser.CurrentUser.ObjectId = PlayerManager.instance.userId;
		//ParseUserManagement.instance.SaveData();
		//ParseUserManagement.instance.GetData();
		//ParseFacebookUtils.LogInAsync(userId, accessToken, DateTime.Now);
	}*/

	// Update is called once per frame
	void Update () 
	{
	
	}
}
