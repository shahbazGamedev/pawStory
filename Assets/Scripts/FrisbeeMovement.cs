using UnityEngine;
using System.Collections;

public class FrisbeeMovement : MonoBehaviour {
	private Vector3 fp;
	private Vector3 lp;
	private float dragDistance;
	public float power;
	private Vector3 frisbeePosition;
	public float factor =34f;
	public bool canThrow= true;
	public bool returned= true;
	public Rigidbody rb;
	public int turn;





	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Time.timeScale = 1;
		dragDistance = Screen.height*50/100;
		frisbeePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(returned && turn==0){    
			 playerLogic(); 

			}
	
	}
	void playerLogic()
	{
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				fp = touch.position;
				lp = touch.position; 
			}
			
			if (touch.phase == TouchPhase.Ended)
			{
				lp = touch.position; 
				if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
				{   
					float x = (lp.x - fp.x) / Screen.height * factor; 
					float y = (lp.y-fp.y)/Screen.height*factor;


					if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
					{      
						
						if ((lp.x>fp.x) && canThrow)  
						{   
							rb.AddForce((new Vector3(x,10,15))*power); 
						}
						else
						{   
							rb.AddForce((new Vector3(x,10,15))*power);
						}
					}
					else
					{  
						if (lp.y>fp.y)  
						{   
							rb.AddForce((new Vector3(x,y,15))*power);
						}
						else
						{   
							Debug.Log ("down swipe not suppourted");
						}
					}
				}
				canThrow = false;
				returned = false;
				StartCoroutine(ReturnBall());
}
		}
	}
		
			IEnumerator ReturnBall() 
			{
				yield return new WaitForSeconds(5.0f);  //set a delay of 5 seconds before the ball is returned
				rb.velocity = Vector3.zero;   //set the velocity of the ball to zero
				rb.angularVelocity = Vector3.zero;  //set its angular vel to zero
				transform.position = frisbeePosition;   //re positon it to initial position
				//take turns in shooting
				if(turn==1)      
					turn=0;    
				else if(turn==0)
					turn=1;
				canThrow =true;     //set the canshoot flag to true
				returned = true;     //set football returned flag to true as well
			}
		}
