/**
Script Author : Vaikash
Description   : Manages Ball Behaviour - Petting
**/

using UnityEngine;
using System.Collections;

public class PettingBall : MonoBehaviour
{
    public GameObject dogRef;
    public bool isActive;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        Physics.IgnoreCollision(dogRef.GetComponent<Collider>(), GetComponent<Collider>());
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if(isActive)
            {
                Debug.Log("PuppyReact");
                PettingManager.instRef.puppyReactBall = true;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            if (isActive)
            {

            }
        }
    }
}
