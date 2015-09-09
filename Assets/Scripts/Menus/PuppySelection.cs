using UnityEngine;
using System.Collections;

public class PuppySelection : MonoBehaviour {
    Vector3 startingPos;
    Vector3 endPos;
    bool backMove;
    bool FrontMove;
    

	void Start ()
    {
        startingPos = Camera.main.transform.position;
        endPos = new Vector3(0.37f, 1.85f, 10.05f);
        FrontMove = true;
        
	}
	

	void Update ()
    {
        if(Camera.main.transform.position==startingPos)
        {
            backMove = false;
        }
        else
        {
            backMove = true;
        }
        if(Camera.main.transform.position == endPos)
        {
            FrontMove = false;
        }
        else
        {
            FrontMove = true;
        }

	}


    public void OnSelect()
    {
        GameMgr.Inst.LoadScene(GlobalConst.Scene_MainMenu);
    }
    public void Transition()
    {
        if (FrontMove)
        {
            Camera.main.transform.position += new Vector3(6, 0, 0);
        }

    }
    public void BackTransition()
    {
        if (backMove)
        {
            Camera.main.transform.position += new Vector3(-6, 0, 0);
        }
    }
}
