/**
Script Author : Vaikash 
Description   : Makeshift Trigger Manager for Initial Platform - ComboJump
**/

using UnityEngine;
using System.Collections;

public class InitPlatformTrigger : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && DogRunner.instRef.runStart)
        {
            Pooler.InstRef.Sleep(gameObject);
        }
    }

}
