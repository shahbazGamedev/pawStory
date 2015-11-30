using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    public bool isGrounded;

    Rigidbody foodRb;

    public void Awake()
    {
        foodRb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        isGrounded = true;
    }

    public void Jump(Vector3 direction)
    {
        //dogAnim.SetTrigger("Jump");
        foodRb.AddForce(direction, ForceMode.Impulse);
        isGrounded = false;
    }

}
