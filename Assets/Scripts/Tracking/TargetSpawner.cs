/**
Script Author : Vaikash
Description   : Spawns target for dog to track
**/

using UnityEngine;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    #region Variables

    public static TargetSpawner instRef;
    public GameObject markerPrefab;
    public GameObject[] targetCollection;

    int randNo;
    List<GameObject> markerStorage;

    #endregion Variables

    void Start()
    {
        instRef = this;
        markerStorage = new List<GameObject>();

        // Add Event Listener
        TrackingManager.SpawnMarker += RandMarkerSpawn;
    }

    void Update()
    {
    }

    // Decouple Event Markers
    public void OnDisable()
    {
        TrackingManager.SpawnMarker -= RandMarkerSpawn;
    }

    // Destroy all alive markers
    public void KillMarkers()
    {
        foreach (var obj in markerStorage)
        {
            Destroy(obj);
        }
    }

    #region EventTriggers

    // Called to spawn markers randomly
    void RandMarkerSpawn()
    {
        if (markerStorage.Count > 0)
        {
            KillMarkers();
        }
        randNo = Random.Range(0, targetCollection.Length);
        GameObject instance=(GameObject)Instantiate(markerPrefab, targetCollection[randNo].transform.position, Quaternion.identity);
        markerStorage.Add(instance);
    }

    #endregion EventTriggers
}