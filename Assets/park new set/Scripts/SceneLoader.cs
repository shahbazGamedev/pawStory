using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public enum Training
    {
        JumpCatch,
        Follow,
        ColorLesson,
        Running,
        Obedience,
        Skipping,
        Balloon,
        None
    }

    public Training ID;
    public static Training currentPos;
    public static GameObject enterBtn;

    public void Start()
    {
        if (enterBtn == null)
        {
            enterBtn = GameObject.FindGameObjectWithTag("Respawn");
        }
        enterBtn.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        currentPos = ID;
        enterBtn.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        enterBtn.SetActive(false);
    }

    public void EnterGame()
    {
        switch (currentPos)
        {
            case Training.Balloon:
                {
                    GameMgr.Inst.LoadScene(GlobalConst.Scene_Bubble);
                    break;
                }
            case Training.ColorLesson:
                {
                    GameMgr.Inst.LoadScene(GlobalConst.Scene_ColourLessonTraining);
                    break;
                }
            case Training.Follow:
                {
                    GameMgr.Inst.LoadScene(GlobalConst.Scene_FollowTraining);
                    break;
                }
            case Training.JumpCatch:
                {
                    GameMgr.Inst.LoadScene(GlobalConst.Scene_CatchTraining);
                    break;
                }
            case Training.Obedience:
                {
                    Debug.Log("Missing");
                    break;
                }
            case Training.Running:
                {
                    GameMgr.Inst.LoadScene(GlobalConst.Scene_RunningTraining);
                    break;
                }
            case Training.Skipping:
                {
                    GameMgr.Inst.LoadScene(GlobalConst.Scene_Skipping);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
