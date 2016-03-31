using UnityEngine;
using System.Collections;

public class PettingMainMenu : MonoBehaviour 
{

    private Animator puppyAnim;
    public float currentAnimState;
    private bool isAnimPlaying;

    private float timeElapsed, timeDuration=6f;

	// Use this for initialization
	void Start () {
        puppyAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (timeElapsed > timeDuration)
        {
            isAnimPlaying = false;
            timeElapsed = 0;

            puppyAnim.SetFloat("animationState", 0f);

        }

        timeElapsed += Time.deltaTime;

        if (Input.GetMouseButton(0) && !isAnimPlaying)
        {
            if (gameObject.name == "Dog")
            {
                isAnimPlaying = true;
                puppyAnim.SetFloat("animationState", currentAnimState);
                currentAnimState = Random.Range(1, 5);
                Debug.Log("Current animstate : " + currentAnimState);
            }
        }
       

    }


}
