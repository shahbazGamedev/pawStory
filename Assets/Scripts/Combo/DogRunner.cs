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
    public Vector3 runDirection;
    public bool isGrounded;


    public bool updateAnim;
    Vector3 startPos;
    float dogVelocity;
    bool jump;
    bool gameOver;
    bool isCoroutineON;
    float time;

    CharacterController dogCC;
    public Animator dogAnim;

    public float jumpSpeed;
    float verticalVelocity = 0f;

    // Camera 
    public float smoothDampTime;
    public Vector3 cameraOffset;
    Vector3 smoothDampVelocity;

    public float springForce;

    #endregion Variables

    public void Awake()
    {
        instRef = this;
        dogCC = GetComponent<CharacterController>();
        dogAnim = GetComponent<Animator>();
    }

    void Start()
    {
        ComboManager.Jump += HandleDogJump;
        ComboManager.StartGame += StartGame;
        gameOver = true;
        startPos = transform.position;
    }

    public void OnDisable()
    {
        ComboManager.Jump -= HandleDogJump;
        ComboManager.StartGame -= StartGame;
    }

    void LateUpdate()
    {
        CamUp();
    }

    public void Update()
    {
       
        if (runStart)
        {
            // Handle Movement
            Vector3 dist = runDirection * runSpeed * Time.deltaTime;
            if (dogCC.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalVelocity += 2.5f * Physics.gravity.y * Time.deltaTime;
            }

            // Handle Jump
            if (dogCC.isGrounded && jump)
            {
                //Debug.Log(verticalVelocity);
                StartCoroutine(JumpForce(jumpSpeed));
                jump = false;
            }

            dist.y = verticalVelocity * Time.deltaTime;
            dogCC.Move(dist);

            dogVelocity = 1;
            SyncAnim();
            //dogCC.SimpleMove(runDirection * runSpeed * Time.deltaTime);

        }

    }

    //public void FixedUpdate()
    //{
        //if (runStart)
        //{
        //    //Moves dog using rigidbody - need to change
        //    dogRB.AddForce(runDirection * runSpeed * Time.deltaTime);
        //    Debug.Log(Vector3.Distance(dogRB.position, prevPos));
        //    //if (dogRB.velocity.magnitude > maxSpeed)
        //    //{
        //    //    dogRB.velocity = dogRB.velocity.normalized * maxSpeed;
        //    //}
        //    dogRB.drag = dogRB.velocity.magnitude * runDragFactor;
        //    //dogRB.MovePosition(dogRB.position + runDirection * runSpeed * Time.deltaTime);
        //    //prevPos = dogRB.position;
        //    dogVelocity = 1;
        //    SyncAnim();
        //}
    //}

    // Camera Movement
    void CamUp()
    {
        var pos = Camera.main.transform.parent.transform.position;
        pos.z = transform.position.z;
        // Camera.main.transform.parent.transform.position = pos;
        Camera.main.transform.parent.transform.position = Vector3.SmoothDamp(Camera.main.transform.parent.transform.position, pos - cameraOffset, ref smoothDampVelocity, smoothDampTime); ;
    }

    // Reset GameOver Flag
    void ResetGameOverFlag()
    {
        gameOver = false;
    }

    // Sync Animation
    void SyncAnim()
    {
        if (updateAnim)
        {
            dogAnim.SetFloat("Speed", dogVelocity);
            updateAnim = false;
        }
    }

    // GameOver
    public void GameOver()
    {

        gameOver = true;
        ComboManager.instRef.GameOver();
        runStart = false;
        dogVelocity = 0;
        updateAnim = true;
        SyncAnim();

    }

    // Reset Dog Pos
    public void ResetPos()
    {
        transform.position = startPos;
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

        if(dogCC.isGrounded && runStart)
        {
            jump = true;
            dogAnim.SetTrigger("Jump");
        }
        //if (isGrounded && runStart)
        //{
        //    dogAnim.SetTrigger("Jump");
        //    GetComponent<CharacterController>().AddForce(jumpForce, ForceMode.Impulse);
        //}
    }

    // Game start event handler
    void StartGame()
    {
        runStart = true;
        updateAnim = true;
        Invoke("ResetGameOverFlag", 2f);
    }
    #endregion EventHandlers

    #region Coroutines

    IEnumerator JumpForce(float force)
    {
        isCoroutineON = true;
        while (time<0.2f)
        {
            verticalVelocity += force * Time.deltaTime;
            time += Time.deltaTime;
            force -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        time = 0;
        isCoroutineON = false;
        yield return null;
    }



    #endregion Coroutines


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "target" && !isCoroutineON)
        {
            dogAnim.SetTrigger("Fly");
            StartCoroutine(JumpForce(springForce));
        }
        else if(hit.gameObject.tag == "LoseLine")
        {
            GameOver();
        }
    }
}
