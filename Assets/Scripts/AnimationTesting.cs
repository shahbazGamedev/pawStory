using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimationTesting : MonoBehaviour
{

    int animCount;
    public int maxAnimCount;
    public SpriteRenderer max;
    public Text animName;

    public GameObject[] animatorRef;
    public string[] animClips;

    Animator animState;

    // Use this for initialization
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SyncAnim()
    {
        for (int i = 0; i < animatorRef.Length; i++)
        {
            animatorRef[i].GetComponent<Animator>().SetInteger("AnimCount", animCount);
            animatorRef[i].GetComponent<Animator>().SetTrigger("Sync");
            Debug.Log(animCount);
            
        }
        if(animCount>0)
            animName.text = animClips[animCount-1];
    }

    public void OnNextBtn()
    {
        animCount++;
        animCount = animCount > maxAnimCount ? maxAnimCount : animCount;
        SyncAnim();
        //Debug.Log(animCount);
    }

    public void OnPrevBtn()
    {
        animCount--;
        animCount = animCount < 0 ? 0 : animCount;
        SyncAnim();
    }
}