using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ColorTrainingMgr : MonoBehaviour
{

	public GameObject currentObjectToPick;

	public static ColorTrainingMgr instRef;

	public Image colorImage;
	public GameObject colorPanelUI, gameOverPanel; // gameScreenPanel removed
	public Text txt_gameOver;
	public Transform Dummy;
	public Text txt_chances;
	public GameObject[] toys;
	public GameObject colorToys;

	public Animator dogAnim;
	Rigidbody rb;

	Vector3[] objStartPos;
	Vector3 dogStartPos;
	public Vector3 targetPos;

	int colorIndex;
	float distance;
	string layerName;
	public float speed;
	public int score;
	public bool red, green, blue, yellow,falseTap, canThrow;
	public bool isMoving;

	void Awake()
	{
		instRef = this;
	}


	void Start()
	{
		dogAnim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		dogStartPos = new Vector3(0,0,0);
		objStartPos = new Vector3[toys.Length];
		//gameScreenPanel.SetActive (true);
		isMoving = false;
		canThrow = true;
		for(int i = 0; i < toys.Length; i++)
		{
			toys [i] = toys [i];
			objStartPos [i] = toys [i].transform.position;
		}
	}


	void Update()
	{
		distance = Vector3.Distance (targetPos, transform.position);

		if (isMoving && distance > 2f) 
		{
			transform.LookAt (targetPos);
			dogAnim.SetFloat ("Walk", 1f);
			float step = speed * Time.deltaTime;
			rb.MovePosition (Vector3.MoveTowards (transform.position, targetPos, step));
		}
			
		if(distance < 2f) 
		{
			targetPos = dogStartPos;
			transform.LookAt (targetPos);
			float step = speed * Time.deltaTime;
			rb.MovePosition (Vector3.MoveTowards (transform.position, targetPos, step));
		}
			
		if (transform.position == dogStartPos)
		{
			transform.LookAt (Dummy);
			dogAnim.SetFloat ("Walk", 0f);
		}

		if(score == 4)
		{
			GameOver ();
		}
			
		CheckFalseCall ();
		SetColor();
		txt_chances.text = " Chances " + score + " /4 ";

		if (!isMoving && falseTap) 
		{
			RestartGame ();
			falseTap = false;
		}
	}


	void OnEnable()
	{
		EventManager.GameRestart += RestartGame;
		//EventMgr.GameRestart += RestartGame;
	}


	void OnDisable()
	{
		EventManager.GameRestart -= RestartGame;
		//EventMgr.GameRestart -= RestartGame;
	}


	void CheckFalseCall()
	{
		if (isMoving && ThrowingObjects.instRef.transform.position == objStartPos [0] && falseTap) 
		{
			falseTap = false;
			isMoving = true;
		}
	
		if (isMoving && ThrowingObjects.instRef.transform.position == objStartPos [1] && falseTap)
		{
			falseTap = false;
			isMoving = true;
		} 

		if (isMoving && ThrowingObjects.instRef.transform.position == objStartPos [2] && falseTap) 
		{
			falseTap = false;
			isMoving = true;
		} 

		if (isMoving && ThrowingObjects.instRef.transform.position == objStartPos [3] && falseTap) 
		{
			falseTap = false;
			isMoving = true;
		} 
	}


	void SetColor()
	{
		if (colorIndex == 0) {
			colorImage.color = Color.red;
			red = true;
		} else if (colorIndex == 1) {
			colorImage.color = Color.green;
			green = true;
		} else if (colorIndex == 2) {
			colorImage.color = Color.blue;
			blue = true;
		} else if (colorIndex == 3) {
			colorImage.color = Color.yellow;
			yellow = true;
		}
	}


	public void GetRandomColor()
	{
		red = false;
		green = false;
		blue = false;
		yellow = false;
		colorIndex = Random.Range (0, 4);

	}


	public void ResetObjectPos()
	{
		for (int i = 0; i < toys.Length; i++) 
		{
			GameObject current = toys [i];
			current.SetActive (true);
			current.transform.position = objStartPos [i];
		}
	}


	public void GameOver()
	{
		canThrow = false;
		isMoving = false;
		//gameScreenPanel.SetActive (false);
		gameOverPanel.SetActive (true);

		if (score == 4) 
		{
			txt_gameOver.text = "Session Success";
		}
		else 
		{
			txt_gameOver.text = "Session Failed";
		}

	}


	public void RestartGame()
	{
		canThrow = true;
		isMoving = false;
		colorPanelUI.SetActive (true);
		score = 0;
		targetPos = Vector3.zero;
		transform.position = dogStartPos;
		//gameScreenPanel.SetActive (true);
		gameOverPanel.SetActive (false);
		ResetObjectPos ();
		GetRandomColor ();
	}
		
	void OnTriggerEnter(Collider col)
	{
		layerName = LayerMask.LayerToName (col.gameObject.layer);
		Debug.Log ("Touched1 :" + col.gameObject.name);

		if (currentObjectToPick.name == col.gameObject.name) 
		{
			ResetObjectPos ();
		}

		switch (layerName) {
		/*
				case "RedObject":
				//colorToys.SetActive (false);
				ResetObjectPos ();
				break;

				case "GreenObject":
				//colorToys.SetActive (false);
				ResetObjectPos ();
				break;
		
				case "BlueObject":
				//colorToys.SetActive (false);
				ResetObjectPos ();
				break;

				case "YellowObject":
				//colorToys.SetActive (false);
				ResetObjectPos ();
				break;
				*/
			case "CheckPoint":
			score += 1;
			//colorToys.SetActive (true);
			isMoving = false;
			colorPanelUI.SetActive (true);
			GetRandomColor ();
			break;
		}
	}
}