using UnityEngine;
using System.Collections;

public class NPCShoot : MonoBehaviour {
	const float attackDistance = 5f;
	const float bulletSpeed = 15.0f;
	const float bulletDuration = 10.0f;

	public bool cheatRoot = false;
	public Transform bulletSpawnPoint;
	public GameObject bullet;
	public Transform bulletParent;

	Animator animator;
	GameObject player;
	Animator playerAnimator;
	bool hasShootInCycle; // shoot only when no shoot is in the play
	float previousStateTime;
	Vector3 lookAtPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		player = GameObject.FindWithTag ("Player");
		playerAnimator = player.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ShouldShootPlayer ()) {
			animator.SetBool ("Shoot", true);
			ManageShootCycle ();
			if (!hasShootInCycle) {
				//Debug.Log (animator.GetFloat ("Throw"));
				if (animator.GetFloat ("Throw") > .99f) { //The Throw animation reaches the end of the movement. 24 frame, then spawn the bullet
					SpawnBullet ();
				}
			}
		} else {
			animator.SetBool ("Shoot", false);
		}
	
	}

	bool ShouldShootPlayer ()
	{
		float distance = Vector3.Distance (transform.position, player.transform.position);
		if (distance < attackDistance) {
			AnimatorStateInfo playerStateInfo = playerAnimator.GetCurrentAnimatorStateInfo (0);
			if (!playerStateInfo.IsName ("Base Layer.Dying") && 
			    !playerStateInfo.IsName ("Base Layer.Death") &&
			    !playerStateInfo.IsName ("Base Layer.Reviving")) {
				return true;
			}
		}
		return false;
 	}

	void ManageShootCycle ()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		float time = Mathf.Repeat (stateInfo.normalizedTime, 1.0f); // the normalized time?????????
		//Debug.Log (time);
		if (time < previousStateTime) { // meaning the animation has restart the play.
			//Debug.Log ("" + time + " " + previousStateTime + " " + animator.GetFloat ("Throw"));
			hasShootInCycle = false;
		}
		previousStateTime = time;
	}

	void SpawnBullet ()
	{
		GameObject newBullet = Instantiate (bullet, bulletSpawnPoint.position, Quaternion.Euler (0, 0, 0)) as GameObject;
		Destroy (newBullet, bulletDuration);
		Vector3 direction = player.transform.position - bulletSpawnPoint.position;
		direction.y = 0;
		newBullet.GetComponent<Rigidbody> ().velocity = Vector3.Normalize (direction) * bulletSpeed;
		if(bulletParent) newBullet.transform.parent = bulletParent;
		hasShootInCycle = true;
 	}

	void OnAnimatorMove (){
		if (cheatRoot) {
			if (animator.GetBool ("Shoot")) {
				lookAtPosition.Set (player.transform.position.x,
				                    transform.position.y, 			//look at in parallel
				                    player.transform.position.z);
				transform.rotation = Quaternion.Lerp (transform.rotation, 
				                                      Quaternion.LookRotation (lookAtPosition - transform.position), //in x-z plane
				                                      Time.deltaTime*5); // speed up a little
				animator.rootRotation = transform.rotation; // use transform's rotationt to drive animation
			} 
			GetComponent<CharacterController>().Move (animator.deltaPosition); // without this line, the Teddy won't move at all. Since its root motion is not controlled by animator.
			transform.rotation = animator.rootRotation; // sync the transform's rotation with animation
			Vector3 position = transform.position;
			position.y = 0; // don't know why it is necessary.
			transform.position = position;
		}
	}
}
