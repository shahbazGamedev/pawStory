using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

[System.Serializable]
public class DogProperties
{
    public int _Id;
    public int _WeeksOld;
    public string _Name;
    public float _Health;
    public float _WalkSpeed;
    public float _RunSpeed;
    public float _Tiredness;
    public float _Happiness;
    public float _SleepTime;
    public float _WaterLevel;
    public float _FoodLevel;
}


public class DogManager : MonoBehaviour
{
    public static DogManager instRef;
    public GameObject dogReference;
    public DogProperties dogProps;
    public bool disableDogControl;

    public float turnSmoothing = 15f;   // A smoothing value for turning the player.
    public float speedDampTime = 0.1f;  // The damping for the speed parameter

    Animator dogAnim;

    public float moveSpeed;
    public float rotationSpeed;

    Vector3 moveDirection;
    Vector3 jumpHeight;
    public float jumpForce;
    public float dragFactor;
    public bool isGrounded=false;
    public bool isCoroutineOn;

    Vector2 swipeBegin;
    Vector2 swipeEnd;

    public bool isCircuitRun; // Added to override Run animation during Circuit Run

    // Event to broadcast reset complete to listeners
    public delegate void DogReset();
    public event DogReset ResetComplete;

    public void Awake()
    {
        instRef = this;
        dogAnim = dogReference.GetComponent<Animator>();
    }


    // Use this for initialization
    void Start()
    {

        jumpHeight = new Vector3(0, jumpForce, 0);
    }


    void FixedUpdate()
    {
        if (!disableDogControl)
        {
            // Cache the inputs.
            float h = CrossPlatformInputManager.GetAxis ("Horizontal");
            float v = CrossPlatformInputManager.GetAxis ("Vertical");

            MovementManagement(h, v);
        }
    }

    void MovementManagement(float horizontal, float vertical)
    {


        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded == true)
        {
            Jump();
        }

        moveDirection = new Vector3(horizontal, 0, vertical);

        // If there is some axis input...
        if (horizontal != 0f || vertical != 0f || isCircuitRun == true)
        {
            // ... set the players rotation and set the speed parameter to 5.5f.
            //Rotating (horizontal, vertical);

            dogAnim.SetFloat("Speed", 1f, 0f, Time.deltaTime);
            //GetComponent<Rigidbody> ().AddForce(transform.forward * moveSpeed );

        }
        else
            // Otherwise set the speed parameter to 0.
            dogAnim.SetFloat("Speed", 0);


        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * moveSpeed * Time.deltaTime);
        transform.LookAt(transform.position + moveDirection);
        GetComponent<Rigidbody>().drag = GetComponent<Rigidbody>().velocity.magnitude * dragFactor;
    }


    // Update is called once per frame
    void Update()
    {


    }

    public void Jump()
    {
        dogAnim.SetTrigger("Jump");
        GetComponent<Rigidbody>().AddForce(jumpHeight, ForceMode.Impulse);

    }

    // Overloading for JumpAndShoot Module
    public void Jump(Vector3 direction)
    {
        dogAnim.SetTrigger("Jump");
        GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);

    }

    void OnTriggerStay()
    {
        isGrounded = true;
    }

    void OnTriggerExit()
    {
        isGrounded = false;
    }

    public void OnPointerDown(BaseEventData data)
    {
        //Debug.Log("Begins");
        PointerEventData e=(PointerEventData) data;
        swipeBegin = e.position;
    }

    public void OnPointerUp(BaseEventData data)
    {
        //Debug.Log("Ends");
        PointerEventData e=(PointerEventData) data;
        swipeEnd = e.position;
        detectSwipe();
    }

    void detectSwipe()
    {
        Vector2 direction=swipeEnd-swipeBegin;
        direction.Normalize();

        //swipe upwards
        if (direction.y > 0 && direction.x > -0.5f && direction.x < 0.5f)
        {
            //Debug.Log("up swipe");
            if (isGrounded)
                Jump();

        }
        //swipe down
        if (direction.y < 0 && direction.x > -0.5f && direction.x < 0.5f)
        {
            Debug.Log("down swipe");
        }
        //swipe left
        if (direction.x < 0 && direction.y > -0.5f && direction.y < 0.5f)
        {
            Debug.Log("left swipe");
        }
        //swipe right
        if (direction.x > 0 && direction.y > -0.5f && direction.y < 0.5f)
        {
            Debug.Log("right swipe");
        }
    }

    /// <summary>
    /// Moves the dog to target position using coroutine.
    /// </summary>
    /// <param name="targetPosition">Target position.</param>
    /// <param name="targetRotation">Target rotation.</param>
    public IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        isCircuitRun = true;
        isCoroutineOn = true;
        if (disableDogControl)
        {
            dogAnim.SetFloat("Speed", 1f, 0f, Time.deltaTime);
            yield return new WaitForSeconds(1f);
            transform.LookAt(targetPosition);
        }
        while (Vector3.Distance(transform.position, targetPosition)+0.7f >1f)
        {
            yield return new WaitForFixedUpdate();
            targetPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.LookAt(targetPosition);
        }
        if (disableDogControl)
        {
            dogAnim.SetFloat("Speed", 0f, 0f, Time.deltaTime);
            yield return new WaitForSeconds(0.8f);
        }
        isCircuitRun = false;
        while (transform.rotation != targetRotation)
        {
            yield return new WaitForFixedUpdate();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        isCoroutineOn = false;

        // Trigger Event if it has listeners
        if (ResetComplete != null)
            ResetComplete();

        yield return null;
    }


}
