using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DogMovementTugOfWar : MonoBehaviour {
	Rigidbody rb;
	public float speed;
	private Animator dogAnim;
	public GameObject win;
	public GameObject gameOver;
	public GameObject dog;
	public GameObject pulley;
	public GameObject timer;
	public Text winCondition;
	// Use this for initialization
	void Start () 
	{
		win.SetActive(false);

		gameOver.SetActive(false);
	    rb=GetComponent<Rigidbody>();
		dogAnim=GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public void Movement()
	{
		rb.AddForce(transform.forward*speed);
	}

	public void BackMovement()
	{

		rb.AddForce(0,0,1*speed);
	}


	 void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="Finish")
		{
			timer.SetActive(false);
			dog.SetActive(false);
			gameOver.SetActive(true);
			Debug.Log ("win");
			win.SetActive(true);
			winCondition.text="PLAYER WINS";
			pulley.SetActive(false);

		}
		if(other.gameObject.tag=="LoseLine")
		{
			timer.SetActive(false);
			dog.SetActive(false);
			gameOver.SetActive(true);
			Debug.Log ("lose");
		    win.SetActive(true);
		    winCondition.text="PUPPY WINS";
		    pulley.SetActive(false);
		}
	}

	public void GameOver()
	{

		winCondition.text="Ended as Draw";
	}
}
