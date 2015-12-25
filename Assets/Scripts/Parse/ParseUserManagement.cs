using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Parse;
using System.Threading.Tasks;

[ParseClassName("PuppyData") ]
public class PuppyData :ParseObject
{
	[ParseFieldName("puppyName")]
	public string PuppyName
	{
		get { return GetProperty<string>("PuppyName");}
		set { SetProperty<string>(value,"PuppyName");}
	}

	public int level;
	public int currentTournamentLevel;
	public bool isInappPurchased;

}

public enum UserState
{
	
	LOGIN,
	SIGNUP,
	LOADING,
	DELETE,
	NONE =0

}

public class ParseUserManagement : MonoBehaviour 
{


	//public List<PuppyData> puppyData;

	//public GameStates state;

	public static ParseUserManagement instance ;

	public InputField userName, email, password;

	public GameObject createNewObjectPanel , dataStorePanel;

	ParseUser user;

	public PuppyData pupData;

	string objectID;

	void Awake()
	{
		instance = this;
		ParseObject.RegisterSubclass<PuppyData>();

		//createNewObjectPanel.SetActive(false);
	}
	// Use this for initialization
	void Start () 
	{
		/*Debug.Log("Calling user login");
		if (ParseUser.CurrentUser != null)
		{
			 //Login with User credentials.
			Debug.Log(ParseUser.CurrentUser.Username);
			createNewObjectPanel.SetActive(false);
			dataStorePanel.SetActive(true);
		}

		else
		{
			createNewObjectPanel.SetActive(true);
			dataStorePanel.SetActive(false);
		}
*/
	}

	void DisplayLogin()
	{
		createNewObjectPanel.SetActive(true);
		dataStorePanel.SetActive(false);
	}

	void DisplayDataStorage()
	{
		createNewObjectPanel.SetActive(false);
		dataStorePanel.SetActive(true);
	}

	public void CreateOrUpdateUser()
	{
		
		 	user = new ParseUser()
			{
				Username = email.text,
				Password = password.text,
				Email = email.text
			};


			//Task signUpTask = user.SignUpAsync();

		user.SignUpAsync().ContinueWith(t=>
			{
				if(t.IsFaulted || t.IsCanceled)
				{

					Debug.Log("user isnt created try again");
					foreach (var currError in t.Exception.InnerExceptions)
					{
						var error = (ParseException)currError;
						Debug.LogError("Login error:" + error.Code);
					
						/*switch (error.Code)
						{
						case ParseException.ErrorCode.UsernameMissing:
							Debug.LogError("Username not exists!");
							yield break;
						case ParseException.ErrorCode.PasswordMissing:
							Debug.LogError("Password incorrect!");
							yield break;
						default:
							Debug.LogError("Login error:" + error.Code);
							yield break;
						}*/
					}
				}
				else
				{
					Debug.Log("UserCreated");
					Debug.Log(Parse.ParseUser.CurrentUser.Username);


					//DisplayDataStorage();
					 
				}
			});
		 
		 
		SaveData();
	 
	}



	/*private IEnumerator SignUpHandler()
	{
		bool success = true;
		string error;

		Task signup = user.SignUpAsync();//.ContinueWith(t =>
		while (!signup.IsCompleted) yield return null;
		if (signup.IsFaulted || signup.IsCanceled)
		{
			//Debug.Log("Error " + signup.Exception.Message);
			error = "Failed to sign up Parse User. Reason: " + signup.Exception.Message;
			success = false;
		}
		else
		{
			Debug.Log("Done");
			Application.LoadLevel("ExampleScene");
		}

	}*/


	 
	public void SaveData()
	{

		StartCoroutine(SaveDataToParse());
		 
	}

	IEnumerator SaveDataToParse()
	{
		ParseObject pupData;
		var user = ParseUser.CurrentUser;
		//user.ObjectId = PlayerManager.instance.userId;
		//user.Add("player",PlayerManager.instance.userId);

		if(!user.ContainsKey("player"))
		{
			pupData = new ParseObject("PuppyDataBase");
		}
		else
		{
			//user.Add("player",PlayerManager.instance.userId);
			pupData = ParseObject.CreateWithoutData("PuppyDataBase", user.Get<string>("player"));

		}

		pupData["player"] =ParseUser.CurrentUser; //PlayerManager.instance.userId;
		pupData["fbUserId"] = PlayerManager.instance.userId;
		pupData["pupLevel"] = 2;
		// add or modify the elements here which has to be stored for the game

		var saveTask = pupData.SaveAsync();
		while (!saveTask.IsCompleted)
			yield return null;

		user["player"] = pupData.ObjectId;
		var userSaveTask = user.SaveAsync();

		while (!userSaveTask.IsCompleted)	 
			yield return null;

			

		Debug.Log("Data saved!");

		//GetData();
	}

	public void GetData()
	{

		StartCoroutine(GetDataFromParse());
	}

	IEnumerator GetDataFromParse()
	{
		Debug.Log("getting data");
		var user = ParseUser.CurrentUser;
		if (user.ContainsKey("player"))
		{
			var key = user.Get<string>("player");

			var task = ParseObject.GetQuery("PuppyDataBase").GetAsync(key);

			while (!task.IsCompleted && !task.IsFaulted)
				yield return null;

			if (task.IsFaulted)
			{
				foreach (var currError in task.Exception.InnerExceptions)
				{
					var error = (ParseException)currError;

					if (error.Code == ParseException.ErrorCode.ObjectNotFound)
					{
						Debug.LogError("No obj with id " + key);
						yield break;
					}

					Debug.LogError("Login error:" + error.Code);
				}
			}
			else
			{
				var pupDB = task.Result;

				var pupLevel = pupDB.Get<int>("pupLevel");

				Debug.Log("Get Data loaded!");
				Debug.Log("puppy level from server : " + pupLevel);
			}
		}
		else
		{
			// handle file corrupted error and redirect to new game.
			//Best = 0;
			Debug.Log("cant get data..");
		}
	}

	public void LinkFaceBook()
	{
		
	}



	/*public void UserLogginIn()
	{
		ParseUser.LogInAsync("myname", "mypass").ContinueWith(t =>
			{
				if (t.IsFaulted || t.IsCanceled)
				{
					// The login failed. Check the error to see why.
					 
				}
				else
				{
					// Login was successful.
					createNewObjectPanel.SetActive(false);
					Debug.Log("Login Sucessful");
				}
			});
	}*/

	// Update is called once per frame
	void Update () 
	{
	
	}
}
