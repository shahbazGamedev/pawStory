/**
Script Author : Srivatsan 
Description   : Color Lesson Training
**/
using UnityEngine;
using System.Collections;

public class DogMovementColorLesson : MonoBehaviour {
	
	
	
    public float Speed;
    public float speedDampTime;
    public GameObject PanelGameOver;
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





    void Start ()
    {
		rb=GetComponent<Rigidbody>();
        isMoving = false;
		dogAnim = GetComponent<Animator>();
        PanelGameOver.SetActive(false);
        canClick = true;




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
                    //isMoving = true;

                }
                if (layerName == "Floor")
                {
                    collectBlue = true;
                    Debug.Log("t");
                    Target = blue;
                    //isMoving = true;

                }
                if (layerName == "Dog")
                {
                    collectWhite = true;
                    Debug.Log("e");
                    Target = yellow;
                    //isMoving = true;

                }
                if (layerName == "Back")
                {
                    collectRed = true;
                    Debug.Log("v");
                    Target = red;
                    
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


    void Gameover()
    {
        PanelGameOver.SetActive(true);
    }
}
	



