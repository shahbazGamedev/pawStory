using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {
	public GameObject balloonItem;
	private float speed = 3;
	private float endLocation = 11;
	
	void Start () 
	{

    }

    void Update ()
	{
		transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;

		if(transform.position.y > endLocation)
		{
            Destroy(balloonItem);
            Destroy(gameObject);
        }
	}


	void OnMouseDown()
	{
		Rigidbody rb = balloonItem.GetComponent<Rigidbody> () as Rigidbody;
		rb.useGravity = true;
        BubbleGame.dogState = 1;

        if(balloonItem != null)
        {
            balloonItem.SetActive(true);
            balloonItem.transform.position = transform.position;
            BubbleGame.ballonItemsToCollectionQueue.Enqueue(balloonItem);
            Debug.Log("Queue count : " + BubbleGame.ballonItemsToCollectionQueue.Count);


        }

        Destroy(gameObject);
	}


	public void setBalloonItem(GameObject t_balloonItem)
	{
	    balloonItem = t_balloonItem;
		balloonItem.transform.position = transform.position;
	}

}
