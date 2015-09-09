using UnityEngine;
using System.Collections;

public class NewFollowTraining : MonoBehaviour
{
    string layerName;
    public GameObject Cube;
    public GameObject Dog;
    
  
    void Start()
    {
       
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log("Hit");
                layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (layerName == "Floor")
                {
                   Cube.transform.position = hit.point + (Vector3.up * 0.01f);
                    Dog.GetComponent<Movement>().isMoving = true;

                }
            }
        }
    }
    public void OnRestart()
    {
        Application.LoadLevel("NewFollowTraining");
    }
   
}
