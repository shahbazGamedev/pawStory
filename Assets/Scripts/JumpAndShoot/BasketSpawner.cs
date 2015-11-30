/**
Script Author : Vaikash 
Description   : Spawning of Baskets
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BasketSpawner : MonoBehaviour {
	public GameObject basketPrefab;
	GameObject dogRef;
	Vector3 startPosition;
	Quaternion startRotation;
	bool resetInProgress;
	DogManager dogManager;
	SpawnPts[] spawnPointList;
	public Transform basketHolder;

	public int score;
	public Text scoreText;

	public enum gameState
	{
		start,
		spawn,
		end,
		reset
	};

	public gameState currentGame;


	// Use this for initialization
	void Start () {
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		dogManager = dogRef.GetComponent <DogManager> ();
		startPosition = dogRef.transform.position;
		startRotation = transform.rotation;
		spawnPointList = FindObjectsOfType <SpawnPts>();
		currentGame = gameState.spawn;
	}
	
	// Update is called once per frame
	void Update () {

		// Keeps track of coroutine status
		if(!dogManager.isCoroutineOn)
		{
			resetInProgress = false;
		}

		// Updates game based on current game state
		switch(currentGame)
		{
		case gameState.spawn:
			{
				currentGame = gameState.start;
				GameObject currentInstance = (GameObject) Instantiate (basketPrefab, spawnPointList[Random.Range (0,spawnPointList.Count ())].transform.transform.position, Quaternion.identity);
				currentInstance.transform.parent = basketHolder;
				break;
			}
		case gameState.start:
			{
				break;
			}
		case gameState.end:
			{
				break;
			}
		case gameState.reset:
			{
				break;
			}
		default:
			{
				break;
			}
		}
		scoreText.text = "Score: " + score;
	}

	// Resets dog position onClick - reset button
	public void ResetDog()
	{
        //if(!resetInProgress) {
        //	resetInProgress = true;

        //	// Resets ball status in ShootManager
        //	dogRef.GetComponent <ShootManager> ().hasBall = true;

        //	StartCoroutine (dogManager.MoveToPosition (startPosition, startRotation));
        //	Destroyer ("Ball");
        //	Destroyer ("Basket");

        //	StartCoroutine (SetSpawnFlag ());
        //}
        Application.LoadLevel(Application.loadedLevel);
	}

	// Waits for dog to come to initial position to spawn basket
	IEnumerator SetSpawnFlag()
	{
		while (resetInProgress)
			yield return new WaitForFixedUpdate();
		currentGame = gameState.spawn;
	}

	// Destroys all objects with a particular tag in screen
	void Destroyer(string typeName)
	{
		foreach(GameObject currentObject in GameObject.FindGameObjectsWithTag (typeName))
		{
			Destroy (currentObject);
		}
	}
}
