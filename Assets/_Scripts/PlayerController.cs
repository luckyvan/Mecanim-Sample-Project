using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private Animator anim;
	private MyLocomotion loc = null;
	private float speed = 0;
	private float direction = 0;
	public bool hasLog = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.logWarnings = false;
		loc = new MyLocomotion(anim);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ((Camera.main.transform.rotation*Vector3.forward).ToString ()); 
		if (anim && Camera.main) {
			Vector3 dir = new Vector3(
				Input.GetAxis ("Horizontal"), //between -1 .. 1. Increase by the length of Press Horizontal Key.
				0,
				Input.GetAxis ("Vertical"));
			//Debug.Log (dir.ToString ());
			Vector3 worldDir = Camera.main.transform.rotation * dir; //make it towards camera direction, or make the input into camera space
																	// the camera always behind the player, hence after the rotation, the input would always be relative to player space
								//transform.rotation * dir
								// can have similar effect when input is horizontal, but would gitter drastically when input is backwards.
																	
			//Debug.Log (worldDir.ToString ());
			worldDir.y = 0;
			worldDir.Normalize(); //After Normalize (), the length would be 0, 1s
			float _speed = Mathf.Clamp (worldDir.magnitude, 0, 1); // speed could be 0, 1, .9999999
			//Debug.Log (_speed);
			float _direction = 0.0f;

			if (_speed > .01f) { // moving
				Vector3 axis = Vector3.Cross (
					transform.forward, //blue axis of the transform in world space
					worldDir); // left rules. 
				//Debug.Log (transform.forward);
				_direction = Vector3.Angle (transform.forward, worldDir)/180.0f * (axis.y < 0 ? -1 : 1);
				//Debug.Log (_direction);
			}
			speed = _speed;
			direction = _direction;
			loc.Do (speed * 6, direction * 180);
			anim.SetBool ("HoldLog", hasLog);
		}
	}
}
