using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public GameObject[] obstacleCollection;
	public PowerUpCollection[] collectibleCollection;
	public SpawnPoint[] spawnPts;

	// Emulates hashtable in editor - PowerUp
	[System.Serializable]
	public struct PowerUpCollection {
		public PowerUp key;
		public GameObject valueRef;
		public float coolDown;
	}

	// PowerUp types
	public enum PowerUp
	{
		SlowMotion,
		TurboRun,
		Freeze
	};

	// Use this for initialization
	void Start () 
	{
		spawnPts = FindObjectsOfType<SpawnPoint> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
