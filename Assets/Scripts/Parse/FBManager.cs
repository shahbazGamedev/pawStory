using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Facebook.Unity;
using Facebook.MiniJSON;
using UnityEngine.Purchasing;
using com.shephertz.app42.paas.sdk.csharp;   
using com.shephertz.app42.paas.sdk.csharp.user;   

public class FBManager : MonoBehaviour
{


	public static FBManager instance;
	private string lastResponse = string.Empty;
	private  Texture2D profilePic;
	protected string LastResponse
	{
		get
		{
			return this.lastResponse;
		}

		set
		{
			this.lastResponse = value;
		}
	}



	private string status = "Ready";
	protected string Status
	{
		get
		{
			return this.status;
		}

		set
		{
			this.status = value;
		}
	}

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
		
	}


	void OnSceneEnd()
	{

	}


	public void Awake()
	{
		instance = this;
		CallFBInit();
	}

	// Use this for initialization
	void Start ()
	{
				 
	}



	public void CallFBInit()
	{
		Debug.Log ("calling FB init");
		FB.Init(OnInitComplete, OnHideUnity);
	}

	private void OnInitComplete()
	{
		Status = "Success - Check logk for details";
		LastResponse = "Success Response: OnInitComplete Called\n";
		string logMessage = string.Format(
			"OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
			FB.IsLoggedIn,
			FB.IsInitialized);
		 
	}

	private void OnHideUnity(bool isGameShown)
	{
		Status = "Success - Check log for details";
		LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
		 
	}
	public void CallFBLoginWithPermissions()
	{
		Debug.Log ("CallingLoginwithPermissions");
		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, HandleLoginResult);
	}

	void AuthCallBack(ILoginResult result)
	{
		if(FB.IsLoggedIn)
		{
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

			Debug.Log("AccessToken "+ aToken);
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) 
			{
				Debug.Log(perm); 
				var dict = Json.Deserialize(perm) as Dictionary<string,object>;

				Debug.Log(dict);


			}


		}
	}

	void HandleLoginResult(IResult result)
	{
		MenuManager.instance.ShowLoadingScreen ("Logging In...");
		if (result == null)
		{
			LastResponse = "Null Response\n";

			return;
		}

		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error))
		{
			Status = "Error - Check log for details";
			LastResponse = "Error Response:\n" + result.Error;
			MenuManager.instance.ShowLoadingScreen ("Error Connecting In...");

		}
		else if (result.Cancelled)
		{
			Status = "Cancelled - Check log for details";
			LastResponse = "Cancelled Response:\n" + result.RawResult;

		}
		else if (!string.IsNullOrEmpty(result.RawResult))
		{
			Status = "Success - Check log for details";
			LastResponse = "Success Response:\n" + result.RawResult;
			 
			var dict = Json.Deserialize(result.RawResult) as Dictionary<string,object>;
			 
			Debug.Log(dict["access_token"]);
			Debug.Log(dict["user_id"]);

			GlobalVariables.userID = dict["user_id"] as string;
			App42API.SetLoggedInUser (GlobalVariables.userID);
			GlobalVariables.userAccessToken =  dict["access_token"]as string;
			 
			UserManager.instance.CreateUserWithFaceBook ();
			//CallFBLoginForPublish ();

			 

		}
		else
		{
			LastResponse = "Empty Response\n";

		}
	}

	private void CallFBLoginForPublish()
	{
		
		// It is generally good behavior to split asking for read and publish
		// permissions rather than ask for them all at once.
		//
		// In your own game, consider postponing this call until the moment
		// you actually need it.
		FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, HandlePublishResult);
	}

	void HandlePublishResult(IResult result)
	{
		if (result == null)
		{
			LastResponse = "Null Response\n";

			return;
		}

		if (!string.IsNullOrEmpty(result.Error))
		{
			Status = "Error - Check log for details";
			LastResponse = "Error Response:\n" + result.Error;

		}
		else if (result.Cancelled)
		{
			Status = "Cancelled - Check log for details";
			LastResponse = "Cancelled Response:\n" + result.RawResult;

		}
		else if (!string.IsNullOrEmpty(result.RawResult))
		{
			Status = "Success - Check log for details";
			LastResponse = "Success Response:\n" + result.RawResult;
 
 		}
		else
		{
			LastResponse = "Empty Response\n";

		}
	}

	public void RetrieveName()
	{
		FB.API("/me", HttpMethod.GET, HandleNameResult);
	}

	void HandleNameResult(IResult result)
	{
		if (result == null)
		{
			LastResponse = "Null Response\n";

			return;
		}


		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error))
		{
			Status = "Error - Check log for details";
			LastResponse = "Error Response:\n" + result.Error;

		}
		else if (result.Cancelled)
		{
			Status = "Cancelled - Check log for details";
			LastResponse = "Cancelled Response:\n" + result.RawResult;

		}
		else if (!string.IsNullOrEmpty(result.RawResult))
		{
			Status = "Success - Check log for details";
			LastResponse = "Success Response:\n" + result.RawResult;

			PlayerManager.instance.StoreUserData(result.RawResult);

			 
		}
		else
		{
			LastResponse = "Empty Response\n";

		}
	}

	public void RetrieveProfilePic()
	{
		FB.API("/me/picture", HttpMethod.GET, ProfilePhotoCallback);
	}

	private void ProfilePhotoCallback(IGraphResult result)
	{
		if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
		{
			profilePic = result.Texture;
		}

		HandlePhotoResult(result);
	}

	void HandlePhotoResult(IResult result)
	{
		if (result == null)
		{
			LastResponse = "Null Response\n";

			return;
		}


		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error))
		{
			Status = "Error - Check log for details";
			LastResponse = "Error Response:\n" + result.Error;

		}
		else if (result.Cancelled)
		{
			Status = "Cancelled - Check log for details";
			LastResponse = "Cancelled Response:\n" + result.RawResult;

		}
		else if (!string.IsNullOrEmpty(result.RawResult))
		{
			Status = "Success - Check log for details";
			LastResponse = "Success Response:\n" + result.RawResult;
			 
		}
		else
		{
			LastResponse = "Empty Response\n";

		}
	}

	// Update is called once per frame
	void Update () 
	{
		//Debug.Log("Last Response" + LastResponse);
	}
}
