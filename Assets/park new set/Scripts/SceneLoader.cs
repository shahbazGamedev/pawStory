using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public static GameObject gameText;
    public static Text gameNameText;

    public void Start()
    {
        if (enterBtn == null)
        {
            enterBtn = GameObject.FindGameObjectWithTag("Respawn");
        }
        if (gameText == null)
        {
            gameText = GameObject.FindGameObjectWithTag("BtnText");
            gameNameText = gameText.GetComponent<Text>();
        }
        enterBtn.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        currentPos = ID;
        enterBtn.SetActive(true);
        gameText.SetActive(true);
        SyncName();
    }

    public void OnTriggerExit(Collider other)
    {
        enterBtn.SetActive(false);
        gameText.SetActive(false);
    }

    public void SyncName()
    {
        switch (currentPos)
        {
            case Training.Balloon:
                {
                    gameNameText.text = "Balloon";
                    break;
                }
            case Training.ColorLesson:
                {
                    gameNameText.text = "Color Lesson";
                    break;
                }
            case Training.Follow:
                {
                    gameNameText.text = "Follow Training";
                    break;
                }
            case Training.JumpCatch:
                {
                    gameNameText.text = "Jump Catch";
                    break;
                }
            case Training.Obedience:
                {
                    Debug.Log("Missing");
                    break;
                }
            case Training.Running:
                {
                    gameNameText.text = "Running Training";
                    break;
                }
            case Training.Skipping:
                {
                    gameNameText.text = "Skipping";
                    break;
                }
            default:
                {
                    break;
                }
        }
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
