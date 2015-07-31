using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TugOfWarManager : MonoBehaviour 
{
	public GameObject dogRef;

	private Animator dogAnimator;
	private Rigidbody dogRigidBody;

	public bool isPulleyTouched;
	public Image pulleyRef;

	void Awake()
	{
		dogAnimator =  dogRef.GetComponent<Animator>();
		dogRigidBody = this.GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () 
	{
	
	}

	public void TrackMouseMovement(BaseEventData data)
	{
		 
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log(pointerData);
	}

	public void PointerDown(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log("Selected");
		isPulleyTouched = true;
	}

	public void PointerUp(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		Debug.Log("Unselected");
		isPulleyTouched = false;
	}

	public void RotatePulley(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		if(isPulleyTouched)
		Debug.Log( Mathf.Atan(pointerData.delta.x) * Mathf.Rad2Deg);

		Vector3 rotate = pulleyRef.rectTransform.localEulerAngles;
		rotate.z = Mathf.Atan(pointerData.delta.x) * Mathf.Rad2Deg;//pointerData.delta.x;
		pulleyRef.rectTransform.localEulerAngles = rotate;

	}

	public void DragPulley(BaseEventData data)
	{
		PointerEventData pointerData = (PointerEventData ) data;
		if(isPulleyTouched)
		{
			Debug.Log(pointerData);
		}
	}
	 
	// Update is called once per frame
	void Update () 
	{
		pulleyRef.transform.Rotate(-Vector3.forward * Time.deltaTime * 100);
	}
}
