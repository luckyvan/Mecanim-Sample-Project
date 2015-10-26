using UnityEngine;
using System.Collections;

public class PlayerWithLog : MonoBehaviour {
	public Transform hollowLog;
	public Transform leftHandle;
	public Transform rightHandle;

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetLayerWeight (1, 1);
		bool isHolding = anim.GetCurrentAnimatorStateInfo (1).IsName ("HoldLog.HoldLog");
		hollowLog.localScale = isHolding ?
			new Vector3 (.2f, .2f, .4f) :
				new Vector3 (.0001f, .0001f, .0001f);
	}

	void OnAnimatorIK(int layerIndex){
		if (!enabled) {
			return;
		}
		if (layerIndex == 1) { //HoldLog Layer
			float ikWeight = anim.GetCurrentAnimatorStateInfo (1).IsName ("HoldLog.HoldLog") ? 1 : 0;
			anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandle.transform.position);
			anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandle.transform.rotation);
			anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
			anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);

			anim.SetIKPosition(AvatarIKGoal.RightHand, leftHandle.transform.position);
			anim.SetIKRotation(AvatarIKGoal.RightHand, leftHandle.transform.rotation);
			anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
			anim.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
		}
	}
}
