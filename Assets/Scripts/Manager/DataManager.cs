using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

[System.Serializable]
public class PuppyDBase
{
	public string 	PuppyName ;//	 {get; set;}
	public string 	PuppyLevel;//	 {get; set;}
	public string 	PuppyColor;//	 {get; set;}
}
public class DataManager : MonoBehaviour 
{

	public List<PuppyDBase> puppyDBase;

	public Dictionary<string, string> dDict = new Dictionary <string,string >();

	string jsonTest = "{\"PuppyName\":\"pup\",\"PuppyLevel\":\"12\",\"PuppyColor\":\"brown\"}";

	// Use this for initialization
	void Start () 
	{
	 

		var dict= Json.Deserialize(jsonTest) as IDictionary;

		Debug.Log(dict["PuppyName"].ToString());

		string newJson = Json.Serialize(dDict);
		Debug.Log(newJson);
		
		//var dictN= Json.Deserialize(newJson) as IDictionary;
		
		//Debug.Log(dictN["PuppyName"].ToString());
		 
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
