using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public GameObject[] obstacleCollection;
	public PickUpCollection[] collectibleCollection;
	public spawnLocationHolder[] spawnPts;

	int dogPosition; // in predefined partition

	[System.Serializable]
	public struct spawnLocationHolder {
		public int partition;
		public SpwanPtData innerSpawnPos;
		public SpwanPtData midSpawnPos;
		public SpwanPtData outerSpawnPos;
	}

	[System.Serializable]
	public struct SpwanPtData {
		public GameObject position;
		public GameObject direction;
	}

	// Emulates hashtable in editor - PickUp
	[System.Serializable]
	public struct PickUpCollection {
		public PickUp key;
		public GameObject valueRef;
	}

	// PowerUp types
	public enum PickUp
	{
		SlowMotion,
		TurboRun,
		HurdleJump,
		HurdleSlide
	};

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	#region Coroutines

	// Spawn random objects at all spawnPts
	IEnumerator FillAllSpawnPts()
	{
		yield return new WaitForFixedUpdate ();
		// spawning code here in while loop
	}

	#endregion
}
