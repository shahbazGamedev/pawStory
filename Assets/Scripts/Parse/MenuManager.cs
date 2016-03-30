using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

using com.shephertz.app42.paas.sdk.csharp; 
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.user; 
using com.shephertz.app42.paas.sdk.csharp.social; 

[System.Serializable]
public class MenuItems
{
	public string menuScreenName;
	public GameObject menuScreenGameObject;
}


public class MenuManager : MonoBehaviour
{


	public static MenuManager instance = null;

	public List<MenuItems> menuItems;
	public Image playerProfilePic;
	public Text loadingScreenText;
	public GameObject menuScreenPanel, facebookLoginPanel, loadingScreen;
	//public GameObject fbProfileScreen, dogProfileScreen, shopScreen, settingScreen, notificationScreen, friendsScreen, achievementScreen;


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

		Debug.Log("Events are registered :- MenuManager");


	}





	void Awake()
	{
		instance = this;
	}



	// Use this for initialization
	void Start () 
	{
		for(int i =0;i<=menuItems.Count-1;i++)
		{	 
			menuItems [i].menuScreenGameObject.SetActive (false); 
		}

		//menuScreenPanel.SetActive (false);
		//CheckForUser ();
	}


	public void CheckForUser()
	{
		if(App42API.GetLoggedInUser ()!=null )//|| App42API.GetLoggedInUser ()!=null)
		{
			ShowLoadingScreen ("Initializing Server..!!!");
			//loadingScreen.SetActive (true);
			GlobalVariables.userID = App42API.GetLoggedInUser ();
			Debug.Log ("Global Variables "+ GlobalVariables.userID);
			UserManager.instance.CheckForUserDocFromStorage ();


		} 
		else
		{
			FBManager.instance.CallFBInit ();
			Debug.Log ("Display FB Login Button");
			ShowFbLoginPanel ();
		}
	}

	public void ShowFbLoginPanel()
	{
		facebookLoginPanel.SetActive (true);
	}

	public void HideFbLoginPanel()
	{
		facebookLoginPanel.SetActive (false);
	}

	public void GetUserFBProfilePic()
	{
		StartCoroutine (RetrieveUserFBProfilePic ());
	}

	IEnumerator RetrieveUserFBProfilePic()
	{
		Debug.Log ("Downloading fb pic");

		Texture2D tex = new Texture2D (1, 1);
		var www = new WWW (UserManager.instance.userData.userPicURL);
		yield return www;
		www.LoadImageIntoTexture (tex);

		Sprite image = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (2,2));
		playerProfilePic.sprite = image;

		HideLoadingScreen ();
		 
	}


	public void LoginUsingFB()
	{
		FBManager.instance.CallFBLoginWithPermissions ();
	}

	public void ShowLoadingScreen(string text)
	{
		loadingScreenText.text = text;
		loadingScreen.SetActive (true);
	}

	public void HideLoadingScreen()
	{
		loadingScreen.SetActive (false);
	}

	public void DisablePanel(string panelName)
	{
		for(int i =0;i<=menuItems.Count-1;i++)
		{
			if(panelName == menuItems[i].menuScreenName)
			{
				menuItems [i].menuScreenGameObject.SetActive (false);
			} 
		}
	}

	 

	public void ClickButton(string buttonName)
	{
		 
		for(int i =0;i<=menuItems.Count-1;i++)
		{
			if(buttonName == menuItems[i].menuScreenName && menuItems [i].menuScreenGameObject.activeSelf==false )
			{
				menuItems [i].menuScreenGameObject.SetActive (true);
			}
			 
			else
			{
				menuItems [i].menuScreenGameObject.SetActive (false);
			}
		}
		 
	}

	public void TrainingSelectionClicked()
	{
		SceneManager.LoadScene ("PuppyTraining");
	}
	public void OnHomeButtonClicked()
	{
		
	}


	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}


	 
	public void OnShopButtonClicked()
	{
		
	}

 

	// Update is called once per frame
	void Update () 
	{
		 
	}


	void OnSceneEnd()
	{


	}
}
