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

}
