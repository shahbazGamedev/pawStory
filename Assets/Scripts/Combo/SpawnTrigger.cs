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

    bool hasSpawned;
    //float dist;
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

    // Spawning takes place here
    public void OnTriggerEnter(Collider other)
    {
        //dist = Vector3.Distance(other.gameObject.transform.position, transform.position);
        //Debug.Log("dist: "+dist);
        if (other.gameObject.CompareTag("Player"))
        {

            if (/*dist < 12f && dist > 0.1f && Time.time - timer > 0.8f &&*/ !hasSpawned)
            {
                //Debug.LogError(Time.time - timer);
                //timer = Time.time;

                randNo = Random.Range(1, 6);
                randNo = randNo > 5 ? 1 : randNo;

                // Added to make platform type 2 to appear after 30 secs.
                if (ComboManager.instRef.distance < 15)
                {
                    randNo = randNo == 2 ? 1 : randNo;
                }

                if (prevPlat == 2 || prevPlat == 3 || beforePrevPlat == 2 || beforePrevPlat == 3 || twoBeforePrevPlat == 3 || prevPlat == 4 || prevPlat == 5)
                {
                    randNo = 1;
                }
                instance = Pooler.InstRef.GetPooledObject(randNo);
                instance.transform.position = transform.position - new Vector3(0f, 0f, 2 * distBtnPlatforms[beforePrevPlat == 2 ? 2 : prevPlat]);
                instance.SetActive(true);
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
            //Debug.Log(Vector3.Distance(other.gameObject.transform.position, transform.position));
            if (Vector3.Distance(other.gameObject.transform.position, transform.position) > 20f)
            {
                Pooler.InstRef.Sleep(gameObject);
                hasSpawned = false;
            }
        }
    }
}
