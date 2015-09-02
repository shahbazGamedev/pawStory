/**
Script Author : Vaikash 
Description   : Emulates a spring board
**/

using UnityEngine;
using System.Collections;

public class SpringBoard : MonoBehaviour
{
    #region Var

    public float springForce;

    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            // code to slingshot dog
            other.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, -0.2f).normalized * springForce, ForceMode.Impulse);
        }
    }
}
