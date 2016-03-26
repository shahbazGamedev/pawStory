using UnityEngine;
using System.Collections;

public class BonePullManager : MonoBehaviour {
    private Animator animator;
    private float timeElapsed;
    private bool isHolding;

    public float timeDuration = 10f;
    public GameObject whiteBone;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (isHolding)
        {
            if (timeElapsed < timeDuration)
            {
                animator.SetFloat("bonehold", 1f);
                timeElapsed += Time.deltaTime;
                Debug.Log("holding");
            }
            else
            if (timeElapsed > timeDuration)
            {
                whiteBone.SetActive(false);
                animator.SetFloat("lose", 1f);//dog lost
                Debug.Log("lost");
                isHolding = false;
            }
        }

    }

    void OnMouseDown()
    {
        if(!isHolding)
            isHolding = true;
    }

    void OnMouseUp()
    {
        if (isHolding)
        {
            animator.SetFloat("win", 1f);//dog wins
            isHolding = false;
        }
    }
}
