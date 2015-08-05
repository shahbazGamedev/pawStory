/**
Script Author : Vaikash 
Description   : Takes care of obstacle spawning in Agility
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	public GameObject[] obstacleCollection;
	public GameObject[] collectibleCollection;
	public spawnLocationHolder[] spawnPts;

	public int dogPosition; // in predefined partition
	int dogPrevPos;
	bool spawnOn;

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

	// Use this for initialization
	void Start () 
	{
		dogPrevPos = 10;
		spawnOn = true;
		StartCoroutine (Spawner ());

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// Actual spawining takes place here
	void spawn(int partition)
	{
		switch(partition)
		{
		case 0:
			{
				Debug.Log (partition);

				break;
			}
		case 2:
			{
				Debug.Log (partition);
				break;
			}
		case 4:
			{
				Debug.Log (partition);
				break;
			}
		case 6:
			{
				Debug.Log (partition);
				break;
			}
		default:
			{
				//Debug.Log (partition);
				break;
			}
		}
	}

	#region Coroutines

	// Spawn random objects at near spawnPts based on dog position
	public IEnumerator Spawner()
	{
		while(spawnOn)
		{
			
			yield return new WaitForFixedUpdate ();
			if(dogPosition!=dogPrevPos)
			{
				if(dogPosition%2==0)
				{
					spawn (dogPosition);
				}
				// spawn next 2 sets of spawn pts
			}
			dogPrevPos=dogPosition;
		}
		yield return null;
	}

	#endregion
}
