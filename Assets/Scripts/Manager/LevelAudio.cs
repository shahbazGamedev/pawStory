using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelAudio : MonoBehaviour 
{

	public static LevelAudio levelAudio = null;
	public List<AudioList> audioList;



	void OnEnable()
	{

	}

	void OnDisable()
	{

		//AudioManager.audioManager.audioSourceSFX.Clear();
	}


	void Awake()
	{
		levelAudio = this;
		for(int i =0;i<audioList.Count;i++)
		{
			AudioManager.audioManager.audioSourceSFX.Add(audioList[i].audioSource);
		}
	}


	void Start()
	{
		if(AudioManager.audioManager.muteSound)
		{
			AudioManager.audioManager.MuteSound();
			Debug.Log("All sound Muted");
		}
		else
		{  
			AudioManager.audioManager.UnMuteSound();
			Debug.Log("All sound UnMuted");
		}
	}

	public void PlaySound(string audioName)
	{
		if( !AudioManager.audioManager.muteSound)
		{
			for(int i =0;i<audioList.Count;i++)
			{
				if(audioList[i].audioName== audioName)
				{
					Debug.Log("Playing S");
					audioList[i].audioSource.Play();
				}
			}
		}

	}

}
