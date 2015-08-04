using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject dogRef;

	Vector3 majorAxis;
	Vector3 minorAxis;
	Vector3 target;
	Vector3 currentPosition;

	float x;
	float y;
	float xForward;
	float yForward;
	float forwardAlpha;

	public float height;
	public float alpha;
	public float centerX;
	public float centerY;
	public float moveSpeed;
	public float alphaFactor;
	public float forwardDist;
	public Vector3 forwardDirection;

	// Use this for initialization
	void Start () {
		dogRef = GameObject.FindGameObjectWithTag ("Player");
		dogRef.GetComponent <EllipseMovement> ().DogJustMoved += FollowDogMovement;
		majorAxis = dogRef.GetComponent <EllipseMovement> ().circuitLaneData [1].majorAxis.transform.position;
		minorAxis = dogRef.GetComponent <EllipseMovement> ().circuitLaneData [1].minorAxis.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Decouple event listener
	void OnDisable()
	{
		if(dogRef!=null)
			dogRef.GetComponent <EllipseMovement> ().DogJustMoved -= FollowDogMovement;
	}

	// Camera update
	void FollowDogMovement()
	{
		// Update ellipse angle
		alpha += Time.deltaTime * alphaFactor;

		// Calculate current position on ellipse based on angle(radians)
		x = centerX + majorAxis.x * Mathf.Cos (alpha * 0.0174f);
		y = centerY + minorAxis.z * Mathf.Sin (alpha * 0.0174f);

		// Calculate LookAt Position
		forwardAlpha=alpha+forwardDist;

		xForward = centerX + majorAxis.x * Mathf.Cos (forwardAlpha * 0.0174f);
		yForward = centerY + minorAxis.z * Mathf.Sin (forwardAlpha * 0.0174f);

		currentPosition=transform.position;
		target=new Vector3(x, currentPosition.y, y);
		forwardDirection=new Vector3(xForward, currentPosition.y, yForward);

		// Dog Movement
		transform.position= Vector3.MoveTowards (transform.position, target, moveSpeed * Time.deltaTime);
		transform.LookAt(forwardDirection);
	}

	void LateUpdate()
	{
		Camera.main.transform.position = Vector3.Scale (transform.position, new Vector3(1f,0f,1f)) + transform.rotation * new Vector3 (0f, 8f, -8f);
		var rotation = Quaternion.LookRotation(transform.position-Camera.main.transform.position);
		rotation = Quaternion.Euler (21.5f, rotation.eulerAngles.y, 0f);
		Camera.main.transform.rotation = rotation;
	}
}
