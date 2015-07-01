using UnityEngine;
using System.Collections;

public class PuppyWorld_GameConstant : MonoBehaviour 
{
	 
	public static PuppyWorld_GameConstant instance = null;
	 
	public string	APIKEY , SECRETKEY;
	public string	userName;
	public string 	userFBID;
	public string 	userFBAccessToken;
	public string 	userProfilImageURL;

	//public string	

	void Awake()
	{
		instance = this;
	}
}
