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


public class FacebookManager : MonoBehaviour 
{

	public static FacebookManager instance;
	public string userId, userName;
	public string accessToken;
	public string linkToTheApp, linkName, linkDescription, linkCaption, pictureURL; 
	public Texture userTexture;
	public Text userNameText;

	public Image userProfileImage;

	public GameObject quad;

	private string userImageURL;
	private string lastResponse = "";
	private Texture2D lastResponseTexture;
	private bool enabled;
	private Dictionary<string, string>   profile         = null;
	private string get_data;
	private bool    haveUserPicture       = false;
	public Sprite  userProfileSprite;

	void Awake()
	{
		instance = this;
		CallFaceBookInit();

		App42Log.SetDebug(true);  
		App42API.Initialize("2c2f08398a4fb2273f474b4dfcc0cc3aa2ff2a78c4db8a0937991080d8b9751f","71444279f46016e07ba2e42a379ab186b7cbd2649491a150223fb96439ad331b");  
		  

	}

//	public class UnityCallBack : App42CallBack  
//	{  
//		public void OnSuccess(object response)  
//		{  
//			com.shephertz.app42.paas.sdk.csharp.social.Social social = (com.shephertz.app42.paas.sdk.csharp.social.Social) response;      
//			App42Log.Console("facebookProfileId is : " + social.GetFacebookProfile().GetId());  
//			App42Log.Console("facebookProfileName is : " + social.GetFacebookProfile().GetName());     
//			App42Log.Console("facebookProfileImage is : " + social.GetFacebookProfile().GetPicture());   
//			
//		}  
//		public void OnException(Exception e)  
//		{  
//			App42Log.Console("Exception : " + e);  
//		}  
//	}  

	public class UnityCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			com.shephertz.app42.paas.sdk.csharp.social.Social social = (com.shephertz.app42.paas.sdk.csharp.social.Social) response;      
			App42Log.Console("userName is" + social.GetUserName());   
			App42Log.Console("status is" + social.GetStatus());   

			App42Log.Console("fb Access Token is" + social.GetFacebookAccessToken());  
			App42Log.Console("facebookProfileId is : " + social.GetFacebookProfile().GetUsername());//.GetId());  
			App42Log.Console("facebookProfileName is : " + social.GetFacebookProfile().GetName());     
			App42Log.Console("facebookProfileImage is : " + social.GetFacebookProfile().GetPicture());   
						
		}  
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
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

#region CallFacebookLogin, AccessToken,PublishInstall, UserName, UserImage and UserId

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

	string meQueryString = "/v2.3/me?fields=id,first_name,email";//,friends.limit(100).fields(first_name,id,picture.width(128).height(128)),invitable_friends.limit(100).fields(first_name,id,picture.width(128).height(128))";

	void OnLoggedIn()
	{
		// Reqest player info and profile picture
		FB.API(meQueryString, Facebook.HttpMethod.GET, APICallback);
		GetPictureUrl(Util.GetPictureURL("me", 128, 128));
		//LoadPictureAPI(Util.GetPictureURL("me", 128, 128),MyPictureCallback);
		//Debug.Log(Util.GetPictureURL("me", 128, 128));

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
			Debug.Log(userImageURL);
			GetUserProfilePicture();
		});

		//userProfileSprite =  GetUserProfilePicture();
	}

    public Sprite GetUserProfilePicture()
	{
		//get
		{ 
			StartCoroutine(FetchImageFromServer(userImageURL));
			return userProfileSprite;
		}

	}

	IEnumerator FetchImageFromServer(string imageURL)
	{
		Debug.Log("Downloading Profile Image...");
		Texture2D texture = new Texture2D(1,1);
		WWW www = new WWW(userImageURL);
		yield return www;
		www.LoadImageIntoTexture(texture);
	    userProfileSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		 
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
		userName  = dict["first_name"].ToString();
		 
		userNameText.text = userName;



		Debug.Log("Player Name " +dict["first_name"].ToString());// userName); 
	}
	

	delegate void LoadPictureCallback (Texture texture);

	void MyPictureCallback(Texture texture)
	{
		//Util.Log("MyPictureCallback");
		
		if (texture ==  null)
		{
			// Let's just try again
			LoadPictureAPI(Util.GetPictureURL("me", 128, 128),MyPictureCallback);
			
			return;
		}


		userTexture = texture;
		quad.GetComponent<Renderer>().material.mainTexture =  userTexture;
		haveUserPicture = true;
		 
	}

	
	IEnumerator LoadPictureEnumerator(string url, LoadPictureCallback callback)    
	{
		WWW www = new WWW(url);
		yield return www;
		callback(www.texture);
	}

	void LoadPictureAPI (string url, LoadPictureCallback callback)
	{
		FB.API(url,Facebook.HttpMethod.GET,result =>
		       {
			if (result.Error != null)
			{
				//Util.LogError(result.Error);
				return;
			}
			
			var imageUrl = Util.DeserializePictureURLString(result.Text);
			Debug.Log(imageUrl);
			
			StartCoroutine(LoadPictureEnumerator(imageUrl,callback));
		});
	}
	void LoadPictureURL (string url, LoadPictureCallback callback)
	{
		StartCoroutine(LoadPictureEnumerator(url,callback));
		
	}


	void GetAccessToken()
	{
		accessToken =  FB.AccessToken;
		
		Debug.Log("Access Token "+ accessToken);
	}
	
	void GetUserId()
	{
		CallFBActivateApp();
		userId = FB.UserId;
		Debug.Log("User ID "+ userId);

		 
		
	}

 

	#region FB.ActivateApp()  // To include publish install 
		
		private void CallFBActivateApp()
		{
			FB.ActivateApp();
			Debug.Log("Check Insights section for your app in the App Dashboard under \"Mobile App Installs\"");
		}
		
	#endregion

#endregion	 

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


	public void CallPublishFeed()
	{
		GetFBPublishAction();
		CallFBFeed();
	}

#region FB.Feed() example

	public string FeedToId = "";
	public string FeedLink = "";
	public string FeedLinkName = "";
	public string FeedLinkCaption = "";
	public string FeedLinkDescription = "";
	public string FeedPicture = "";
	public string FeedMediaSource = "";
	public string FeedActionName = "";
	public string FeedActionLink = "";
	public string FeedReference = "";
	public bool IncludeFeedProperties = false;
	private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();
	
	private void CallFBFeed()
	{
		Dictionary<string, string[]> feedProperties = null;
		if (IncludeFeedProperties)
		{
			feedProperties = FeedProperties;
		}
		FB.Feed(
			 
			link: FeedLink,
			linkName: FeedLinkName,
			linkCaption: FeedLinkCaption,
			linkDescription: FeedLinkDescription,
			picture: FeedPicture,
			 
			callback: Callback
			);
	}
	




	protected void Callback(FBResult result)
	{
		lastResponseTexture = null;
		// Some platforms return the empty string instead of null.
		if (!String.IsNullOrEmpty (result.Error))
		{
			lastResponse = "Error Response:\n" + result.Error;
		}
		else if (!String.IsNullOrEmpty (result.Text))
		{
			lastResponse = "Success Response:\n" + result.Text;
		}
		else if (result.Texture != null)
		{
			lastResponseTexture = result.Texture;
			lastResponse = "Success Response: texture\n";
		}
		else
		{
			lastResponse = "Empty Response\n";
		}
	}

#endregion

	public void CallFBLogout()
	{
		FB.Logout();
	}


	public void LinkSocialUser()
	{
		SocialService socialService = App42API.BuildSocialService();   
		socialService.LinkUserFacebookAccount(userName, accessToken, "1411530505809972","e0ce9a5611a34a53772d26d9bafd85c2", new UnityCallBack()); 
	}

	public void PostStatus()
	{
		SocialService socialService = App42API.BuildSocialService();   
		socialService.GetFacebookProfile(accessToken, new UnityCallBack()); 
		Debug.Log("access token shep " + accessToken);

//		SocialService socialService = App42API.BuildSocialService();   
//		 
//		socialService.UpdateFacebookStatus(userName, "Hi How are u ?", new UnityCallBack());   
	}
	 
}
