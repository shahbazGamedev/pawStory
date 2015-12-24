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

	public void CreateUpdateParseUser()
	{
		Task<ParseUser> logInTask = ParseFacebookUtils.LogInAsync(userId, accessToken,System.DateTime.Now);
		//ParseFacebookUtils.LogInAsync(userId, accessToken, DateTime.Now);
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
