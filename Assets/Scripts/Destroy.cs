using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {
	public float health = 10;

	private void OnCollisionEnter2D(Collision2D collisionData) {
		OnTriggerEnter2D(collisionData.collider);
	}

	private void OnTriggerEnter2D(Collider2D collisionData) {
		if (collisionData.gameObject.CompareTag ("Projectile")) {
			if (collisionData.gameObject.name == "fireball(Clone)"){
				health -= 2;
			}
			if (collisionData.gameObject.name == "iceball(Clone)") {
				health -= 1;
			}
            Destroy(collisionData.gameObject);
			if (health < 1) {
				Destroy (gameObject);
			}

		}
	}
}
