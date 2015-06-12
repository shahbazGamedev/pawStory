using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class FrisbeeMovement : MonoBehaviour {
	
	Vector2 swipeBegin;		
	Vector2 swipeEnd;
	public  float speed;
	Rigidbody rb;
	public float curveAmount;
	public float height;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 sideDir = Vector3.Cross(transform.up, rb.velocity).normalized;
		rb.AddForce(sideDir * curveAmount);
	}
	public void OnPointerDown(BaseEventData  data)
	{
		Debug.Log("Begins");
		PointerEventData e=(PointerEventData) data;
		swipeBegin=e.position;
	}
	
	public void OnPointerUp(BaseEventData  data)
	{
		Debug.Log("Ends");
		PointerEventData e=(PointerEventData) data;
		swipeEnd=e.position;
		detectSwipe();
	}
	void detectSwipe()
	{
		Debug.Log ("swiped");
		Vector2 direction = swipeEnd - swipeBegin;
		Vector3 newDirection = new Vector3 (direction.x, height, direction.y);
		Vector3 velocity = newDirection * speed;
		rb.velocity = velocity;
	}
}
