/**
Script Author : Vaikash 
Description   : Takes care of collisions in Agility
**/

using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour
{

    public collisionTypes objectID;
    public int triggerID; // starts from 1
    public static int previousID;
    public static GameObject dogRef;

    EllipseMovement dogMovement;

    public enum collisionTypes
    {
        hurdleJump,
        hurdleSlide,
        powerTurbo,
        powerSlow,
        waypoint
    }

    public void Awake()
    {
        if (dogRef == null)
            dogRef = GameObject.FindGameObjectWithTag("Player");
        dogMovement = dogRef.GetComponent<EllipseMovement>();
    }

    // Manage Collision
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (objectID == collisionTypes.waypoint)
            {
                if (triggerID != previousID)
                    dogMovement.FireDogMovedNextPartition();
                previousID = triggerID;
                return;
            }

            if (objectID == collisionTypes.hurdleJump || objectID == collisionTypes.hurdleSlide)
            {
                // Reduce dog movement speed
                //Debug.Log("Hit hurdle");
                dogMovement.RunCoroutine(18);
                AgilityManager.instanceRef.hurdlesCollided += 1;
                //Destroy(gameObject.transform.parent.gameObject);
                Pooler.InstRef.Sleep(gameObject.transform.parent.gameObject);
            }
            else if (objectID == collisionTypes.powerTurbo)
            {
                // Dog movement speed boost
                // Debug.Log ("Turbo Mode");
                dogMovement.RunCoroutine(28);
                //Destroy(gameObject.transform.parent.gameObject);
                Pooler.InstRef.Sleep(gameObject.transform.parent.gameObject);
            }
            else if (objectID == collisionTypes.powerSlow)
            {
                // Slow game speed
                // Debug.Log ("Bullet Time");
                dogMovement.RunCoroutine(14);
                //Destroy(gameObject.transform.parent.gameObject);
                Pooler.InstRef.Sleep(gameObject.transform.parent.gameObject);
            }
        }
    }
}
