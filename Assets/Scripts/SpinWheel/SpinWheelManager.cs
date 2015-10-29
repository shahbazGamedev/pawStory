/**
Script Author : Vaikash
Description   : Spin Wheel - Game Manager
**/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpinWheelManager : MonoBehaviour
{
    #region Var

    public GameObject spinWheel;
    public int torqueMin;
    public int torqueMax;
    public float timerMax;
    public float timer;
    public int sectionCount; // starts from 0 && current image has 8 sections
    public Text result;
    public int[] torqCollection; // pre-calculated torque list

    int section;
    public float randomTorque;
    float angle;
    bool startSpin;
    bool endSpin;

    Rigidbody2D wheelRigidbody;

    // anim curve for angular drag
    AnimationCurve angularDragCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.8f, 1.0f), new Keyframe(1.0f, 2.0f));

    #endregion Var

    public void Awake()
    {
        wheelRigidbody = spinWheel.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        timer = timerMax;
        endSpin = true;
    }

    void Update()
    {
        if(startSpin)
        {
            // applies angular drag based on timer
            timer -= Time.deltaTime;
            wheelRigidbody.angularDrag = angularDragCurve.Evaluate(1 - (timer / timerMax));
            if(timer<=0f)
            {
                startSpin = false;
                Invoke("EndSpin", 3f); // delay before displaying result
            }
        }
    }

    // apply random torque
    void SpinWheel()
    {
        result.text = "";
        //randomTorque = -Random.Range(torqueMin, torqueMax); // produces random results
        wheelRigidbody.AddTorque(randomTorque);
        Debug.Log(randomTorque);
    }

    // process spin result
    void EndSpin()
    {
        Debug.Log("Bang!!");
        angle = spinWheel.transform.rotation.eulerAngles.z;
        section = (int) angle / (360 / sectionCount) + 1;
        result.text = "You have won item: " + section;
        StartCoroutine(ResetSpin());

    }

    // reset wheel rotation after 3 secs
    IEnumerator ResetSpin()
    {
        yield return new WaitForSeconds(3f);
        spinWheel.transform.rotation = Quaternion.Euler(0, 0, 0);
        endSpin = true;
        yield return null;
    }

    #region BtnCallbacks

    // spins the wheel
    public void Spin()
    {
        if (endSpin)
        {
            endSpin = false;
            timer = timerMax;
            startSpin = true;
            SpinWheel();
        }
    }

    // Added to check reproducibility @ mobile
    public void Btn1_Click()
    {
        randomTorque = torqCollection[0];
    }
    public void Btn2_Click()
    {
        randomTorque = torqCollection[1];
    }
    public void Btn3_Click()
    {
        randomTorque = torqCollection[2];
    }
    public void Btn4_Click()
    {
        randomTorque = torqCollection[3];
    }
    public void Btn5_Click()
    {
        randomTorque = torqCollection[4];
    }
    public void Btn6_Click()
    {
        randomTorque = torqCollection[5];
    }
    public void Btn7_Click()
    {
        randomTorque = torqCollection[6];
    }
    public void Btn8_Click()
    {
        randomTorque = torqCollection[7];
    }

    #endregion BtnCallbacks
}
