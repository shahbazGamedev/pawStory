using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour 
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(Input.GetMouseButtonDown(1))
		{
			Debug.Log("UnMuting Sound");
			AudioManager.audioManager.UnMuteSound();// = true;
		}
		else if(Input.GetMouseButtonDown(0))
		{
			Debug.Log("Muting Sound");
			AudioManager.audioManager.MuteSound();// = false;
		}
	
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Playing button click sound");
			LevelAudio.levelAudio.PlaySound("ButtonClick");
		}
	}
}
