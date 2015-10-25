using UnityEngine;
using System.Collections;

public class PlayerFall : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (transform.position.y < -5) {
			transform.position = new Vector3(0, 25, 3);
		}
	}
}
