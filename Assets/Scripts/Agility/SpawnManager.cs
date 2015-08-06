﻿/**
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

	public container[] spacePartition;

	public GameObject cloneHolder;

	[System.Serializable]
	public struct container {
		public List<GameObject> partitionContainer;
	}

	public int dogPosition; // in predefined partition
	int prevDogPosition;
	int randomNumber;
	int dogPrevPos;
	public bool spawnOn;
	int partitionCount;

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
		partitionCount = spawnPts.Length;
		StartCoroutine (Spawner ());
		EllipseMovement.DogMovedNextPartition += UpdateDogPosition;


	}

	void OnDisable()
	{
		EllipseMovement.DogMovedNextPartition -= UpdateDogPosition;
		spawnOn = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// Random spawn code
	GameObject CheckForProbability(GameObject positionRef, GameObject[] collection)
	{
		randomNumber = Random.Range (0, collection.Length);
		var cloneInstance = (GameObject)Instantiate (collection [randomNumber], positionRef.transform.position, Quaternion.identity);
		cloneInstance.transform.parent = cloneHolder.transform;
		return cloneInstance;
	}

	// Event Handler for dogMovedNextPartition
	void UpdateDogPosition()
	{
		//Debug.Log ("Fired");
		dogPosition += 1;
		if (dogPosition > 7)
			dogPosition = 0;
	}

	// Destroy spawned objects when no longer seen
	void ClearObstacles(List<GameObject> spawnHodler)
	{
		foreach(var gameObj in spawnHodler)
		{
			Destroy (gameObj);
		}
		spawnHodler.Clear ();
	}


	// Actual spawining takes place here
	void spawn(int partition)
	{
		switch(partition)
		{
		case 0:
			{
//				Debug.Log (partition);

				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].innerSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].midSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].outerSpawnPos.position, obstacleCollection));

				if(spacePartition[partition + 6].partitionContainer.Count!=0)
				{
					ClearObstacles (spacePartition [partition + 6].partitionContainer);
				}

				break;
			}
		case 1:
			{
//				Debug.Log (partition);
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].innerSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].midSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].outerSpawnPos.position, obstacleCollection));

				break;
			}
		case 2:
			{
//				Debug.Log (partition);
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].innerSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].midSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].outerSpawnPos.position, obstacleCollection));

				if(spacePartition[partition-2].partitionContainer.Count!=0)
				{
					ClearObstacles (spacePartition [partition - 2].partitionContainer);
				}

				break;
			}
		case 3:
			{
//				Debug.Log (partition);
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].innerSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].midSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].outerSpawnPos.position, obstacleCollection));

				if(spacePartition[partition-2].partitionContainer.Count!=0)
				{
					ClearObstacles (spacePartition [partition - 2].partitionContainer);
				}

				break;
			}
		case 4:
			{
//				Debug.Log (partition);
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].innerSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].midSpawnPos.position, obstacleCollection));
				spacePartition [partition+2].partitionContainer.Add (CheckForProbability (spawnPts [partition+2].outerSpawnPos.position, obstacleCollection));

				if(spacePartition[partition-2].partitionContainer.Count!=0)
				{
					ClearObstacles (spacePartition [partition - 2].partitionContainer);
				}

				break;
			}
		case 5:
			{
//				Debug.Log (partition);

				if(spacePartition[partition-1].partitionContainer.Count!=0)
				{
					ClearObstacles (spacePartition [partition - 2].partitionContainer);
				}

				break;
			}
		case 6:
			{
//				Debug.Log (partition);
				spacePartition [0].partitionContainer.Add (CheckForProbability (spawnPts [0].innerSpawnPos.position, obstacleCollection));
				spacePartition [0].partitionContainer.Add (CheckForProbability (spawnPts [0].midSpawnPos.position, obstacleCollection));
				spacePartition [0].partitionContainer.Add (CheckForProbability (spawnPts [0].outerSpawnPos.position, obstacleCollection));

				if(spacePartition[partition-1].partitionContainer.Count!=0)
				{
					ClearObstacles (spacePartition [partition - 2].partitionContainer);
				}

				break;
			}
		case 7:
			{
//				Debug.Log (partition);
				spacePartition [1].partitionContainer.Add (CheckForProbability (spawnPts [1].innerSpawnPos.position, obstacleCollection));
				spacePartition [1].partitionContainer.Add (CheckForProbability (spawnPts [1].midSpawnPos.position, obstacleCollection));
				spacePartition [1].partitionContainer.Add (CheckForProbability (spawnPts [1].outerSpawnPos.position, obstacleCollection));

				if(spacePartition[partition-1].partitionContainer.Count!=0)
				{
					ClearObstacles (spacePartition [partition - 2].partitionContainer);
				}

				break;
			}
		case 10:
			{
//				Debug.Log (partition);
				spacePartition [0].partitionContainer.Add (CheckForProbability (spawnPts [0].innerSpawnPos.position, obstacleCollection));
				spacePartition [0].partitionContainer.Add (CheckForProbability (spawnPts [0].midSpawnPos.position, obstacleCollection));
				spacePartition [0].partitionContainer.Add (CheckForProbability (spawnPts [0].outerSpawnPos.position, obstacleCollection));

				spacePartition [1].partitionContainer.Add (CheckForProbability (spawnPts [1].innerSpawnPos.position, obstacleCollection));
				spacePartition [1].partitionContainer.Add (CheckForProbability (spawnPts [1].midSpawnPos.position, obstacleCollection));
				spacePartition [1].partitionContainer.Add (CheckForProbability (spawnPts [1].outerSpawnPos.position, obstacleCollection));

				break;
			}
		}
	}

	#region Coroutines

	// Spawn random objects at near spawnPts based on dog position //TODO change it to event handler
	public IEnumerator Spawner()
	{
		spawn (10);
		while(spawnOn)
		{
			
			yield return new WaitForFixedUpdate ();
			if(dogPosition!=dogPrevPos)
			{
					spawn (dogPosition);
				// spawn next 2 sets of spawn pts
			}
			dogPrevPos=dogPosition;
		}
		yield return null;
	}

	#endregion
}
