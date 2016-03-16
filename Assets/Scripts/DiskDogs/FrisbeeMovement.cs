/**
Script Author : Srivatsan 
Description   : Dog Frisbee movement
**/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FrisbeeMovement : MonoBehaviour
{
    public static FrisbeeMovement instRef;

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
	float frisbeeVelocity;
	Vector3 frisbeeForce;
	Vector3 currentPosition;
	bool isJumping=false;
	bool detectLife;
	public bool canCollect;
    string layerName;
    public float curveAmount;


    //Defaults
    Rigidbody rb;


    void Awake()
	{
        instRef = this;

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
			frisbeeVelocity = Mathf.Sqrt (distance * Physics.gravity.magnitude / Mathf.Sin (2 * angleRadians));
			frisbeeForce =frisbeeVelocity * direction.normalized;
			if(direction.x<0 && direction.x>-1f)
			{
			DogMovementFrisbee.instRef.jumpingLeft(frisbeeForce);
			isJumping = true;
				
			}
			else if(direction.x>0 && direction.x<1f)
			{
                DogMovementFrisbee.instRef.jumpingRight(frisbeeForce);
				isJumping=true;
				
			}
		}
        if(canCollect)
        {

            DogMovementFrisbee.instRef.FoulCollect();
        }
	}


	void FixedUpdate()
	{
        Vector3 sideDir = Vector3.Cross(transform.up, rb.velocity).normalized;
        rb.AddForce(sideDir * curveAmount);


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
        Debug.Log(force * power);
       // DogMovementFrisbee.instRef.chances= DogMovementFrisbee.instRef.chances +1;
		dummyFrisbee.SetActive(false);
		detectLife=true;
        StartCoroutine(ReturnFrisbee());
        isJumping=false;
	}


	IEnumerator ReturnFrisbee ()
	{
		yield return new WaitForSeconds(5.0f);
		
		transform.position = currentPosition;
		rb.velocity = Vector3.zero;
		GetComponent<MeshRenderer>().enabled=true;
		isJumping = false;
        DogMovementFrisbee.instRef.isMoving =true;
        DogMovementFrisbee.instRef.FrisbeeAttached.SetActive(false);
		GetComponent<Rigidbody>().detectCollisions=true;
		detectLife=false;
		dummyFrisbee.SetActive(true);
        
        
        

        
	}

		
	void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody)
		{
            DogMovementFrisbee.instRef.chances = DogMovementFrisbee.instRef.chances + 1;
            GetComponent<MeshRenderer>().enabled=false;
			GetComponent<Rigidbody>().detectCollisions=false;
            DogMovementFrisbee.instRef.Score++;
		    StartCoroutine(Dogmovement());
			if(DogMovementFrisbee.instRef.chances == DogMovementFrisbee.instRef.MaxChances || DogMovementFrisbee.instRef.Life ==0)
			{
				
				StartCoroutine(EndGame());
			}
            DogMovementFrisbee.instRef.isSpawn =true;
            DogMovementFrisbee.instRef.FrisbeeAttached.SetActive(true);
		}
		if (collision.gameObject.tag=="floor" && detectLife==true)
		{
            DogMovementFrisbee.instRef.chances = DogMovementFrisbee.instRef.chances + 1;
            canCollect = true;
            DogMovementFrisbee.instRef.Life--;
            

            if (DogMovementFrisbee.instRef.chances== DogMovementFrisbee.instRef.MaxChances || DogMovementFrisbee.instRef.Life ==0)
			{
				
				StartCoroutine(EndGame());
			}
            
		}
	}


	IEnumerator Dogmovement()
	{
		yield return new WaitForSeconds(1.5f);
        DogMovementFrisbee.instRef.isMoving =true;

	}
	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(3.0f);
        DogMovementFrisbee.instRef.isGameover =true;
		
	}

}




