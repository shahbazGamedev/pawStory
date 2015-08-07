using UnityEngine;
using System.Collections;

public class GlobalValues : MonoBehaviour {
	public static GlobalValues instanceRef;
	public int baloonsAtScene;
	public int playerScore;

	void Awake(){
		Debug.Log ("Hello");
		DontDestroyOnLoad (this);
		instanceRef = this;
	}


	void Start () {
		Reset ();
	}


	void Update () {
	}


	public void Reset(){
		playerScore = 0;
		baloonsAtScene = 0;
	}
}
