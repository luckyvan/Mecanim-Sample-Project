using UnityEngine;
using System.Collections;

public class NPCPatrol : MonoBehaviour {
	public Transform[] wayPoints;
	int wayPointIndex = 0;

	const float maxSpeed = 3f;
	const float speedDampTime = .25f;
	const float directionDampTime = .25f;
	const float thresholdDistance = 1.5f;

	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	// why do not just use the NavMesh instead?????
	void Update () {
		Transform target = wayPoints [wayPointIndex];
		if (Vector3.Distance (transform.position, target.position) > thresholdDistance) {
			animator.SetFloat ("Speed", maxSpeed, speedDampTime, Time.deltaTime); //accelerate gradually

			Vector3 currentDirection = animator.rootRotation * Vector3.forward; //standard method to define facing direction
			Vector3 wantedDirection = (target.position - transform.position).normalized; //normalized for cross method

			if (Vector3.Dot (currentDirection, wantedDirection) > 0) { // -90 degrees to 90 degrees
				animator.SetFloat ("Direction", 
				                   Vector3.Cross (currentDirection, wantedDirection).y, //|c|=|a||b|sin<a,b>, which means the less they are divided, the less 
				                   														// turn they need.
				                   directionDampTime,
				                   Time.deltaTime);
			} else {
				animator.SetFloat ("Direction", 
				                   Vector3.Cross (currentDirection, wantedDirection).y > 0 ? 1 : -1,
				                   directionDampTime,
				                   Time.deltaTime);
			}
		} else {
			animator.SetFloat ("Speed", 0, speedDampTime, Time.deltaTime);
			wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;
		}
	}
}
