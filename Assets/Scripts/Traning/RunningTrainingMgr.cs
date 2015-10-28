using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RunningTrainingMgr : MonoBehaviour 
{
	public GameObject panelGameOver;
	public GameObject pulley;
    private Animator dogAnim;
    bool gameStart;
    public bool canSpin;
	Rigidbody rb;
	float angle;
	float preAngle;
	float value;
    Vector2 normalizedPositions;


	void Start () 
	{
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
		
	}
	public void OnPointerUp()
    {
        gameStart = false;
        value = 0f;
        dogAnim.SetFloat("Walk", 0f);
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
                
				
			}
           
			
			if(angle-preAngle>270)
			{
				value+=1*Time.deltaTime;
                Debug.Log(value);
                
				
			}
           if(value>0.1 && value<0.5)
            {
                dogAnim.SetFloat("Walk", 0.8f);
            }
            if(value>0.5)
            {
                dogAnim.SetFloat("Walk", 1.5f);
            }
			preAngle=angle;
            
		}
	}

	public void OnRestartGame()
	{
		gameStart=false;
		value=0;
	    panelGameOver.SetActive(false);
		
	}


	public void MainMenu()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}
}
