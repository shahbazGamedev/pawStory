using UnityEngine;
using System;
using System.Collections;

public class EventManager : MonoBehaviour
{

  //  public delegate void Action();
   
    public static event Action SceneStart, GamePaused, GameResumed, UpdateUI ,GameRestart, SceneEnd;

  //  public delegate void ServerAction();

    public static event Action GetDataFromServer,PostDataToServer,UpdateDataFromServer;
     
    public delegate void FacebookCallBack(string result);

    public static event Action FacebookInit , FacebookLogin, FacebookLoginWithPermissions, FacebookUserProfile;

    //public static event ServerAction 

    public static void OnSceneStart()
    {
        if (SceneStart != null)
        {

            SceneStart();
        }
    }

    public static void OnGamePaused()
    {
        if (GamePaused != null)
        {

            GamePaused();
        }
    }

    public static void OnGameResumed()
    {
        if (GameResumed != null)
        {

            GameResumed();
        }
    }

    public static void OnUpdateUI()
    {
        if (UpdateUI != null)
        {
            UpdateUI();
        }
    }


    public static void OnGameRestart()
    {
        if (GameRestart != null)
        {

            GameRestart();
        }
    }

    public static void OnSceneEnd()
    {
        if (SceneEnd != null)
        {
            SceneEnd();
        }
    }

    public static void OnGetServerData()
    {
        if(GetDataFromServer !=null)
        { 
            GetDataFromServer();
        }
    }

    public static void OnPostServerData()
    {
        if(PostDataToServer !=null)
        {
            PostDataToServer();
        }
    }

    public static void OnUpdateServerData()
    {
        if(UpdateDataFromServer !=null)
        {
            UpdateDataFromServer();
        }
    }


    public static void OnFacebookInit()
    {
        if(FacebookInit !=null)
        {
            FacebookInit();
        }
    }


    public static void OnFacebookLogin()
    {
        if(FacebookLogin !=null)
        {
            FacebookLogin();
        }
    }


    public static void OnFacebookLoginWithPermissions()
    {
        if(FacebookLoginWithPermissions !=null)
        {
            FacebookLoginWithPermissions();
        }
    }


    public static void OnFacebookUserProfile()
    {
        if (FacebookUserProfile != null)
        {
            FacebookUserProfile();
        }
    }
}
