using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {

	const float damageAmount = .25f;

	void OnCollisionEnter (Collision collider){
		PlayerHurt player = collider.gameObject.GetComponent<PlayerHurt>();
		if (player) {
			player.DoDamage (damageAmount);
		}
	}
}
