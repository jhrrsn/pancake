using UnityEngine;
using System.Collections;

public class VillagerBehaviour : MonoBehaviour {

	public float detectDistance;
	public float runSpeed;
	public int maxHealth = 2;
	public LayerMask wolfLayer;
	public bool flight = true;
	public float lerpSpeed;

	private int health;	
	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		health = maxHealth;
	}

	void FixedUpdate () {
		Collider2D [] nearbyWolves = Physics2D.OverlapCircleAll(transform.position, detectDistance, wolfLayer);

		Vector2 wolfVector = Vector2.zero;
		int wolfCount = 0;

		foreach (Collider2D wolf in nearbyWolves) {
			Transform t = wolf.transform;
			wolfCount++;
			wolfVector += (Vector2)t.transform.position - (Vector2)transform.position;
		}

		wolfVector /= wolfCount;
		wolfVector = wolfVector.normalized;

		Debug.DrawRay (transform.position, wolfVector);

		if (flight) {
			Vector2 fleeVector = -wolfVector * runSpeed;
			rb.velocity = Vector2.Lerp (rb.velocity, fleeVector, lerpSpeed * Time.deltaTime);
		}
	}

	public bool Attacked(int damage) {
		health -= damage;
		if (health <= 0) {
			Die ();
			return true;
		} else {
			return false;
		}
	}

	void Die() {
		Destroy (gameObject);
	}
}
