using UnityEngine;
using System.Collections;

public class PettingBall : MonoBehaviour
{
    public GameObject dogRef;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        Physics.IgnoreCollision(dogRef.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
