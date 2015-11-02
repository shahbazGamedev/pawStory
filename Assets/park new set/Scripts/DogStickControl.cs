using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DogStickControl : MonoBehaviour
{

    Vector3 moveDirection;
    Vector3 forward;
    Animator dogAnim;
    Rigidbody dogRb;
    CharacterController cc;

    public float dragFactor;
    public float moveSpeed;
    //public float rotationSpeed;

    float h;
    float v;

    public void OnEnable()
    {
        dogAnim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //dogRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Cache the inputs.
        h = CrossPlatformInputManager.GetAxis ("Horizontal");
        v = CrossPlatformInputManager.GetAxis ("Vertical");

        MovementManagement(-v, h);
    }

    void MovementManagement(float horizontal, float vertical)
    {

        moveDirection = new Vector3(horizontal, 0f, vertical);

        

        // If there is some axis input...
        if (horizontal != 0f || vertical != 0f)
        {
            // ... set the players rotation and set the speed parameter to 5.5f.
            //Rotating (horizontal, vertical);

            dogAnim.SetFloat("Speed", 1f, 0f, Time.deltaTime);
            //GetComponent<Rigidbody> ().AddForce(transform.forward * moveSpeed );

        }
        else
            // Otherwise set the speed parameter to 0.
            dogAnim.SetFloat("Speed", 0);

        transform.LookAt(transform.position + moveDirection);
        //forward = Vector3.forward * horizontal;

        //dogRb.MovePosition(dogRb.position + moveDirection * moveSpeed * Time.deltaTime);
        //moveDirection.y = 0.09059119f;
        cc.SimpleMove(moveDirection * moveSpeed * Time.deltaTime);

        //dogRb.drag = dogRb.velocity.magnitude * dragFactor;
    }
}
