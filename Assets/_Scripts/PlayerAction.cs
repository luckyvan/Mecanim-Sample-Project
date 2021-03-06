﻿using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {
	public bool Slide;
	public bool Vault;
	public bool DeactivateCollider;
	public bool MatchTarget;

	public GameObject bullet;
	public float bulletDuration = 1f;
	private float bulletDisplayTime = 0f;
	public bool displayTarget;

	private const float VaultMatchTargetStart = .4f;
	private const float VaultMatchTargetStop = .51f;
	private const float SlideMatchTargetStart = .11f;
	private const float SlideMatchTargetStop = .4f;

	private Animator anim;
	// CharacterController use SimpleMove and Move as its interface.
	private CharacterController controller;
	private Vector3 target = Vector3.zero;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
	    if (anim) {
			if (Slide) {
				ProcessSlide ();
			}
			if (Vault) {
				ProcessVault ();
			}
			if (DeactivateCollider) {
				//Selecting Slide Animation in Animation View can see its curve for "Collider" Parameter
				//When "Collider" < .5f, the Player Slides across the fence, hence need to disable collider component to pass it
				controller.enabled = anim.GetFloat ("Collider") > .5f;
			}
			ProcessMatchTarget ();
		}
	}

	void ProcessSlide ()
	{
		bool slide = false;

		RaycastHit hit;
		//transform from local space to world space.
		Vector3 direction = transform.TransformDirection (Vector3.forward);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName ("Locomotion.Run")) {
			// from a little above the head
			if (Physics.Raycast (transform.position + new Vector3 (0, 1.5f, 0), direction, out hit, 10)) {
				if (hit.collider.tag == "Obstacle") {
					target = transform.position + 1.25f * hit.distance * direction;//.25 distance over the fence
					slide = (hit.distance < 6);

					DisplayTarget ();
				}
			}
		}
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
					target = hit.point; //target be the obstacle itself
					vault = (hit.distance < 4.5f && hit.distance > 4f);

					DisplayTarget ();
				}
			}
		}
		anim.SetBool ("Vault", vault);
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
			                 SlideMatchTargetStart, //nomalized Animation Time. Could See the animation by selecting  animation window and choosing the right animation clip in it.
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

	void DisplayTarget ()
	{
		if (displayTarget && (Time.time > bulletDisplayTime)) {
			GameObject newBullet = Instantiate (bullet, target, Quaternion.Euler (0, 0, 0)) as GameObject;
			Destroy (newBullet, bulletDuration);
			bulletDisplayTime = Time.time + bulletDuration;
		}
	}
}
