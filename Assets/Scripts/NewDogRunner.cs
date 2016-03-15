// Author : Harish 
// Paw adventure Backup runner scriptk

using UnityEngine;
using System.Collections;

public class NewDogRunner : MonoBehaviour 
{
	//Instance
	public static NewDogRunner instRef;

	public bool runStart;
	public bool isGrounded;
	public static bool updateAnim;
	public static bool gameOver;
	public static int Life;
			
	float dogVelocity;

	public GameObject BtnVideo;
	public Animator dogAnim;
	public Rigidbody dogRb;

	string layerName;
	//Dog Jump Velocity

	public float normalJumpForce;
	public float springJumpForce;

	void Awake () 
	{
		instRef = this;
		dogAnim = GetComponent<Animator> ();
		dogRb = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		if (runStart) 
		{
			dogVelocity = 1;
			SyncAnim ();
			if(Life==5 || GlobalVariables.distanceCovered>150f)
			{
				BtnVideo.SetActive(true);
			}
		}
	}

	public  void SyncAnim()
	{
		if (updateAnim)
		{
			dogAnim.SetFloat("Speed", dogVelocity);
			updateAnim = false;
		}
	}

	void Start()
	{
		BtnVideo.SetActive(false);
		ComboManager.StartGame += StartGame;
		gameOver = true;
	}

	void StartGame()
	{
		runStart = true;
		updateAnim = true;
		Invoke("ResetGameOverFlag", 2f);
	}

	public void OnDisable()
	{
		ComboManager.StartGame -= StartGame;
	}

	void ResetGameOverFlag()
	{
		gameOver = false;
	}

	public void HandleDogJump()
	{
		if (runStart && isGrounded)
		{
			dogAnim.SetTrigger ("Jump");
			dogRb.velocity = new Vector3(0, normalJumpForce, 0);
			dogRb.useGravity = true;
			isGrounded = false;
		}
	}

	public void GameOver()
	{
		gameOver = true;
		ComboManager.instRef.GameOver();
		runStart = false;
		dogVelocity = 0;
		updateAnim = true;
		SyncAnim();
		dogAnim.SetBool("GameOver",gameOver);
	}

	void OnTriggerEnter(Collider col)
	{
		layerName = LayerMask.LayerToName( col.gameObject.layer);

		switch (layerName) 
		{
		case "target":
			dogAnim.SetTrigger ("Fly");
			dogRb.velocity = new Vector3(0, springJumpForce, 0);
			break;

		case "loseLine":
			GameOver ();
			if (ComboManager.LifeCalc) 
			{
				Life += 1;
				ComboManager.LifeCalc = false;
			}
			Debug.Log (Life);
			break;
		}


//		if (col.gameObject.tag == "target")
//		{
//			dogAnim.SetTrigger ("Fly");
//			dogRb.velocity = new Vector3(0, springJumpForce, 0);
//		}
//		else if (col.gameObject.tag == "LoseLine") 
//		{
//			GameOver ();
//			if (ComboManager.LifeCalc) 
//			{
//				Life += 1;
//				ComboManager.LifeCalc = false;
//			}
//			Debug.Log (Life);
//		}
	}

	void OnCollisionEnter(Collision col)
	{
		layerName = LayerMask.LayerToName( col.gameObject.layer);

		switch (layerName) 
		{
		case "Platform":
			isGrounded = true;
			Debug.Log ("Grounded");
			break;
		}
//		if (col.gameObject.tag == "Platform") 
//		{
//			
//		}
	}		
}


