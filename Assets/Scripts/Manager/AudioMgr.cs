using UnityEngine;
using System.Collections;

public class AudioMgr : MonoBehaviour {
	
	private static AudioMgr _instance = null;
	public static AudioMgr instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<AudioMgr>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}


	bool isSFXOn = true;
	public bool IsSFXOn
	{
		get
		{
			return isSFXOn;
		}
		set
		{
			isSFXOn = value;
		}
	}
	bool isMusicOn = true;
	public bool IsMusicOn
	{
		get
		{
			return isMusicOn;
		}
		set
		{
			isMusicOn = value;
		}
	}


	void Awake() 
	{
		if(_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}

	
	void Init()
	{
	}


	void Start () 
	{
	}


	void Update () 
	{
	}


	void PlaySFX()
	{
		if (IsSFXOn) 
		{

		}
	}


	void PlayMusic()
	{
		if (IsMusicOn) 
		{

		}
	}
}

