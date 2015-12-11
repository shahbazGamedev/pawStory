/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FrisbeeMovement : MonoBehaviour
{

    //UI Elements
	public GameObject dummyFrisbee;

    //GamePlay Elements
	public GameObject dog;
	public GameObject frisbee;
	public float power;
    Vector3 endPos;
    Vector3 force;
    Vector3 startPos;
	Vector3 direction;
	float shootingAngle=45f;
	float distance;
	float angleRadians;
	float velocity;
	Vector3 frisbeeForce;
	Vector3 currentPosition;
	bool isJumping=false;
	bool detectLife;
	public bool canCollect;

    //Defaults
    Rigidbody rb;


    void Awake()
	{
	

	}


    void  Start ()
	{
		rb = GetComponent<Rigidbody>();
		currentPosition = transform.position;
	}


	void Update()
	{
		if (Vector3.Distance (dog.transform.position, frisbee.transform.position) < 2.5f && isJumping == false) 
		{
			direction = frisbee.transform.position - dog.transform.position;
            
			distance = direction.magnitude;
		    angleRadians = shootingAngle * Mathf.Deg2Rad;
			velocity = Mathf.Sqrt (distance * Physics.gravity.magnitude / Mathf.Sin (2 * angleRadians));
			frisbeeForce = velocity * direction.normalized;
			if(direction.x<0 && direction.x>-1f)
			{
			dog.GetComponent<DogMovementFrisbee> ().jumpingLeft(frisbeeForce);
			isJumping = true;
				
			}
			else if(direction.x>0 && direction.x<1f)
			{
				dog.GetComponent<DogMovementFrisbee> ().jumpingRight(frisbeeForce);
				isJumping=true;
				
			}
		}
        if(canCollect)
        {
           
            dog.GetComponent<DogMovementFrisbee>().FoulCollect();
        }
	}


	void FixedUpdate()
	{

	}


	void  OnMouseDown ()
	{
		startPos = Input.mousePosition;
		startPos.z = transform.position.z - Camera.main.transform.position.z;
		startPos = Camera.main.ScreenToWorldPoint(startPos);
	}


	void  OnMouseUp ()
	{

		endPos= Input.mousePosition;
		endPos.z = transform.position.z - Camera.main.transform.position.z;
		endPos = Camera.main.ScreenToWorldPoint(endPos);
		
		force= endPos - startPos;
		force.z = force.magnitude;
		force.Normalize();
		
		rb.AddForce(force * power);
		dog.GetComponent<DogMovementFrisbee>().chances= dog.GetComponent<DogMovementFrisbee>().chances+1;
		dummyFrisbee.SetActive(false);
		detectLife=true;


		StartCoroutine(  ReturnFrisbee() );
		isJumping=false;
	}


	IEnumerator ReturnFrisbee ()
	{
		yield return new WaitForSeconds(5.0f);
		
		transform.position = currentPosition;
		rb.velocity = Vector3.zero;
		GetComponent<MeshRenderer>().enabled=true;
		isJumping = false;
		dog.GetComponent<DogMovementFrisbee>().isMoving=true;
		dog.GetComponent<DogMovementFrisbee>().FrisbeeAttached.SetActive(false);
		GetComponent<Rigidbody>().detectCollisions=true;
		detectLife=false;
		dummyFrisbee.SetActive(true);
	}

		
	void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody)
		{
			GetComponent<MeshRenderer>().enabled=false;
			GetComponent<Rigidbody>().detectCollisions=false;
			dog.GetComponent<DogMovementFrisbee> ().Score++;
		    StartCoroutine(Dogmovement());
			if(dog.GetComponent<DogMovementFrisbee>().chances==dog.GetComponent<DogMovementFrisbee>().MaxChances || dog.GetComponent<DogMovementFrisbee>().Life==0)
			{
				
				StartCoroutine(EndGame());
			}
			dog.GetComponent<DogMovementFrisbee>().isSpawn=true;
			dog.GetComponent<DogMovementFrisbee>().FrisbeeAttached.SetActive(true);
		}
		if (collision.gameObject.tag=="floor" && detectLife==true)
		{
			
            canCollect = true;
			dog.GetComponent<DogMovementFrisbee>().Life--;
            

            if (dog.GetComponent<DogMovementFrisbee>().chances==dog.GetComponent<DogMovementFrisbee>().MaxChances || dog.GetComponent<DogMovementFrisbee>().Life==0)
			{
				
				StartCoroutine(EndGame());
			}
            
		}
	}


	IEnumerator Dogmovement()
	{
		yield return new WaitForSeconds(1.5f);
			dog.GetComponent<DogMovementFrisbee>().isMoving=true;

	}
	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(3.0f);
		dog.GetComponent<DogMovementFrisbee>().isGameover=true;
		
	}

}




