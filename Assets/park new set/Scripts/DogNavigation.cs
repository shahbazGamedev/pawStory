using UnityEngine;
using System.Collections;

public class DogNavigation : MonoBehaviour
{

    public static DogNavigation instRef;

    public float moveSpeed;
    public float rotationSpeed;

    public Vector2 doubleTapPos;

    public bool isCoroutineOn;

    Animator dogAnim;

    public void Awake()
    {
        instRef = this;
        dogAnim = GetComponent<Animator>();
    }


    // Use this for initialization
    void Start()
    {
        DoubleTap.PatternRecognized += PatternRecognizedEvent;
    }

    public void OnDisable()
    {
        DoubleTap.PatternRecognized -= PatternRecognizedEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        isCoroutineOn = true;
        dogAnim.SetFloat("Speed", 1f);
        dogAnim.SetTrigger("Start");
        yield return new WaitForSeconds(0.2f);
        transform.LookAt(targetPosition);

        while (Vector3.Distance(transform.position, targetPosition) + 0.7f > 1f)
        {
            yield return new WaitForFixedUpdate();
            targetPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.LookAt(targetPosition);
        }

        dogAnim.SetFloat("Speed", 0f);


        //while (transform.rotation != targetRotation)
        //{
        //    yield return new WaitForFixedUpdate();
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //}
        isCoroutineOn = false;
        yield return null;
    }

    //Event Handler for PatternRecognized Event
    void PatternRecognizedEvent(SwipeRecognizer.TouchPattern pattern)
    {
        Debug.Log(pattern);

        if (pattern == SwipeRecognizer.TouchPattern.doubleTap)
        {
            if (!isCoroutineOn)
            {
                var screenPoint = new Vector3(doubleTapPos.x, doubleTapPos.y, 0f);
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(screenPoint);
                if (Physics.Raycast(ray, out hit, 300f))
                {
                    string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    if (layerName == "Path")
                    {
                        var worldPoint = hit.point + (Vector3.up * 0.01f);
                        Debug.Log(worldPoint);

                        // code for move dog to target
                        StartCoroutine(DogNavigation.instRef.MoveToPosition(worldPoint, Quaternion.identity));

                    }
                }
            }
        }
    }
}
