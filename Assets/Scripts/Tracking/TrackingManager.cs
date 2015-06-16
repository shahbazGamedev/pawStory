using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TrackingManager : MonoBehaviour {
	List<Vector3> dragData;
	public GameObject lineRenderer;
	LineRenderer line;
	string layerName;
	Vector3 worldPoint;
 
	// Use this for initialization
	void Start () {
		dragData=new List<Vector3>();
		line = lineRenderer.GetComponent<LineRenderer>(); 
		line.SetWidth(0.4F, 0.4F);
		line.sortingLayerName = "Foreground";
	
	}
	
	// Update is called once per frame
	void Update () {

		if(dragData!=null)
		{

			line.SetVertexCount(dragData.Count);
			for(int i=0;i<dragData.Count;i++)
			{
				//Debug.Log(dragData.Count);
				line.SetPosition(i, dragData[i]);

			}
	}
	}

	public void OnBeginDrag(BaseEventData Data)
	{
		Debug.Log("onDrag");
		PointerEventData data=(PointerEventData)Data;
		if(dragData!=null)
			dragData.Clear();
	}

	public void OnEndDrag(BaseEventData data)
	{
		Debug.Log("offDrag");
	}

	public void OnDrag(BaseEventData Data)
	{
		Debug.Log("Dragging");
		PointerEventData data=(PointerEventData)Data;
		Vector3 screenPoint = new Vector3(data.position.x, data.position.y, 0f);

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(screenPoint);
		if (Physics.Raycast(ray, out hit, 200f))
		{
			layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
			if(layerName == "Floor")
			{
				worldPoint = hit.point+(Vector3.up);
			}
		dragData.Add(worldPoint);
	}
}
}
