using UnityEngine;
using System.Collections;

public class PuppySelection : MonoBehaviour {


	void Start () {
	
	}
	

	void Update () {
	
	}


    public void OnSelect()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }
}
