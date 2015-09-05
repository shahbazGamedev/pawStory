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
    public float[] distBtnPlatforms; //  starts from index 1

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
        if (other.gameObject.tag == "Player")
        {
            randNo = Random.Range(1, 6);
            randNo = randNo > 5 ? 1 : randNo;
            if(prevPlat==2 || prevPlat==3 || beforePrevPlat == 2 || beforePrevPlat == 3 || twoBeforePrevPlat == 3 || prevPlat==4 || prevPlat == 5)
            {
                randNo = 1;
            }
            instance = Pooler.InstRef.GetPooledObject(randNo);
            instance.transform.position = transform.position - new Vector3(0f, 0f, 2 * distBtnPlatforms[beforePrevPlat == 2  ? 2 : prevPlat]);
            instance.SetActive(true);

            // Update spawn history
            twoBeforePrevPlat = beforePrevPlat;
            beforePrevPlat = prevPlat;
            prevPlat = randNo;
        }
    }

    // De-spawning takes place here
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && DogRunner.instRef.runStart)
        {
            Pooler.InstRef.Sleep(gameObject);
        }
    }
}
