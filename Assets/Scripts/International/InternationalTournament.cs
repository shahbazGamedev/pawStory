using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InternationalTournament : MonoBehaviour {

    int dogAnimState = 0; // 0: Idle, 1: Idle 2, 2: Idle 3, 3: Run
    public bool isMoving;
    public bool canTap;
    public List<GameObject> Nodes;
    Animator DogAnimator;
    private bool waitForTap = true;
    Vector3 dogMoveOffset = Vector3.zero;
    float dogSpeed = 10;
    int curNode = 0;
    Rigidbody rb;
    Vector3 jumpForce;
    Vector3 highJump;
    public Text timer;
    private float Timer;
    int DogID;
    string layerName;
    public Text Txt_GameOver;
    public GameObject Gameover;
    

    void OnEnable()
    {
        EventMgr.GameRestart += OnRestartGame;
    }


    void OnDisable()
    {
        EventMgr.GameRestart -= OnRestartGame;
    }


	void Start () {
        OnRestartGame();
        rb = GetComponent<Rigidbody>();
        jumpForce = new Vector3(0f, 5f, 0f);
        highJump = new Vector3(0f, 9f, 0f);
        Timer = 0f;
        DogID = 1;
        Gameover.SetActive(false);
       
	}


    void Update()
    {
        if(isMoving && DogID == 1)
        {
            Movement();
        }
        Timer += Time.deltaTime;
        timer.text = "Total Time:" + (int) Timer;
        TargetSelector();
        //if (task1)
        //{
        //    Movement();
        //}
        //else if(tunnel)
        //{
        //    MovementTunnel();
        //}
        //if(pannel)
        //{
        //    MovementPannel();
        //}
        //if(temp)
        //{
        //    TempMove();
        //}
        //        if (waitForTap)
        //        {
        //#if UNITY_ANDROID || UNITY_IOS
        //            if (Input.touchCount > 0)
        //            {
        //                Touch touch = Input.touches[0];
        //                if (touch.phase == TouchPhase.Ended)
        //                {
        //                    waitForTap = false;
        //                    dogAnimState = 3;
        //                    DogAnimator.SetInteger("DogAnimState", dogAnimState);
        //                    curNode = 0;
        //                }
        //            }
        //#endif
        //            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0))
        //            {
        //                waitForTap = false;
        //                dogAnimState = 3;
        //                DogAnimator.SetInteger("DogAnimState", dogAnimState);
        //                curNode = 0;
        //            }
        //        }
        //    }
        //else
        //{
        //    // move the dog
        //    transform.LookAt(Nodes[curNode].transform, new Vector3(0, 1, 0));
        //    transform.position = Vector3.MoveTowards(transform.position, Nodes[curNode].transform.position, Time.deltaTime * dogSpeed);

        //    if (Vector3.Distance(transform.position, Nodes[curNode].transform.position) < 1f)
        //    {
        //        curNode++;

        //    }
        //               //transform.position += dogMoveOffset;
        //           }
        //}
    }


	private void LateUpdate()
	{
		if (!waitForTap) {
			//Camera.main.transform.position += dogMoveOffset;
		}
	}


    public void OnRestartGame()
    {
        dogAnimState = 0;
        Timer = 0f;
        waitForTap = true;
        DogAnimator = gameObject.GetComponent<Animator>();
    }
    void OnTriggerEnter(Collider coll)
    {
        
        if(coll.gameObject.tag=="Finish")
        {
            DogAnimator.SetTrigger("Jump");
            rb.AddForce(highJump, ForceMode.Impulse);
            Debug.Log("Working");
        }
        else if (coll.gameObject.tag == "target")
        {
            gameOver();
        }
        else
        {
            Debug.Log("ComingHere");
            rb.AddForce(jumpForce, ForceMode.Impulse);
            DogAnimator.SetTrigger("Jump");
        }
       
    }
    public void Movement()
    {
        dogAnimState = 3;
        DogAnimator.SetInteger("DogAnimState", dogAnimState);
        transform.LookAt(Nodes[curNode].transform, new Vector3(0, 1, 0));
        transform.position = Vector3.MoveTowards(transform.position, Nodes[curNode].transform.position, Time.deltaTime * dogSpeed);

        if (Vector3.Distance(transform.position, Nodes[curNode].transform.position) < 1f)
        {
            dogAnimState = 0;
            DogAnimator.SetInteger("DogAnimState", dogAnimState);
            isMoving = false;
            canTap = true;

        }
    }
    public void OnPointerDown()
    {
        if (canTap && DogID == 1)
        {
            curNode++;
            isMoving = true;
            Debug.Log(curNode);
            canTap = false;
        }
    }
   
    
    
    void TargetSelector()
          {
            if (Input.GetMouseButtonDown(0))
            {
           
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000f) && canTap)
                {
                    Debug.Log("Hit");
                    layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    if (layerName == "Floor" && DogID == 1)
                    {
                       Debug.Log("workingfine");
                    curNode++;
                    isMoving = true;
                    Debug.Log(curNode);
                    canTap = false;

                }
                
            }
            }
    }
    void gameOver()
    {
        Gameover.SetActive(true);
        Txt_GameOver.text = "Total Time:" + (int)Timer;
    }
  

}

