using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.MiniJSON;

using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using UnityEngine.SocialPlatforms;
using AssemblyCSharp;

public class PuppyWorld_FBManager : MonoBehaviour 
{

	private string lastResponse = "";
	private string accessToken;
	private string userID;
	private string get_data;
	private string userName;
	private string userImageURL;


	public GameObject loadingScreen, fbLoginButton;

	void Awake()
	{
		CallFaceBookInit();
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	void ShowLoadingScreen()
	{

	}

	void ShowFbLoginButton()
	{

	}

	private void CallFaceBookInit()
	{
		Debug.Log("Calling Init Function");
		
		FB.Init(OnInitComplete , OnHideUnity);
		
	}

#region InitFacebook
	private void OnInitComplete()                                                                       
	{                                                                                            
		enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn)                                                                       
		{                                                      
			Debug.Log("LoggedIn");                                                                    
		}  
		else
		{
			Debug.Log("Not LoggedIn");
			
		}
	}  
	
	private void OnHideUnity(bool isGameShown)                                                   
	{  
		if (!isGameShown)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}                                                                                        
	}  
#endregion


	public void CallFacebookLogin()
	{
		GetFacebookLogin();
	}

	private void GetFacebookLogin()
	{
		FB.Login("public_profile,email,user_friends", LoginCallback);
		// Reqest player info and profile picture                                                                           
		
	}
	
	private void LoginCallback(FBResult result)
	{
		
		if (result.Error != null)
			lastResponse = "Error Response:\n" + result.Error;
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
			
		}
		
		else
		{
			lastResponse = "Login was successful!";
			OnLoggedIn();
			GetAccessToken();
			GetUserId();
			 
		}
		
		Debug.Log(lastResponse);
		
	}

	string meQueryString = "/v2.3/me?fields=id,first_name,email"; 
	
	void OnLoggedIn()
	{
		// Reqest player info and profile picture
		FB.API(meQueryString, Facebook.HttpMethod.GET, APICallback);
		GetPictureUrl(Util.GetPictureURL("me", 128, 128));
		 
	}
	void APICallback(FBResult result)
	{
		//Util.Log("APICallback");
		if (result.Error != null)
		{
			
			Debug.Log ("Error");
			
		}
		else
		{
			Debug.Log ("No error");
			get_data = result.Text;
		}
		
		var dict= Json.Deserialize(get_data) as IDictionary;
		PuppyWorld_GlobalVariables.instance.userName = dict["first_name"].ToString();
		userName  = dict["first_name"].ToString();
		
		/*userNameText.text = userName;*/
 
		Debug.Log("Player Name " +dict["first_name"].ToString());// userName); 
	}

	void GetPictureUrl(string url)
	{
		FB.API(url,Facebook.HttpMethod.GET,result =>
		       {
			if (result.Error != null)
			{
				//Util.LogError(result.Error);
				return;
			}
			
			userImageURL = Util.DeserializePictureURLString(result.Text);
			PuppyWorld_GlobalVariables.instance.userProfilImageURL = userImageURL;
			Debug.Log(userImageURL);
		 
		});
		
		//GetFBPublishAction();
	}

	void GetAccessToken()
	{
		accessToken =  FB.AccessToken;

		PuppyWorld_GlobalVariables.instance.userFBAccessToken = FB.AccessToken; 
		Debug.Log("Access Token "+ accessToken);
	}
	
	void GetUserId()
	{
		CallFBActivateApp();
		PuppyWorld_GlobalVariables.instance.userFBID = FB.UserId;
		userID = FB.UserId;
		Debug.Log("User ID "+ userID);
 

	}

	private void CallFBActivateApp()
	{
		FB.ActivateApp();
		Debug.Log("Check Insights section for your app in the App Dashboard under \"Mobile App Installs\"");

	}


#region CallPublishAction  // to be called when sharing is done... 
	
	
	void GetFBPublishAction()
	{
		CallFBLoginForPublish();
	}
	
	
	
	void CallFBLoginForPublish()
	{
		// It is generally good behavior to split asking for read and publish
		// permissions rather than ask for them all at once.
		//
		// In your own game, consider postponing this call until the moment
		// you actually need it.
		FB.Login("publish_actions", PublishCallBack);
	}
	
	void PublishCallBack(FBResult result)
	{
		if (result.Error != null)
			lastResponse = "Error Response:\n" + result.Error;
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Publish permission Unsuccesfull";
			
		}
		else
		{
			lastResponse = "Publish permission succesfull";
		}
		Debug.Log(lastResponse);
	}
	
#endregion  
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
