using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour {

    private float platformMovementSpeed=10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(ComboManager.gameRunning)
            movePlatform();
    }

    private void movePlatform()
    {
        //if (ComboManager.gameRunning)
        //{
            if (gameObject.transform.position.z < 12)
            {
                //move the platform towards left
                gameObject.transform.position += new Vector3(0, 0, Time.deltaTime * platformMovementSpeed);
            }
            else
            {
                //destroy the platform
                TrashMan.despawn(this.gameObject);
                Debug.Log("Destroyed this platform");
            }
        //}
    }
}
