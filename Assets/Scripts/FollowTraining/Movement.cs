using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public float distance;
    public Rigidbody rb;
    Vector3 pos;
    public float speed;
    public Transform target;
    private Animator dogAnim;
    public  bool isMoving;
    


    void Start ()
    {
        isMoving = true;
        rb = GetComponent<Rigidbody>();
        dogAnim = GetComponent<Animator>();
	}
	
	
	void Update ()
    {
        if (isMoving)
        {
            DogMovement();
        }
	}

    void DogMovement()
    {
        distance = Vector3.Distance(target.position, transform.position);
        if (distance < 6f)
        {
                
                Debug.Log("new");
                transform.LookAt(target);
                dogAnim.SetFloat("Walk", 1f);
                float step = speed * Time.deltaTime;
                pos = target.position;
                rb.MovePosition(Vector3.MoveTowards(transform.position, pos, step));
            }
            if(distance<2f)
            {
                isMoving = false;

                dogAnim.SetFloat("Walk", 0f);
                
            }
        }
    }

