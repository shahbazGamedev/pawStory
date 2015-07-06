using UnityEngine;
using System.Collections;

public class PuppyWorld_GlobalVariables : MonoBehaviour 
{

	
	public static PuppyWorld_GlobalVariables instance = null;
	
	public string	APIKEY , SECRETKEY;
	public string	userName;
	public string 	userFBID;
	public string 	userFBAccessToken;
	public string 	userProfilImageURL;

	public bool isFBLoggedIn;

	//public string	
	
	void Awake()
	{
		instance = this;
	}
}
