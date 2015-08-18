using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TugOfWarManager : MonoBehaviour {
	public GameObject win;
	public GameObject gameOver;
	public GameObject dog;
	public GameObject pulley;
	public Text winCondition;
	public float speed;
	private Animator dogAnim;
	Rigidbody rb;


	void Start () 
	{
		win.SetActive(false);
		gameOver.SetActive(false);
	    rb=GetComponent<Rigidbody>();
		dogAnim=GetComponent<Animator>();
	}
	

	void Update ()
	{

	}


	void FixedUpdate()
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
			dog.SetActive(false);
			gameOver.SetActive(true);
			Debug.Log ("win");
			win.SetActive(true);
			winCondition.text="PLAYER WINS";
			pulley.SetActive(false);

		}
	if(other.gameObject.tag=="LoseLine")
		{
			dog.SetActive(false);
			gameOver.SetActive(true);
			Debug.Log ("lose");
		    win.SetActive(true);
		    winCondition.text="PUPPY WINS";
		    pulley.SetActive(false);
		}
	}


	public void ReStart()
	{
		Application.LoadLevel("TugOfWar");
	}


	public void MainMenu()
	{
		Application.LoadLevel("MainMenu");
	}
}
