using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuppySelectionManager : MonoBehaviour
{
	public static PuppySelectionManager instance = null;

	void OnEnable()
	{
		EventManager.SceneStart += OnSceneStart;
		EventManager.SceneEnd += OnSceneEnd;
	}


	void OnDisable()
	{
		EventManager.SceneStart -= OnSceneStart;
		EventManager.SceneEnd -= OnSceneEnd;
	}


	void OnSceneStart()
	{

	}


	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
	
	}


	public void SelectPuppy(int index)
	{
		GlobalVariables.puppyID = index;
		// save this to the server as soon as it is recieved

	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnSceneEnd()
	{

	}
}
