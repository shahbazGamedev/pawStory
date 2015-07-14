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
}
