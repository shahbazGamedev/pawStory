using UnityEngine;
using System.Collections;
using System;
using Parse;

using System.Threading.Tasks;

public class PlayerData
{
	public string playerName;
	public string playerFBAccessToken;
	public string playerEmail;
	public string playerLevel;
	public string playerNumberOfPets;

}

[Serializable]
public class UserData
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

	public UserData userData;
	public string userId;
	public string accessToken;
	public string expirationTimeStamp;

	public void Awake()
	{
		instance = this;
	}


	 

	public void StoreUserData(string result)
	{
		userData = JsonUtility.FromJson<UserData>(result);
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	IEnumerator CreateUser()
	{
		Debug.Log("creating player ");
		 

		Task<ParseUser> logInTask = ParseFacebookUtils.LogInAsync(userId, accessToken,System.DateTime.Now);

		 
		Debug.Log("created user ");
		yield return new WaitForEndOfFrame();


	}
	public void CreateUpdateParseUser()
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
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
