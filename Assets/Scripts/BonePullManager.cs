using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//	cube :rL =30, LL = -30

public class BonePullManager : MonoBehaviour {

    private Animator animator;
    private float timeElapsed;
    private bool isHolding=true;

    public float timeDuration = 10f;
    public GameObject whiteBone;
    public GameObject dog;
	
	//dog vars
	private bool isRotating;

    //rotation vars
    private float currentAngle;
    private float startTime_Rotation;
    private float distanceCovered;
    private float rotationSpeed=15f;

    //ui
    public Slider slider;

    // Use this for initialization
    void Start () {
        animator = dog.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (isHolding)
        {
            if (isRotating)
            {
                rotateToLeft();
            }

            if (timeElapsed < timeDuration)
            {
                animator.SetFloat("bonehold", 1f);
                timeElapsed += Time.deltaTime;
               // Debug.Log("holding");
            }
            else
            if (timeElapsed > timeDuration)
            {
                whiteBone.SetActive(false);
                animator.SetFloat("lose", 1f);//dog lost
                //Debug.Log("lost");
                isHolding = false;
            }
        }

    }

    void OnMouseDown()
    {
        /*         if(!isHolding)
                    isHolding = true; */

        isRotating = true;
        startTime_Rotation = Time.time;
        Debug.Log("started rotating");
    }

    void OnMouseUp()
    {
/*         if (isHolding)
        {
            animator.SetFloat("win", 1f);//dog wins
            isHolding = false;
        } */
    }
	
	void rotateToRight()
	{
       distanceCovered = (Time.time - startTime_Rotation) * rotationSpeed;
       Debug.Log("distance covered = " + distanceCovered+" , distDiff = "+ distanceCovered / 30f);
       currentAngle = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 30f, 0), distanceCovered/30f ).y;
       this.transform.rotation = Quaternion.Euler(0, currentAngle, 0);

       
    }

    void rotateToLeft()
    {
        distanceCovered = (Time.time - startTime_Rotation) * rotationSpeed;
        Debug.Log("distance covered = " + distanceCovered + " , distDiff = " + distanceCovered / 30f);
        currentAngle = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -30f, 0), distanceCovered / 30f).y;
        this.transform.rotation = Quaternion.Euler(0, currentAngle, 0);
    }
}
