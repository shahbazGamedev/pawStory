using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ColorTrainingMgr : MonoBehaviour
{

	public static ColorTrainingMgr instRef;

	public Image colorImage;
	public GameObject colorPanelUI, gameOverPanel, gameScreenPanel;
	public Text txt_gameOver;
	public GameObject[] toys;
	public Transform Dummy;
	public Text txt_chances;

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
	public bool red, green, blue, yellow;
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
		gameScreenPanel.SetActive (true);
		isMoving = false;

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
			dogAnim.SetFloat ("Walk", 0f);
			targetPos = dogStartPos;
			transform.LookAt (targetPos);
			float step = speed * Time.deltaTime;
			rb.MovePosition (Vector3.MoveTowards (transform.position, targetPos, step));
		}
			
		if (transform.position == dogStartPos)
		{
			transform.LookAt (Dummy);
		}

		if(score==4)
		{
			GameOver ();
		}

		SetColor();
		txt_chances.text = " Chances " + score + " /4 ";
	}
		
	void OnEnable()
	{
		EventMgr.GameRestart += RestartGame;
	}

	void OnDisable()
	{
		EventMgr.GameRestart -= RestartGame;
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
		isMoving = false;
		red = false;
		green = false;
		blue = false;
		yellow = false;
		colorIndex = Random.Range (0, 4);
		colorPanelUI.SetActive (true);
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
		gameScreenPanel.SetActive (false);
		gameOverPanel.SetActive (true);

		if (score == 4) 
		{
			txt_gameOver.text = "Session Success";
		}
		else 
		{
			txt_gameOver.text = "Session Failed";
		}

		Time.timeScale = 0f;
	}

	public void RestartGame()
	{
		Time.timeScale = 1;
		gameScreenPanel.SetActive (true);
		gameOverPanel.SetActive (false);
		score = 0;	
		ResetObjectPos ();
		GetRandomColor ();
	}

	void OnTriggerEnter(Collider col)
	{
		layerName = LayerMask.LayerToName (col.gameObject.layer);

		switch(layerName)
		{
			case "RedObject":
			ResetObjectPos ();
			break;

			case "GreenObject":
			ResetObjectPos ();
			break;
	
			case "BlueObject":
			ResetObjectPos ();
			break;

			case "YellowObject":
			ResetObjectPos ();
			break;

		case "CheckPoint":
			score += 1;
			GetRandomColor ();
			break;
		}
	}
}