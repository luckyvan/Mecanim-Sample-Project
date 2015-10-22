using UnityEngine;
using System.Collections;

public class MyLocomotion{
	private Animator anim;
	//Animator Parameter Hash Ids
	private int m_SpeedId = 0;
	private int m_AngularSpeedId = 0;
	private int m_DirectionId = 0;


	public float m_SpeedDampTime = .1f;
	public float m_AngularSpeedDampTime = .25f;
	public float m_DirectionResponseTime = .2f;

	public MyLocomotion(Animator animator){
		anim = animator;

		m_SpeedId = Animator.StringToHash("Speed");
		m_AngularSpeedId = Animator.StringToHash ("AngularSpeed");
		m_DirectionId = Animator.StringToHash ("Direction");
	}

	public void Do(float speed, float direction){
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (0);
		bool inTransition = anim.IsInTransition (0);
		bool inIdle = state.IsName ("Locomotion.Idle");
		bool inTurn = state.IsName ("Locomotion.TurnOnSpot");
		bool inWalkRun = state.IsName ("Locomotion.WalkRun");

		//DampTimes
		float speedDampTime = inIdle?0:m_SpeedDampTime;
		float angularSpeedDampTime = inWalkRun || inTransition ? m_AngularSpeedDampTime : 0; //use to decide the walk or run blend tree
		float directionDampTime = inTurn || inTransition ? 1000000 : 0; //Directly Set the direction, let animation decide the turn speed itself

		float angularSpeed = direction/m_DirectionResponseTime;

		anim.SetFloat (m_SpeedId, speed, speedDampTime, Time.deltaTime);
		anim.SetFloat (m_AngularSpeedId, angularSpeed, angularSpeedDampTime, Time.deltaTime);
		anim.SetFloat (m_DirectionId, direction, directionDampTime,Time.deltaTime);
	}
}
