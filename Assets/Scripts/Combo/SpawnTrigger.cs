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
    public static int count;
    public float[] distBtnPlatforms; //  starts from index 1

    int randNo;
    Renderer myRend;
    GameObject instance;

    #endregion var

    public void Awake()
    {
        myRend = GetComponentInChildren<Renderer>();
        prevPlat = 1;
    }

    void Start()
    {
        //prevPlat = 1;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            randNo = Random.Range(1, 6);
            randNo = randNo > 1 ? 1 : 2;
            randNo = prevPlat == 2 ? 1 : randNo;
            instance = Pooler.InstRef.GetPooledObject(randNo);
            instance.transform.position = transform.position - new Vector3(0f, 0f, 2 * distBtnPlatforms[beforePrevPlat > prevPlat ? beforePrevPlat : prevPlat]);
            instance.SetActive(true);
            beforePrevPlat = prevPlat;
            prevPlat = randNo;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Pooler.InstRef.Sleep(gameObject);
        }
    }
}
