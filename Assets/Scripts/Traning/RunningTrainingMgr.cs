using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RunningTrainingMgr : MonoBehaviour 
{
	public GameObject panelGameOver;
	public GameObject pulley;
    public GameObject PanelGameScreen;
    private Animator dogAnim;
    bool gameStart;
    public bool canSpin;
	Rigidbody rb;
	float angle;
	float preAngle;
	float value;
    Vector2 normalizedPositions;
    float timer;
    public Text TxtTimer;
    public Text TxtGameOver;
    
    
   

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
        GameOver();
    }


	void FixedUpdate()
	{
        timer += Time.deltaTime;
        TxtTimer.text = "Time :" + (int)timer;
       
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
        panelGameOver.SetActive(true);
        TxtGameOver.text = "Traning Session Faild!!!";
        Time.timeScale = 0f;
        PanelGameScreen.SetActive(false);
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
				value+=5*Time.deltaTime;
                Debug.Log(value);
                
				
			}
           if(value>0.01f)
            {
                dogAnim.SetFloat("Walk", 0.8f);
               
            }
            if(value>3)
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
        Time.timeScale = 1;
        timer = 0f;
        dogAnim.SetFloat("Walk", 0f);
	    panelGameOver.SetActive(false);
        PanelGameScreen.SetActive(true);
		
	}


	public void MainMenu()
	{
		GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
	}

    void GameOver()
    {
        if(timer>30)
        {
            panelGameOver.SetActive(true);
            TxtGameOver.text = "Training Sesson Sucessful!!!";
            Time.timeScale = 0;
            PanelGameScreen.SetActive(false);

        }
        if(timer>5 && value==0)
        {
            panelGameOver.SetActive(true);
            TxtGameOver.text = "Training Sesson Failed!!!";
            PanelGameScreen.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
