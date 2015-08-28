/**
Script Author : Vaikash 
Description   : Combo - Handles Triggers
**/

using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour
{
    #region Var

    public float distBtnPlatforms;
    Renderer myRend;
    GameObject instance;

    #endregion var

    public void Awake()
    {
        myRend = GetComponentInChildren<Renderer>();
    }

    void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            instance = Pooler.InstRef.GetPooledObject(2);
            instance.transform.position = transform.position - new Vector3(0f, 0f, 2 * distBtnPlatforms);
            instance.SetActive(true);
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
