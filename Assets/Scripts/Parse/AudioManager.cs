using UnityEngine;
using System.Collections;


public enum SFXVAL
{

	jump =0,
	doubleJump,
	gameOver,
	gameWin,
	gameLost,
	buttonClick
}

public class AudioManager : MonoBehaviour 
{


	public static AudioManager instance =null;

	public AudioSource audioSource;
	public AudioSource audioSourceBg;
	public AudioClip  jump, doubleJump, gameOver,gameWin, gameLost, buttonClick;



	void Awake()
	{
		instance = this;
		Debug.Log ("Instance called");
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
			audioSource.PlayOneShot(buttonClick);
			break;

		case SFXVAL.jump:
			audioSource.PlayOneShot(jump);
			break;

		case SFXVAL.doubleJump:
			audioSource.PlayOneShot(doubleJump);
			break;

		case SFXVAL.gameLost:
			audioSource.PlayOneShot (gameLost);
			break;

		case SFXVAL.gameWin:
			audioSource.PlayOneShot (gameWin);
			break;

		case SFXVAL.gameOver:
			audioSource.PlayOneShot(gameOver);
			break;
		}
	}

	public void MusicToggle()
	{
		if(audioSourceBg!=null)
		audioSourceBg.mute = GlobalVariables.isMuted;
	}
	
	// Update is called once per frame
	void Update () 
	{
		MusicToggle (); // else call when doing mute...

	}
}
