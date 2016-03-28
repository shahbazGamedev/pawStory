using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//	cube :rL =30, LL = -30

public class BonePullManager : MonoBehaviour {
    //Game vars
    private Animator animator;
    private float timeElapsed;
    public float timeLimit = 10f;
    private bool hasCurrentTurnEnded=true; //Used to start first/next turn of bone pulling

    //dog vars
    private bool isDogRotating;
    public GameObject whiteBone;
    public GameObject dog;

    //rotation vars
    private float startTime_Rotation, startTime_SliderMove;
    private float distanceCovered;
    private float rotationSpeed=18f;
    private float currentDogAngle;
    private float maxDogRotationAngle = 35f;

    //Slider vars
    public Slider slider;
    private float sliderSpeed = 0.5f/2f;
    private bool hasSliderReachedEnd;

    // Use this for initialization
    void Start () {
        animator = dog.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!hasCurrentTurnEnded)
        {
			rotateToRight();
            moveSliderLeft();
        }

        //bone pulling animation
        if (timeElapsed < timeLimit)
        {
            animator.SetFloat("bonehold", 1f);
            timeElapsed += Time.deltaTime;
            // Debug.Log("holding");
        }
        else
        if (timeElapsed > timeLimit)//dog lose animation
        {
            whiteBone.SetActive(false);
            animator.SetFloat("lose", 1f);//dog lost
            //Debug.Log("lost");
        }

    }

    void OnMouseDown()
    {

        if (hasCurrentTurnEnded && !isDogRotating)
        {
            hasCurrentTurnEnded = false;
            isDogRotating = true;
            startTime_Rotation = Time.time;
            startTime_SliderMove = Time.time;
            Debug.Log("started rotating");
        }

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
        if (isDogRotating)
        {
            distanceCovered = (Time.time - startTime_Rotation) * rotationSpeed;
            //Debug.Log("distance covered = " + distanceCovered + " , distDiff = " + distanceCovered / 30f);
            currentDogAngle = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, maxDogRotationAngle, 0), distanceCovered / maxDogRotationAngle).y;
            this.transform.rotation = Quaternion.Euler(0, currentDogAngle, 0);

            if (currentDogAngle == maxDogRotationAngle)
                isDogRotating = false;
        }
    }

    void rotateToLeft()
    {
        if (isDogRotating)
        {
            distanceCovered = (Time.time - startTime_Rotation) * rotationSpeed;
            //Debug.Log("distance covered = " + distanceCovered + " , distDiff = " + distanceCovered / 30f);
            currentDogAngle = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -maxDogRotationAngle, 0), distanceCovered / maxDogRotationAngle).y;
            this.transform.rotation = Quaternion.Euler(0, currentDogAngle, 0);

            if (currentDogAngle == -maxDogRotationAngle)
                isDogRotating = false;
        }
    }

    //Movement from center to left
    void moveSliderLeft()
    {

        if (!hasSliderReachedEnd)
        {
            slider.value = 0.5f - sliderSpeed * (Time.time - startTime_SliderMove);
            if (slider.value == 0)
                hasSliderReachedEnd = true;
        }
    }

    //Movement from center to right
    void moveSliderRight()
    {

        if (!hasSliderReachedEnd)
        {
            slider.value = 0.5f + sliderSpeed * (Time.time - startTime_SliderMove);
            if (slider.value == 1)
                hasSliderReachedEnd = true;
        }
    }
}
