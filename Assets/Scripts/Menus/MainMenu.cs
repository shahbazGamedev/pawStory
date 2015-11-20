using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void Awake()
    {
        // Time Scale Glitch Fix
        Time.timeScale = 1f;
    }

    public void OnDogSelection()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_PuppySelection);
    }


    public void OnTraining()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_PuppyTraining);
    }


    public void OnGames()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_TournamentSelection);
    }

    public void OnPetting()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_Petting);
    }

    public void OnPark()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_Park);
    }

}

