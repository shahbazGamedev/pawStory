﻿/**
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
    public float maxSpeed;
    public float runDragFactor;
    public Vector3 runDirection;
    public Vector3 jumpForce;
    public bool isGrounded;

    public bool updateAnim;
    Vector3 startPos;
    float dogVelocity;
    bool gameOver;

    Rigidbody dogRB;
    public Animator dogAnim;

    #endregion Variables

    public void Awake()
    {
        instRef = this;
        dogRB = GetComponent<Rigidbody>();
        dogAnim = GetComponent<Animator>();
    }

    void Start()
    {
        ComboManager.Jump += HandleDogJump;
        ComboManager.StartGame += StartGame;
        gameOver = true;
        //dogRB.mass = 10;
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

    public void FixedUpdate()
    {
        if (runStart)
        {
            // Moves dog using rigidbody - need to change
            dogRB.AddForce(runDirection * runSpeed * Time.deltaTime);
            //Debug.Log(dogRB.velocity.magnitude);
            //if (dogRB.velocity.magnitude > maxSpeed)
            //{
            //    dogRB.velocity = dogRB.velocity.normalized * maxSpeed;
            //}
            dogRB.drag = dogRB.velocity.magnitude * runDragFactor;
            dogVelocity = 1;
            SyncAnim();
        }
        if (dogRB.velocity.magnitude <= 0.1f && !gameOver)
        {
            gameOver = true;
            ComboManager.instRef.GameOver();
            runStart = false;
            dogVelocity = 0;
            updateAnim = true;
            SyncAnim();
        }
    }

    // Camera Movement
    void CamUp()
    {
        var pos = Camera.main.transform.parent.transform.position;
        pos.z = dogRB.transform.position.z;
        Camera.main.transform.parent.transform.position = pos;
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
        if (isGrounded && runStart)
        {
            dogAnim.SetTrigger("Jump");
            GetComponent<Rigidbody>().AddForce(jumpForce, ForceMode.Impulse);
        }
    }

    // Game start event handler
    void StartGame()
    {
        runStart = true;
        updateAnim = true;
        Invoke("ResetGameOverFlag", 2f);
    }
    #endregion EventHandlers
}
