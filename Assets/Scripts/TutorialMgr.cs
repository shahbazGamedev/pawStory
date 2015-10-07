using UnityEngine;
using System.Collections;

public class TutorialMgr : MonoBehaviour
{
    public GameObject[] TutImg;
    public GameObject ForwardBtn;
    public GameObject Backbutton;
    public int count;
    

    void Start()
    {
        Backbutton.SetActive(false);
    }


    void Update()
    {
    }


    public void PointerDownForward()
    {
        Debug.Log(count);
        Backbutton.SetActive(true);
        TutImg[count].SetActive(false);
        count += 1;
        if(count >= 11)
        {
            count = 11;
            ForwardBtn.SetActive(false);
        }
        TutImg[count].SetActive(true);
    }


    public void PointerDownBack()
    {
        Debug.Log(count);
        TutImg[count].SetActive(false);
        ForwardBtn.SetActive(true);
        count -= 1;
        if(count <= 0)
        {
            count = 0;
            
            Backbutton.SetActive(false);
        }
        TutImg[count].SetActive(true);
    }

    
}

