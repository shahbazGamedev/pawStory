using UnityEngine;
using System.Collections;

public class TutorialMgr : MonoBehaviour {
    public GameObject panel1;
    public GameObject panel2;

	
	void Start ()
    {
	
	}
	
	
	void Update ()
    {
	
	}
    public void Forward()
    {
        panel1.SetActive(false);
        
    }
    public void Backward()
    {
        panel1.SetActive(true);

    }


}
