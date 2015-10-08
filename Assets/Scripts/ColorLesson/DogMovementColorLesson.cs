/**
Script Author : Srivatsan 
Description   : Color Lesson Dog Movements
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
    
	


	
	void Start () {
		rb=GetComponent<Rigidbody>();
        isMoving = false;
		dogAnim = GetComponent<Animator>();


	
	}
	

	void Update ()
    {

        distance = Vector3.Distance(Target.position, transform.position);
        if (distance > 2f && isMoving == true)
        {

            Debug.Log(distance);
            transform.LookAt(Target);
            dogAnim.SetFloat("Walk", 1f);
            float step = Speed * Time.deltaTime;
            rb.MovePosition(Vector3.MoveTowards(transform.position, Target.position, step));
        }
        if (distance < 2f)
        {
            isMoving = false;

            dogAnim.SetFloat("Walk", 0f);

        }

    }
	void FixedUpdate()
	{




    }
       
       

    

	

	public void RedMove()
	{
		Target=red;


        isMoving = true;
		
			

	}
	public void BlueMove()
	{
		Target=blue;

        isMoving = true;
	

	}
	public void GreenMove()
	{
		Target=green;

        isMoving = true;
	}
	public void YellowMove()
	{
	    Target=yellow;
        isMoving = true;
	}
	//void Movement()
	//{

 //       distance = Vector3.Distance(Target.position, transform.position);
 //       if (distance < 3f)
 //       {

 //           Debug.Log(distance);
 //           transform.LookAt(Target);
 //           dogAnim.SetFloat("Walk", 1f);
 //           float step = Speed * Time.deltaTime;
 //           rb.MovePosition(Vector3.MoveTowards(transform.position, Target.position, step));
 //       }
 //       if (distance < 2f)
 //       {
 //           isMoving = false;

 //           dogAnim.SetFloat("Walk", 0f);

 //       }





    
  
    	
}
	



