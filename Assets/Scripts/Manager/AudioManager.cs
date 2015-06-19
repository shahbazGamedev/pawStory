using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
public class AudioManager : MonoBehaviour
{
	public static AudioManager audioManager = null;
	public List<AudioSource> audioSourceSFX;
	public float maxSFXVolume =1 , maxBGVolume;

	public bool muteSound;
	 
	void Awake()
	{
		audioManager = this;
		audioSourceSFX =  new List<AudioSource>(5);

	}


	public void MuteSound()
	{
		if(muteSound) return;
		else
		{
			foreach(AudioSource s in audioSourceSFX)
			{
				s.volume =0.0f;
			}
			muteSound = true;
			
			Debug.Log("Sound Muted");
		}
	}

	public void UnMuteSound()
	{
		if(!muteSound) return;
		else
		{
			foreach(AudioSource s in audioSourceSFX)
			{
				s.volume =maxSFXVolume;
			}
			muteSound = false;
			
			Debug.Log("Sound Unmuted");
		}
	}

	void MuteAllSounds()
	{
		foreach(AudioSource s in audioSourceSFX)
		{
			s.volume =0.0f;
		}
	}

	void UnMuteAllSounds()
	{
		foreach(AudioSource s in audioSourceSFX)
		{
			s.volume =maxSFXVolume;
		}
	}
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
