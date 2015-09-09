using UnityEngine;
using System.Collections.Generic;

public class InternationalTournament : MonoBehaviour {

    int dogAnimState = 0; // 0: Idle, 1: Idle 2, 2: Idle 3, 3: Run 
    public Animator DogAnimator;
    private bool waitForTap = true;
    Vector3 dogMoveOffset = Vector3.zero;
    float dogSpeed = 10;
    public List<GameObject> Nodes;
    int curNode = 0;

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
	}
	

	void Update () {
        if (waitForTap)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Ended)
                {
                    waitForTap = false;
                    dogAnimState = 3;
                    DogAnimator.SetInteger("DogAnimState", dogAnimState);
                    curNode = 0;
                }
            }
#endif
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                waitForTap = false;
                dogAnimState = 3;
                DogAnimator.SetInteger("DogAnimState", dogAnimState);
                curNode = 0;
            }
        }
        else
        {
            // move the dog
            transform.LookAt(Nodes[curNode].transform, new Vector3(0, 1, 0));   
            transform.position = Vector3.MoveTowards(transform.position, Nodes[curNode].transform.position, Time.deltaTime * dogSpeed);
            
            if (Vector3.Distance(transform.position, Nodes[curNode].transform.position) < 0.1f)
            {
                curNode++;
                if (curNode >= Nodes.Count)
                {
                    curNode = 0;
                }
            }
            //transform.position += dogMoveOffset;
        }
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
        waitForTap = true;
        DogAnimator = gameObject.GetComponent<Animator>();
    }

}
