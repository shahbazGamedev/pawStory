using UnityEngine;
using System.Collections;

public class InternationalTournament : MonoBehaviour {

    int dogAnimState = 0; // 0: Idle, 1: Idle 2, 2: Idle 3, 3: Run 
    public Animator DogAnimator;
    private bool waitForTap = true;

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
                }
            }
#endif
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                waitForTap = false;
                dogAnimState = 3;
                DogAnimator.SetInteger("DogAnimState", dogAnimState);
            }
        }
        else
        {
                
        }
	}


    public void OnRestartGame()
    {
        dogAnimState = 0;
        waitForTap = true;
        DogAnimator = gameObject.GetComponent<Animator>();
    }

}
