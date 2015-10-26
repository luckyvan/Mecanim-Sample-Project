using UnityEngine;
using System.Collections;

public class PlayerHurt : MonoBehaviour {
	const float wounderDampTime = .15f;
	Animator animator;
	float damage = 0f;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetFloat ("Wounded", damage, wounderDampTime, Time.deltaTime);
		float wounded = animator.GetFloat ("Wounded");
		animator.SetLayerWeight (2, Mathf.Clamp01 (wounded));

		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo (0);
		if (info.IsName ("Base Layer.Dying")) {
			animator.SetBool ("Dead", true);
		}else if(info.IsName ("Base Layer.Reviving")){
			animator.SetBool ("Dead", false);
		}else if(info.IsName ("Death") && info.normalizedTime > 3){
			damage = 0;
		}
	}

	public void DoDamage (float _damage){
		damage += _damage;
	}
}
