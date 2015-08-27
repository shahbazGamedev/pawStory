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

    bool updateAnim;
    float dogVelocity;

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
        runStart = true;
        updateAnim = true;
    }

    void Update()
    {
        if(updateAnim)
        {
            dogAnim.SetFloat("Speed", dogVelocity);
            updateAnim = false;
        }
    }

    public void FixedUpdate()
    {
        if(runStart)
        {
           // Moves dog using rigidbody
           dogRB.AddForce(runDirection * runSpeed * Time.deltaTime);
           dogRB.drag = dogRB.velocity.magnitude * runDragFactor;
           dogVelocity = 1;
        }
        else
        {
            dogVelocity = 0;
        }
    }
}
