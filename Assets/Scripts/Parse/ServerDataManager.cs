using UnityEngine;

public class ServerDataManager : MonoBehaviour
{
    public static ServerDataManager instance = null;

    public void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        Debug.Log("Events are registered on : " + "ServerDatamanager");
        
    }

    public void OnDisable()
    {
        Debug.Log("Events are deregistered on : " + "ServerDatamanager");
    }

    public void GetUserDataFromServer()
    {
        
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}