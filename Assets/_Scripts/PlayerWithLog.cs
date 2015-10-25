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
}
