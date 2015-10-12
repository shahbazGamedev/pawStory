/**
Script Author : Srivatsan 
Description   : Color Lesson Training
**/
using UnityEngine;
using System.Collections;

public class DogMovementColorLesson : MonoBehaviour {
	Transform Target;
	Rigidbody rb;
	public bool isMoving;
    public float Speed;
	private Animator dogAnim;
	public float speedDampTime;
	public Transform red;
	public Transform blue;
	public Transform green;
	public Transform yellow;
    public float distance;
    public GameObject PanelGameOver;
    string layerName;
    bool collectRed;
    public bool collectBlue;
    bool collectGreen;
    bool collectWhite;
    bool canClick;





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
        
    }


    void TargetCreator()
    {
        Debug.Log("yes yes yes");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log("Hit");
                layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (layerName == "Floor")
                {
                    collectBlue=true;
                    Debug.Log("steve");
                    
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
        if (canClick)
        {
            canClick = false;
            Target = red;
            isMoving = true;
        }
        
	}


    public void BlueMove()
    {
        if (canClick)
        {

            canClick = false;
            Target = blue;
            isMoving = true;
        }
    }


	public void GreenMove()
	{
        if (canClick)
        {

            canClick = false;
            Target = green;
            isMoving = true;
        }
	}


	public void YellowMove()
	{
        if (canClick)
        {
            canClick = false;
            Target = yellow;
            isMoving = true;
        }
	}


    void Gameover()
    {
        PanelGameOver.SetActive(true);
    }
}
	



