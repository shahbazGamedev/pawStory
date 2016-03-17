using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.pushNotification;
using System;
using System.Runtime.InteropServices;
public class PushNotificationManager : MonoBehaviour 
{

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void registerForRemoteNotifications();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void setListenerGameObject(string listenerName);


	public static PushNotificationManager instance = null;

	public Text logText;

	public DateTime dateTime  ; 

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

	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () 
	{

		instance = this;
		//#if UNITY_IOS && !UNITY_EDITOR
		Debug.Log("Start called -----"+this.gameObject.name);

	//	Debug.Log (dateTime.;//AddMinutes (5).ToUniversalTime ());
		//#endif

		Debug.Log (SystemInfo.deviceType.ToString ());


		//StoreDeviceToken ();
		//StoreDeviceToken ();
	}

	void OnSceneStart()
	{
	}


	void OnSceneEnd()
	{
		
	}

	public void RegisterForPush()
	{
		Debug.Log ("Recording device token for push");
		setListenerGameObject(this.gameObject.name);// sets the name of the game object as a listener to which this script is assigned.
		registerForRemoteNotifications();
	}

	void onDidRegisterForRemoteNotificationsWithDeviceToken(string deviceToken)
	{
		if (deviceToken != null && deviceToken.Length!=0) 
		{
			registerDeviceTokenToApp42PushNotificationService(deviceToken,App42API.GetLoggedInUser ());
		}
	}

	//Sent when the application failed to be registered with Apple Push Notification Service (APNS).
	void onDidFailToRegisterForRemoteNotificcallBackationsWithError(string error)
	{
		Debug.Log(error);
	}
	//Registers a user with the given device token to APP42 push notification service
	void registerDeviceTokenToApp42PushNotificationService(string devToken,string userName)
	{
		Debug.Log("registerDeviceTokenToApp42PushNotificationService   Called");
		ServiceAPI serviceAPI = new ServiceAPI(GlobalVariables.APIKEY,GlobalVariables.SECRETKEY);	
		PushNotificationService pushService = serviceAPI.BuildPushNotificationService();
		pushService.StoreDeviceToken(App42API.GetLoggedInUser (),devToken,"iOS",new PushResponseCallBack());
	}

	public class PushResponseCallBack :  App42CallBack 
	{

		public void OnSuccess(object response)
		{
			if(response is PushNotification){
				PushNotification pushNotification = (PushNotification)response;
				Debug.Log ("UserName : " + pushNotification.GetUserName());	
				Debug.Log ("Expiery : " + pushNotification.GetExpiry());
				Debug.Log ("DeviceToken : " + pushNotification.GetDeviceToken());	
				Debug.Log ("pushNotification : " + pushNotification.GetMessage());	
				Debug.Log ("pushNotification : " + pushNotification.GetStrResponse());	
				Debug.Log ("pushNotification : " + pushNotification.GetTotalRecords());	
				Debug.Log ("pushNotification : " + pushNotification.GetType());	

			}
		}

		public void OnException(Exception e)
		{
			Debug.Log ("Exception--------- : " + e);
		}
	}

	//Sends push to a given user
	void SendPushToUser(string userName,string message)
	{
		Debug.Log("SendPushToUser Called");
		ServiceAPI serviceAPI = new ServiceAPI(GlobalVariables.APIKEY,GlobalVariables.SECRETKEY);
		PushNotificationService pushService = serviceAPI.BuildPushNotificationService();
		//pushService.send
		//pushService.SendPushMessageToUser(userName,message,callBack);
	}

	void onDidRegisterUserNotificationSettings(string setting)
	{
		Debug.Log("setting"+setting);
	}

	public void SchedulePush()
	{
		PushNotificationService pushService = App42API.BuildPushNotificationService ();
		pushService.ScheduleMessageToUser (App42API.GetLoggedInUser (),"Schedule push ",dateTime.AddMinutes (5).ToUniversalTime (), new SchedulePushCallBack());
	}

	public class SchedulePushCallBack : App42CallBack
	{
		public void OnSuccess(object response)
		{
			Debug.Log ("Push message working!!");
			PushNotification pushNotification = (PushNotification) response;    
			App42Log.Console("Username is " +pushNotification.GetUserName());  
			App42Log.Console("Message is " + pushNotification.GetMessage());  
			App42Log.Console("Expiry is " + pushNotification.GetExpiry());  

		}


		public void OnException(Exception e)
		{
			Debug.Log ("Push Not working!!");
			App42Log.Console("Exception : " + e);  

		}
	}


	//Sent when the application Receives a push notification
	void onPushNotificationsReceived(string pushMessageString)
	{
		Console.WriteLine("onPushNotificationsReceived....Called");
		//dump you code here
		logText.text = "Push Message :" + pushMessageString;
		Debug.Log(pushMessageString);
	}



	/*public void StoreDeviceToken()
	{
		PushNotificationService pushService = App42API.BuildPushNotificationService ();
		var deviceType = SystemInfo.deviceType;
		pushService.StoreDeviceToken (App42API.GetLoggedInUser (),SystemInfo.deviceUniqueIdentifier,SystemInfo.deviceType.ToString (),new StoreDeviceTokenCallBack());
	}

	public class StoreDeviceTokenCallBack : App42CallBack  
	{  
		public void OnSuccess(object response)  
		{  
			PushNotification pushNotification = (PushNotification) response;    
			App42Log.Console("UserName is " + pushNotification.GetUserName());  
			App42Log.Console("Type is " +  pushNotification.GetType());  
			App42Log.Console("DeviceToken is " +  pushNotification.GetDeviceToken());     
		} 
		public void OnException(Exception e)  
		{  
			App42Log.Console("Exception : " + e);  
		}  
	}  */


	public void ScheduleNotification(string notificationID, string notificationMessage )
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
