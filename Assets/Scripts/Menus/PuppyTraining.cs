using UnityEngine;
using System.Collections;

public class PuppyTraining : MonoBehaviour {

	void Start () {
	}

	
	void Update () {
	}


    public void OnCatchTraining()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_CatchTraining);
    }


    public void OnFollowTraining()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_FollowTraining);
    }


    public void OnColourLessonTraining()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_ColourLessonTraining);
    }


    public void OnRunningTraining()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_RunningTraining);
    }



}
