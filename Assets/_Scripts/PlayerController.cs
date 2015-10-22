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
		if (anim && Camera.main) {
			Vector3 dir = new Vector3(Input.GetAxis ("Horizontal"), 0 ,Input.GetAxis ("Vertical"));
			Vector3 worldDir = Camera.main.transform.rotation * dir; //make it towards camera direction
			worldDir.y = 0;
			worldDir.Normalize();
			float _speed = Mathf.Clamp (worldDir.magnitude, 0, 1);
			float _direction = 0.0f;
			if (_speed > .01f) { // moving
				Vector3 axis = Vector3.Cross (transform.forward, worldDir);
				_direction = Vector3.Angle (transform.forward, worldDir)/180.0f * (axis.y < 0 ? -1 : 1);
				speed = _speed;
				direction = _direction;
			}
			loc.Do (speed * 6, direction * 180);
			anim.SetBool ("HoldLog", hasLog);
		}
	}
}
