using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TugOfWarManager : MonoBehaviour {
	public GameObject win;
	public GameObject gameOver;
	public GameObject dog;
	public GameObject pulley;
	public bool gameStart;
	public Text winCondition;
	public Text play;
	public float speed;
	private Animator dogAnim;
	Rigidbody rb;
	float angle;
	float preAngle;
	int value;
	Vector2 normalizedPositions;




	void Start () 
	{
		gameStart=false;
		win.SetActive(false);
		gameOver.SetActive(false);
	    rb=GetComponent<Rigidbody>();
		dogAnim=GetComponent<Animator>();
	}
	

	void Update ()
	{
		if(gameStart==true)
		{
			normalizedPositions = new Vector2((Input.mousePosition.x/Screen.width-0.5f), ((Input.mousePosition.y/Screen.height)-0.5f));
			angle = Mathf.Atan2(normalizedPositions.y, normalizedPositions.x)*Mathf.Rad2Deg; 
			pulley.transform.eulerAngles = new Vector3( 0,0,angle);
			if(angle<0)
			{
				angle += 360;
				//Debug.Log (angle);
			}
			
			if(angle-preAngle>270)
			{
				value+=1;
				Debug.Log (value);
				Movement();
			}
			else
			{
				BackMovement();
			}
			preAngle=angle;
		}
	}


	void FixedUpdate()
	{

	}
    public void OnPointerDown()
	{
		gameStart=true;
		play.text="";
	}

	void Movement()
	{
		Debug.Log ("MovingForward");
		rb.AddForce(transform.forward*speed);
	}


	 void BackMovement()
	{
		Debug.Log("MovingBackwards");
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
		gameStart=false;
		value=0;
		play.text="Tap To Start";


	}


	public void MainMenu()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}
}
