using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

public class LookAtMouse : MonoBehaviour

{
	public float speed;
	public GameObject headRef;
	bool isPlaying;

    void Start()
    {
        isPlaying = false;
    }

    public void OnPointerDown()
    {
        isPlaying = true;
    }
    void FixedUpdate()
    {
        if (isPlaying)
        {
            Vector3 upAxis = new Vector3(0, 0, -1);
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = transform.position.z;
            Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            transform.LookAt(mouseWorldSpace, upAxis);
            Debug.Log(mouseWorldSpace);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            
        }
    }
	}