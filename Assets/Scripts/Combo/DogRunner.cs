/**
Script Author : Vaikash 
Description   : Combo - Dog Movement Controller
**/

using UnityEngine;
using System.Collections;

public class DogRunner : MonoBehaviour
{
    #region Variables

    public static DogRunner instRef;

    public bool runStart;
    public float runSpeed;
    public float runDragFactor;
    public Vector3 runDirection;

    Rigidbody dogRB;
    Animator dogAnim;

    #endregion Variables

    public void Awake()
    {
        instRef = this;
        dogRB = GetComponent<Rigidbody>();
        dogAnim = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void FixedUpdate()
    {
        if(runStart)
        {
           // Moves dog using rigidbody
           dogRB.AddForce(runDirection * runSpeed * Time.deltaTime);
           dogRB.drag = dogRB.velocity.magnitude * runDragFactor;
        }
    }
}
