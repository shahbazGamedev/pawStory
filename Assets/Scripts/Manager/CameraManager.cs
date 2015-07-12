using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraManager : MonoBehaviour
{

	public WebCamTexture webCamTex = null;
	public GameObject plane;

	public Image glass;
	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Script has been started");
		plane = GameObject.FindWithTag ("Player");
		
		webCamTex = new WebCamTexture ();
		//mCamera.requestedHeight =256;
		//mCamera.requestedWidth =256;
		 
		plane.GetComponent<RawImage>().texture= webCamTex;
		webCamTex.Play ();
		Debug.Log( Application.persistentDataPath );
	}


	public void MoveImage()
	{
		this.GetComponent<RectTransform>().position =  Input.mousePosition; 
	}
	public void OnDrag(BaseEventData Data)
	{
		Debug.Log("Dragging");
		PointerEventData data=(PointerEventData)Data;
		 
		Debug.Log(data.dragging);
		Vector3 screenPoint = new Vector3(data.position.x, data.position.y, 0f);
		glass.GetComponent<RectTransform>().position =screenPoint;
	}

	public void ResiezeImage()
	{

	}
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButtonDown(1))
		{

			Application.CaptureScreenshot(Application.persistentDataPath + "Screenshot2.png");
			Debug.Log("Screenshot captured");
		}
	}
}
