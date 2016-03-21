using UnityEngine;
using System.Collections;


public enum PetState
{
	IDLE, 
	LICK,
	PLAY,
	JUMP,
	STAND,
	ROLL,
	MOVE

}
public class PetManager : MonoBehaviour 
{
	public static PetManager instance = null;

	public PetState petState;
	public GameObject dogRef;
	public Animator dogAnimator;
	public AnimatorStateInfo dogAnimStateInfo;

	public PetState currentState, prevState;

	public bool isIdle, isLicking, isPlaying, isJumping, isStanding, isRolling, isMoving;
	public float idleRandom,jumpRandom;
	public bool isAnimPlaying;
	public float animEndTime;

	void Awake()
	{
		instance = this;
		dogAnimator = dogRef.GetComponent <Animator> ();
	}


	void OnEnable()
	{
		EventManager.SceneStart += OnSceneStart;
		EventManager.SceneEnd += OnSceneEnd;
	}


	void OnDisable()
	{
		EventManager.SceneStart -= OnSceneStart;
		EventManager.SceneEnd -= OnSceneEnd;
	}

	void OnSceneStart()
	{
	}


	void OnSceneEnd()
	{

	}




	// Use this for initialization
	void Start () 
	{
	
	}

	public void SetState(PetState state)
	{
		switch(state)
		{
		case PetState.IDLE:

			break;

		case PetState.JUMP:

			break;

		case PetState.LICK:

			break;

		case PetState.ROLL:

			break;

		case PetState.MOVE:

			break;

		case PetState.PLAY:

			break;

		case PetState.STAND:

			break;

		default:

			break;
		}
	}

	void IdleState()
	{
		dogAnimator.SetTrigger ("idle");
		dogAnimator.SetFloat ("idleRandom",idleRandom);

	}

	void JumpState()
	{
		dogAnimator.SetTrigger ("jump");
		dogAnimator.SetFloat ("jumpRandom",jumpRandom);
	}


	public void Stand()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		dogAnimStateInfo = dogAnimator.GetCurrentAnimatorStateInfo (0);
	
	}
}
