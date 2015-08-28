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
    public Vector3 jumpForce;
    public bool isGrounded;

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
        ComboManager.Jump += HandleDogJump;
        //dogRB.mass = 10;
    }

    public void OnDisable()
    {
        ComboManager.Jump -= HandleDogJump;
    }

    void Update()
    {
        if (updateAnim)
        {
            dogAnim.SetFloat("Speed", dogVelocity);
            updateAnim = false;
        }
        CamUp();
    }

    public void FixedUpdate()
    {
        if (runStart)
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

    // Camera Movement
    void CamUp()
    {
        var pos = Camera.main.transform.parent.transform.position;
        pos.z = dogRB.transform.position.z;
        Camera.main.transform.parent.transform.position = pos;
    }

    public void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    #region EventHandlers

    // Handle Jump Event
    void HandleDogJump()
    {
        if (isGrounded)
        {
            dogAnim.SetTrigger("Jump");
            GetComponent<Rigidbody>().AddForce(jumpForce, ForceMode.Impulse);
        }
    }


    #endregion EventHandlers
}
