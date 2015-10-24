using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {
	public bool Slide;
	public bool Vault;
	public bool DeactivateCollider;
	public bool MatchTarget;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (Anim) {
			if (Slide) {
				ProcessSlide ();
			}
			if (DeactivateCollider) {
				Controller.enabled = Anim.GetFloat ("Collider") > .5f;
			}
			ProcessMatchTarget ();
		}
	}

	void ProcessSlide ()
	{
		bool slide = false;
		RaycastHit hit;
		Vector3 dir = transform.TransformDirection (Vector3.forward); //??????
		if (Anim.GetCurrentAnimatorStateInfo (0).IsName ("Locomotion.Run")) {
			if (Physics.Raycast (transform.position + new Vector3(0, 1.5f, 0), dir, out hit, 10)) {
				if (hit.collider .tag == "Obstacle") {
					Target = transform.position + 1.25f*hit.distance*dir;
					slide = (hit.distance < 6);
				}
			}
		}
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
}
