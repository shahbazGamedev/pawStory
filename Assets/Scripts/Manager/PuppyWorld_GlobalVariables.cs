using UnityEngine;
using System.Collections;

public class PuppyWorld_GlobalVariables : MonoBehaviour 
{

	
	public static PuppyWorld_GlobalVariables instance = null;
	
	public string	APIKEY , SECRETKEY;
	public string	userName = "pradeeepUser";
	public string 	userFBID="1111";
	public string 	userFBAccessToken;
	public string 	userProfilImageURL;
	public string 	dbName, collectionName;
	public bool 	isFBLoggedIn;

	//public string	
	
	void Awake()
	{
		instance = this;
	}
}
