using UnityEngine;
using System.Collections;

public class LocalSaveManager : MonoBehaviour 
{


	public static LocalSaveManager instance = null;



	void Awake()
	{
		instance = this;
		//SaveDataToLocal ();
		///LoadDataFromLocal ();
		//ResetDataFromLocal();
	}

	void Start()
	{
		
	}

	public void SaveDataToLocal()
	{
		Debug.Log ("saving data to local");
		PlayerPrefs.SetString ("userID",GlobalVariables.userID);
		Debug.Log (GlobalVariables.userID);
	}

	public void LoadDataFromLocal()
	{
		Debug.Log ("loading data from local");
		GlobalVariables.userID = PlayerPrefs.GetString ("userID");
		Debug.Log (GlobalVariables.userID);
	}
	 

	public void ResetDataFromLocal()
	{
		Debug.Log ("resetting player prefs");
		PlayerPrefs.DeleteAll ();
	}
}
