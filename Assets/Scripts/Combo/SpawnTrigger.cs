/**
Script Author : Vaikash 
Description   : Combo - Handles Triggers
**/

using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour
{
    #region Var

    public static int prevPlat;
    public static int beforePrevPlat;
    public static int twoBeforePrevPlat;
    //public static float timer;
    public float[] distBtnPlatforms; //  starts from index 1
    public bool isStatic;

    bool hasSpawned;
    float dist;
    Vector3 offset;
    int randNo;
    GameObject instance;

    #endregion var

    public void Awake()
    {
        prevPlat = 1;
    }

    void Start()
    {
        //prevPlat = 1;
    }

    public void OnDisable()
    {
        //Debug.Log("Off");
        hasSpawned = false;
    }

    // Spawning takes place here
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasSpawned) // Floating Origin Fix
            {
                randNo = Random.Range(1, 6);
                randNo = randNo > 5 ? 1 : randNo;

                // Added to make platform type 2 to appear after 30 secs.
                if (ComboManager.instRef.distance < 15)
                {
                    randNo = randNo == 2 ? 1 : randNo;
                }

                if (prevPlat !=1 || beforePrevPlat == 2 || beforePrevPlat == 3 || twoBeforePrevPlat == 3 )
                {
                    randNo = 1;
                }
                instance = Pooler.InstRef.GetPooledObject(randNo);
                instance.transform.position = transform.position - new Vector3(0f, 0f, 2 * distBtnPlatforms[beforePrevPlat == 2 ? 2 : prevPlat]);
                instance.SetActive(true);

                if(!isStatic)
                    hasSpawned = true;

                // Update spawn history
                twoBeforePrevPlat = beforePrevPlat;
                beforePrevPlat = prevPlat;
                prevPlat = randNo;
            }
        }
    }



    // De-spawning takes place here
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && DogRunner.instRef.runStart)
        {
            offset = other.gameObject.transform.position - transform.position;
            dist = offset.sqrMagnitude;
            Debug.Log(dist);
            if (dist > 550f) // Floating Origin Fix
            {
                Pooler.InstRef.Sleep(gameObject);
                hasSpawned = false;
            }
        }
    }
}
