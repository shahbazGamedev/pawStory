using UnityEngine;

/*

    1.Spawn only at the 4th spawn point at a faster pace.

    Info:
    1.platformSpawned_1 will not become null even after despawning.
    */

public class Prime31_PlatformGenerator : MonoBehaviour
{
    public static Prime31_PlatformGenerator prime31_PlatformGen;
    public GameObject[] platformTypes;
    public GameObject platformSpawnLoc_X,platformSpawnLoc_0, platformSpawnLoc_1, platformSpawnLoc_2, platformSpawnLoc_3, platformSpawnLoc_4;
    public GameObject previousPlatformSpawned;

    private float timeElapsed, spawnInterval=1.35f; //1.38f;
    private bool isAlreadySpawned = false;
    private bool hasSpawnedInitialPlatforms;

    // Use this for initialization
    void Start()
    {
        Debug.Log("This is prime31");
    }

    public void Awake()
    {
        prime31_PlatformGen = this;
        timeElapsed = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("ComboManager.gameRunning : " + ComboManager.gameRunning);
        if (ComboManager.gameRunning && timeElapsed > spawnInterval)
        {
            createPlatform(getRandomPlatform(), platformSpawnLoc_4);
            timeElapsed = 0;
        }

        timeElapsed += Time.unscaledDeltaTime;

    }

    //Create platforms at the spawn points
    public void createPlatformsAtStart()
    {
        createPlatform(platformTypes[0], platformSpawnLoc_X);
        createPlatform(platformTypes[0], platformSpawnLoc_0);
        createPlatform(platformTypes[0], platformSpawnLoc_1);
        createPlatform(platformTypes[0], platformSpawnLoc_2);
        createPlatform(platformTypes[0], platformSpawnLoc_3);
        //createPlatform(platformTypes[0], platformSpawnLoc_4);

        Debug.Log("Created dummy platforms ****** ");
    }

    //creates a single platform at the given spawn point
    private GameObject createPlatform(GameObject platformType, GameObject platformSpawnPoint)
    {
        Debug.Log("Created a new platform of Type = "+platformType);
        GameObject temp= TrashMan.spawn(platformType, platformSpawnPoint.transform.position);
        temp.transform.SetParent(GameObject.Find("Runtime_SpawnHolder").transform);
        previousPlatformSpawned = temp;
        return temp;
    }

    private GameObject getRandomPlatform()
    {
        GameObject[] t_platformTypes = platformTypes;
        GameObject nextPlatform = platformTypes[Random.Range(0, platformTypes.Length)];

        if (previousPlatformSpawned != null)
            if (string.Equals(previousPlatformSpawned.gameObject.name, nextPlatform.gameObject.name))
            {
                Debug.Log("Same platform found");
                nextPlatform = platformTypes[0];
                return nextPlatform;
            }

        return nextPlatform;
    }

    /// <summary>
    /// Cleans the old platforms.
    /// </summary>
    public void cleanOldPlatforms()
    {
        GameObject spawnHolderParent = GameObject.Find("Runtime_SpawnHolder") ;

        for (int i = 0; i < spawnHolderParent.transform.childCount; i++)
        {
            Destroy(spawnHolderParent.transform.GetChild(i).gameObject);
        }

        Debug.Log("Cleared the leftover platforms.");
    }
}