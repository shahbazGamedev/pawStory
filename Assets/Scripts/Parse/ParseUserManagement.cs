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

public enum GameState
{
	
	LOGIN,
	SIGNUP,
	LOADING,
	MENU,
	GAME,
	FINISH,
	NONE =0

}

public class ParseUserManagement : MonoBehaviour 
{


	//public List<PuppyData> puppyData;

	//public GameStates state;

	public InputField userName, email, password;

	public GameObject createNewObjectPanel;

	ParseUser user;

	public PuppyData pupData;

	string objectID;

	void Awake()
	{
		ParseObject.RegisterSubclass<PuppyData>();

		//createNewObjectPanel.SetActive(false);
	}
	// Use this for initialization
	void Start () 
	{
		Debug.Log("Calling user login");
		if (ParseUser.CurrentUser != null)
		{
			 //Login with User credentials.
			Debug.Log(Parse.ParseUser.CurrentUser.Username);
			createNewObjectPanel.SetActive(false);
		}

		else
		{
			createNewObjectPanel.SetActive(true);
		}

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
				}
				else
				{
					Debug.Log("UserCreated");
					Debug.Log(Parse.ParseUser.CurrentUser.Username);
				}
			});
		 
		 
		 
	 
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
		/*ParseObject pData = new ParseObject("PData");


		pData["player"] = ParseUser.CurrentUser;
		pData["finalScore"] = 1000;

		pData.SaveAsync().ContinueWith(t => {
			if (!t.IsFaulted) {
				Debug.Log("DataSaved successfully");
				pData.ObjectId = pData["player"];
				objectID = pData.ObjectId;
				Debug.Log(objectID);
				// Game history was saved successfully!
			}
			else
			{
				Debug.Log("Not saved");
			}
		});*/
	



		//Task saveTask

		/*var puppyDataObject = new ParseObject("PuppyDataObject");
		//puppyDataObject["puppydataList"] = puppyData;
		Task saveTask = puppyDataObject.SaveAsync();*/

	}

	IEnumerator SaveDataToParse()
	{
		ParseObject pupData;
		var user = ParseUser.CurrentUser;

		if(!user.ContainsKey("player"))
		{
			pupData = new ParseObject("PuppyDataBase");
		}
		else
		{
			pupData = ParseObject.CreateWithoutData("PuppyDataBase", user.Get<string>("player"));
		}

		pupData["player"] = ParseUser.CurrentUser;
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
	}

	public void GetData()
	{
		/*Debug.Log("Getting Data...");
		ParseQuery<ParseObject> query = ParseObject.GetQuery("PData");
		query.GetAsync(objectID).ContinueWith(t =>
			{
				if(!t.IsFaulted)
				{
					ParseObject pGetData = t.Result;
					string playerName = pGetData.Get<String>("player");
					Debug.Log("Player Name : "+ playerName);
					int score = pGetData.Get<int>("finalScore");

					Debug.Log("Score : "+ score);
				} 

				else
				{
					Debug.Log("Cant Get Data");
				}

			});
*/

		StartCoroutine(GetDataFromParse());
	}

	IEnumerator GetDataFromParse()
	{
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



				//Best = dbScores.Get<int>("bestScores");

				Debug.Log("Get Data loaded!");
				Debug.Log("puppy level from server : " + pupLevel);
			}
		}
		else
		{
			// handle file corrupted error and redirect to new game.
			//Best = 0;
		}
	}


	public void UserLogginIn()
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
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
}
