using UnityEngine;
using System.Collections;

public class GlobalValues : MonoBehaviour {
	public static GlobalValues instanceRef;
	public int baloonsAtScene;
	public int playerScore;

	void Awake()
	{
		Debug.Log ("Hello");
		DontDestroyOnLoad (this);
		instanceRef = this;

	}

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reset()
	{
		playerScore = 0;
		baloonsAtScene = 0;
	}
}
