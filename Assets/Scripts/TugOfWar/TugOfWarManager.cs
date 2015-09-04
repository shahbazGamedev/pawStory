using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TugOfWarManager : MonoBehaviour 
{
	public GameObject panelGameOver;
	public GameObject pulley;
	public float speed;
	public Text winCondition;
	public Text play;
	private Animator dogAnim;
	public Vector3 dogPos;
	bool gameStart;
	Rigidbody rb;
	float angle;
	float preAngle;
	int value;
	Vector2 normalizedPositions;


	void Start () 
	{
		dogPos=transform.position;
		gameStart=false;
		panelGameOver.SetActive(false);
	    rb=GetComponent<Rigidbody>();
		dogAnim=GetComponent<Animator>();
	}


	void OnEnable() {
		EventMgr.GameRestart += OnRestartGame;
	}
	
	
	void OnDisable() {
		EventMgr.GameRestart -= OnRestartGame;
	}


	void Update ()
	{
		DogMovement();
	}


	void FixedUpdate()
	{

	}


    public void OnPointerDown()
	{
		gameStart=true;
		play.text="";
	}
	

	 void OnTriggerEnter(Collider other)
	{
	if(other.gameObject.tag=="Finish")
		{
		    panelGameOver.SetActive(true);
			winCondition.text="PLAYER WINS";
			pulley.SetActive(false);
			gameStart=false;

		}
	if(other.gameObject.tag=="LoseLine")
		{
		    panelGameOver.SetActive(true);
			winCondition.text="PUPPY WINS";
		    pulley.SetActive(false);
			gameStart=false;

		}
	}


	void DogMovement()
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
				rb.AddForce(0,0,-25*speed);
			}
			
			else
			{
				
				rb.AddForce(0,0,1*speed);
			}
			preAngle=angle;
		}
	}

	public void OnRestartGame()
	{
		gameStart=false;
		value=0;
		play.text="Tap To Start";
		panelGameOver.SetActive(false);
		pulley.SetActive(true);
		transform.position=dogPos;

	}


	public void MainMenu()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}
}
