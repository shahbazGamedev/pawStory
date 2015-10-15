/**
Script Author : Srivatsan 
Description   : Color Lesson Training
**/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DogMovementColorLesson : MonoBehaviour {
	
	
	
    public float Speed;
    public float speedDampTime;
    public GameObject PanelGameOver;
    public GameObject redBtn;
    public GameObject blueBtn;
    public GameObject greenBtn;
    public GameObject whiteBtn;
    public Transform Target;
    public Transform red;
	public Transform blue;
	public Transform green;
	public Transform yellow;
    float distance;
    bool isMoving;
    bool collectRed;
    bool collectBlue;
    bool collectGreen;
    bool collectWhite;
    bool canClick;
    bool gameStart;
    Animator dogAnim;
    Rigidbody rb;
    string layerName;
    public Text teachingTxt;
    public GameObject TeachingPnl;
    





    void Start ()
    {
		rb=GetComponent<Rigidbody>();
        isMoving = false;
		dogAnim = GetComponent<Animator>();
        PanelGameOver.SetActive(false);
        canClick = true;
        teachingTxt.text = "Teach Green";
        blueBtn.SetActive(false);
        redBtn.SetActive(false);
        whiteBtn.SetActive(false);




    }
	

	void Update ()
    {

        distance = Vector3.Distance(Target.position, transform.position);
        if (distance > 2f && isMoving == true)
        {
            transform.LookAt(Target);
            dogAnim.SetFloat("Walk", 1f);
            float step = Speed * Time.deltaTime;
            rb.MovePosition(Vector3.MoveTowards(transform.position, Target.position, step));
        }
        if (distance < 2f)
        {
            isMoving = false;
            canClick = true;

            dogAnim.SetFloat("Walk", 0f);

        }
        TargetCreator();

    }


    void TargetCreator()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 300f))
            {
                Debug.Log("Hit");
                layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (layerName == "Toys")
                {
                    collectGreen = true;
                    Debug.Log("S");
                    Target = green;
                    collectBlue = false;
                    collectRed = false;
                    collectWhite = false;
                    //isMoving = true;

                }
                if (layerName == "Floor")
                {
                    collectBlue = true;
                    Debug.Log("t");
                    Target = blue;
                    collectGreen = false;
                    collectRed = false;
                    collectWhite = false;
                    //isMoving = true;

                }
                if (layerName == "Dog")
                {
                    collectWhite = true;
                    Debug.Log("e");
                    Target = yellow;
                    collectBlue = false;
                    collectRed = false;
                    collectGreen = false;
                    //isMoving = true;

                }
                if (layerName == "Back")
                {
                    collectRed = true;
                    Debug.Log("v");
                    Target = red;
                    collectBlue = false;
                    collectGreen = false;
                    collectWhite = false;

                    //isMoving = true;

                }
            }
        }
    }

    void FixedUpdate()
	{
    }
       

    public void RedMove()
	{
        if (canClick && collectRed)
        {
            canClick = false;
            
            isMoving = true;
        }
        
	}


    public void BlueMove()
    {
        if (canClick && collectBlue)
        {

            canClick = false;
            
            isMoving = true;
        }
    }


	public void GreenMove()
	{
        if (canClick && collectGreen)
        {

            canClick = false;
            
            isMoving = true;
        }
	}


	public void YellowMove()
	{
        if (canClick && collectWhite)
        {
            canClick = false;
            
            isMoving = true;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Toys")
        {
            greenBtn.SetActive(false);
            teachingTxt.text = "Teach Red";
            redBtn.SetActive(true);
           
        }
        if (other.gameObject.tag == "Basket")
        {
            redBtn.SetActive(false);
            teachingTxt.text = "Teach Blue";
            blueBtn.SetActive(true);

        }
        if (other.gameObject.tag == "MovePoints")
        {
            PanelGameOver.SetActive(true);
            TeachingPnl.SetActive(false);


        }
        if (other.gameObject.tag == "Ball")
        {
            blueBtn.SetActive(false);
            teachingTxt.text = "Teach white";
            whiteBtn.SetActive(true);

        }
    }
}
	



