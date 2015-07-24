using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float transitionDuration =3f;
	public Transform target;
	public GameObject dog;
	public Transform StartPos;
	public Transform camera;




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ReturnCam();
	
	}

	public IEnumerator Transition()
	{

		float t = 0.0f; 
		Vector3 startingPos = transform.position;
		while (t < 1.0f) 
		{ 
		
			t += Time.deltaTime * (Time.timeScale/transitionDuration);
			transform.position = Vector3.Lerp(startingPos, target.position, t);
			yield return null;
		}
	}
	public void cameraMove()
	{
		if(dog.GetComponent<DockJump>().isCameraMove==true)
		{
			dog.GetComponent<DockJump>().isCameraMove=false;
			StartCoroutine(Transition());
		}

	}
	public void ReturnCam()
	{
		if(dog.GetComponent<DockJump>().isCamReturn==true)
			Debug.Log("returncam");
		camera.transform.position=StartPos.transform.position;
	}

}
