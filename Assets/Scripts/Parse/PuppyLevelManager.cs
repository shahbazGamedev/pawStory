using UnityEngine;
using System.Collections;

public class PuppyLevelManager : MonoBehaviour 
{

	public static PuppyLevelManager instance = null;

	public int puppyId;

	public int puppuLevel;



	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		puppyId = GlobalVariables.puppyID;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
