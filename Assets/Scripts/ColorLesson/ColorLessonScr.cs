using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorLessonScr : MonoBehaviour
{
	public GameObject DogObj;
	public Transform StartPos;
	public List<GameObject> BlockList;
	public List<GameObject> PickupList;
	public GameObject PickupsParentObj;
	public Text ScoreText;
	public GameObject GameOverScreenObj;
	public Text GameOverText;

	int dogSpeed = 10;
	float startZPos = 10.0f;
	int curLane = 2;
	int TotalLanes = 3;
	int TotalQuestions = 10;
	List<int> Questions;
	int curQuestion = 0;
	int ScoreVal = 0;
	float pickupGap = 5.0f;
	bool isGameOver = false;


	void Start ()
	{
		OnRestartGame ();
	}


	void Update ()
	{
		if(!isGameOver)
			DogObj.transform.position += new Vector3(0, 0, Time.deltaTime * dogSpeed);
	}


	void OnTriggerEnter(Collider other)
	{
		print(" ColorLessonScr trigger" + other.name);
		if (other.name.Contains ("Ans")) 
		{
			ScoreVal++;
		}
		ScoreText.text = ScoreVal + " / "  + TotalQuestions;
		if(!other.name.Contains("Trigger"))
			curQuestion++;

		if (curQuestion >= TotalQuestions) 
		{
			isGameOver = true;
			GameOverScreenObj.SetActive(true);
			GameOverText.text = "Collected " + ScoreVal +  " out of " + TotalQuestions;
		}
	}


	void LoadPickups()
	{
		int i = 0;
		Questions = new List<int> ();
		while ( i < TotalQuestions)
		{
			int newQuestion = Random.Range(0, PickupList.Count);
			Questions.Add(newQuestion);
			int prevQuestion = newQuestion - 1;
			int nextQuestion = newQuestion + 1;
			if(prevQuestion < 0)
				prevQuestion = PickupList.Count - 1;
			if(nextQuestion >= PickupList.Count)
				nextQuestion = 0;

			int newLane = Random.Range(0, TotalLanes);
			GameObject curObj = GameObject.Instantiate(PickupList[newQuestion]) as GameObject;
			curObj.transform.position = new Vector3(newLane, 0.9f, startZPos + i * pickupGap);
			curObj.transform.localScale = new Vector3(1f, 1f, 1f);
			curObj.transform.parent = PickupsParentObj.transform;
			curObj.name = PickupList[newQuestion].name + "_" + i + "_1" + "Ans";
			newLane++;
			if(newLane >= TotalLanes)
				newLane = 0;

			curObj = GameObject.Instantiate(PickupList[prevQuestion]) as GameObject;
			curObj.transform.position = new Vector3(newLane, 0.9f, startZPos + i * pickupGap);
			curObj.transform.parent = PickupsParentObj.transform;
			curObj.name = PickupList[prevQuestion].name + "_" + i + "_2";
			newLane++;
			if(newLane >= TotalLanes)
				newLane = 0;

			curObj = GameObject.Instantiate(PickupList[nextQuestion]) as GameObject;
			curObj.transform.position = new Vector3(newLane, 0.9f, startZPos + i * pickupGap);
			curObj.transform.parent = PickupsParentObj.transform;
			curObj.name = PickupList[nextQuestion].name + "_" + i + "_3";

			i++;
		}
	}


	public void OnLeft()
	{
		if (curLane > 1) 
		{
			curLane--;
			DogObj.transform.position -= new Vector3 (1, 0, 0);
		}
	}


	public void OnRight()
	{
		if (curLane < TotalLanes) 
		{
			curLane++;
			DogObj.transform.position += new Vector3 (1, 0, 0);
		}
	}


	public void OnRestartGame()
	{
		dogSpeed = 5;
		startZPos = 10.0f;
		curLane = 2;
		TotalLanes = 3;
		TotalQuestions = 10;
		curQuestion = 0;
		ScoreVal = 0;
		pickupGap = 5.0f;
		isGameOver = false;

		foreach (Transform child in PickupsParentObj.transform) 
			GameObject.Destroy(child.gameObject);

		LoadPickups ();

		GameOverScreenObj.SetActive(false);
		ScoreText.text = ScoreVal + " / "  + TotalQuestions;

		DogObj.transform.position = StartPos.position;
	}


	public void OnHome()
	{
		GameMgr.instance.LoadScene (GlobalConst.Scene_MainMenu);
	}
}
