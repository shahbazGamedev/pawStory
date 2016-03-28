using UnityEngine;
using System.Collections;


public enum SFXVAL
{

	jump =0,
	doubleJump,
	gameOver,
	buttonClick
}

public class AudioManager : MonoBehaviour 
{


	public static AudioManager instance =null;

	public AudioSource audioSrc;
	public AudioSource audioSrcBg;
	public AudioClip  jump, doubleJump, gameOver, buttonClick;



	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
	
	}

	public void PlaySfx(SFXVAL curSfxVal)
	{
		if(GlobalVariables.isMuted)
			return;
		switch(curSfxVal)
		{
		case SFXVAL.buttonClick:
			audioSrc.PlayOneShot(buttonClick);
			break;

		case SFXVAL.jump:
			audioSrc.PlayOneShot(jump);
			break;

		case SFXVAL.doubleJump:
			audioSrc.PlayOneShot(doubleJump);
			break;

		case SFXVAL.gameOver:
			audioSrc.PlayOneShot(gameOver);
			break;
		}
	}

	public void MusicToggle()
	{
		audioSrcBg.mute = GlobalVariables.isMuted;
	}
	
	// Update is called once per frame
	void Update () 
	{
		MusicToggle (); // else call when doing mute...

	}
}
