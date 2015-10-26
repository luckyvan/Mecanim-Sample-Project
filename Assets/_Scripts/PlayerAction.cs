using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {
	public bool Slide;
	public bool Vault;
	public bool DeactivateCollider;
	public bool MatchTarget;

<<<<<<< HEAD
	private const float VaulatMatchTargetStart = .4f;
	private const float VaulatMatchTargetStop = .51f;
	private const float SlideMatchTargetStart = .11f;
	private const float SlideMatchTargetStop = .40f;

	private Animator Anim;
	private CharacterController Controller;
	private Vector3 Target;

	// Use this for initialization
	void Start () {
		Anim = GetComponent<Animator>();
		Controller = GetComponent<CharacterController>();
=======
	private const float VaultMatchTargetStart = .4f;
	private const float VaultMatchTargetStop = .51f;
	private const float SlideMatchTargetStart = .11f;
	private const float SlideMatchTargetStop = .4f;

	private Animator anim;
	private CharacterController controller;
	private Vector3 target = Vector3.zero;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
>>>>>>> c54a3570ee64a2eeb21e3e31f63a0a1c735848da
	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
		if (Anim) {
			if (Slide) {
				ProcessSlide ();
			}
			if (DeactivateCollider) {
				Controller.enabled = Anim.GetFloat ("Collider") > .5f;
=======
	    if (anim) {
			if (Slide) {
				ProcessSlide ();
			}
			if (Vault) {
				ProcessVault ();
			}
			if (DeactivateCollider) {
				controller.enabled = anim.GetFloat ("Collider") > .5f;
>>>>>>> c54a3570ee64a2eeb21e3e31f63a0a1c735848da
			}
			ProcessMatchTarget ();
		}
	}

	void ProcessSlide ()
	{
		bool slide = false;
<<<<<<< HEAD
		RaycastHit hit;
		Vector3 dir = transform.TransformDirection (Vector3.forward); //??????
		if (Anim.GetCurrentAnimatorStateInfo (0).IsName ("Locomotion.Run")) {
			if (Physics.Raycast (transform.position + new Vector3(0, 1.5f, 0), dir, out hit, 10)) {
				if (hit.collider .tag == "Obstacle") {
					Target = transform.position + 1.25f*hit.distance*dir;
=======

		RaycastHit hit;
		//transform from local space to world space.
		Vector3 direction = transform.TransformDirection (Vector3.forward);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName ("Locomotion.Run")) {
			// from a little above the head
			if (Physics.Raycast (transform.position + new Vector3 (0, 1.5f, 0), direction, out hit, 10)) {
				if (hit.collider.tag == "Obstacle") {
					target = transform.position + 1.25f * hit.distance * direction;//.25 distance over the fence
>>>>>>> c54a3570ee64a2eeb21e3e31f63a0a1c735848da
					slide = (hit.distance < 6);
				}
			}
		}
<<<<<<< HEAD
		Anim.SetBool ("Slide", slide);
	}

	void ProcessMatchTarget ()
	{
		AnimatorStateInfo info = Anim.GetCurrentAnimatorStateInfo (0);
		Quaternion q = new Quaternion();
		if (info.IsName ("Base Layer.Slide")) {
			Anim.MatchTarget (Target, q, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(1, 0, 1), 0), SlideMatchTargetStart, SlideMatchTargetStop );
		}
	}
=======
		anim.SetBool ("Slide", slide);
	}

	void ProcessVault ()
	{
		bool vault = false;
		
		RaycastHit hit;
		//transform from local space to world space.
		Vector3 direction = transform.TransformDirection (Vector3.forward);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName ("Locomotion.Run")) {
			if (Physics.Raycast (transform.position + new Vector3 (0, .3f, 0), direction, out hit, 10)) {
				if (hit.collider.tag == "Obstacle") {
					target = hit.point;
					vault = (hit.distance < 4.5f && hit.distance > 4f);
				}
			}
		}
 	}

	void ProcessMatchTarget ()
	{
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		Quaternion q = new Quaternion();

		if (stateInfo.IsName ("Base Layer.Slide")) {

			anim.MatchTarget (target, //.25 distance over the fence
			                 q, 
			                 AvatarTarget.Root, //root is the target body part
			                 new MatchTargetWeightMask (new Vector3 (1, 0, 1), //x,z 1 weight
			                          0), // zero rotation weight
			                 SlideMatchTargetStart, 
			                 SlideMatchTargetStop);
		} else if (stateInfo.IsName ("Base Layer.Vault")) {
			if (MatchTarget) {
				anim.MatchTarget (target, 
				                  q, 
				                  AvatarTarget.LeftHand, //left hand is the target body part
				                  new MatchTargetWeightMask (Vector3.one, //x,y,z 1 weight
				                           0), // zero rotation weight
				                  VaultMatchTargetStart, 
				                  VaultMatchTargetStop);
			}
		}

 	}
>>>>>>> c54a3570ee64a2eeb21e3e31f63a0a1c735848da
}
