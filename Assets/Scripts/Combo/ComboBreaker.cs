using UnityEngine;
using System.Collections;

public class ComboBreaker : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // code for breaking combo
            ScoreSystem.instRef.ComboBroken();
        }
    }
}
