using UnityEngine;
using System.Collections;

public class ComboTracker : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(""))
        {
            // update combo
        }
    }
}
