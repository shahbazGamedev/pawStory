using UnityEngine;
using System.Collections;

public class FollowTrainingManager : MonoBehaviour {

    public float speed;
    public GameObject targetObj;
    public Transform target;
    private Animator dogAnim;
    bool isMoving;
    float distance;
    Rigidbody rb;
    Vector3 pos;
    string layerName;
    




    void Start()
    {
        isMoving = true;
        rb = GetComponent<Rigidbody>();
        dogAnim = GetComponent<Animator>();
    }


    void Update()
    {
        if (isMoving)
        {
            DogMovement();
        }
        TargetCreator();
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
        if (distance < 2f)
        {
            isMoving = false;

            dogAnim.SetFloat("Walk", 0f);

        }
    }
    void TargetCreator()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log("Hit");
                layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (layerName == "Floor")
                {
                    targetObj.transform.position = hit.point + (Vector3.up * 0.01f);
                    isMoving = true;

                }
            }
        }
    }
}
