/**
Script Author : Vaikash 
Description   : Controls Collider when dog jumps
**/

using UnityEngine;
using System.Collections;

public class ComboColliderController : MonoBehaviour {

    #region Var

    int stateJump = Animator.StringToHash("Jump");
    int stateRoll = Animator.StringToHash("Roll");
    float boxColliderYBase;

    // Anim Curves
    AnimationCurve colliderYCurve = new AnimationCurve(new Keyframe(0.0f, 1), new Keyframe(0.5f, 1.5f), new Keyframe(0.7f, 1));
    AnimationCurve colliderYCurve1 = new AnimationCurve(new Keyframe(0.0f, 1),new Keyframe(0.25f, 2f),new Keyframe(0.5f, 3f), new Keyframe(0.75f, 2f), new Keyframe(0.9f, 1));

    AnimatorStateInfo animStateBase;
    BoxCollider boxCollider;

    #endregion Var

    // Use this for initialization
    void Start() {
        boxCollider = GetComponent<Collider>() as BoxCollider;
        boxColliderYBase = boxCollider.center.y;
    }

    // Update is called once per frame
    void Update() {

        animStateBase = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0); // 0 = BaseLayer


        if (animStateBase.shortNameHash == stateJump)
        {
            // get the jump animation's time passed
            var animTime = animStateBase.normalizedTime;
            var curveValue = colliderYCurve.Evaluate(animTime);


            var newCenter=boxCollider.center;
            newCenter.y= boxColliderYBase * curveValue;
            boxCollider.center = newCenter;
        }
        else if (animStateBase.shortNameHash == stateRoll)
        {
            // get the jump animation's time passed
            var animTime = animStateBase.normalizedTime;
            var curveValue = colliderYCurve1.Evaluate(animTime);

            var newCenter=boxCollider.center;
            newCenter.y = boxColliderYBase * curveValue;
            boxCollider.center = newCenter;
        }
    }
}
