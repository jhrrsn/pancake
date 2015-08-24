using UnityEngine;
using System.Collections;

public class VillagerBehaviour : MonoBehaviour {

	public float detectDistance;
	public float runSpeed;
	public int maxHealth = 2;
	public LayerMask wolfLayer;
	public bool flight = true;
	public float lerpSpeed;
	public Color [] hairColours;
	public AudioClip [] deathClip;
	public Sprite deathSprite;

	private int health;	
	private Rigidbody2D rb;
	private Animator anim;
	private bool running;
	public bool alive;
	private AudioSource sfx;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sfx = GetComponent<AudioSource> ();
		GetComponent<SpriteRenderer> ().color = hairColours [Random.Range (0, hairColours.Length)];
		health = maxHealth;
		running = false;
		alive = true;
	}

	void Update () {
		// Animation
		if (alive && !running && rb.velocity.sqrMagnitude > 0) {
			running = true;
			anim.SetBool ("running", true);
		} else if (alive && running && rb.velocity.sqrMagnitude < 1f) {
			running = false;
			anim.SetBool ("running", false);
		}
	}

	void FixedUpdate () {
		if (alive) {
			Collider2D [] nearbyWolves = Physics2D.OverlapCircleAll (transform.position, detectDistance, wolfLayer);

			Vector2 wolfVector = Vector2.zero;
			int wolfCount = 0;

			foreach (Collider2D wolf in nearbyWolves) {
				Transform t = wolf.transform;
				wolfCount++;
				wolfVector += (Vector2)t.transform.position - (Vector2)transform.position;
			}

			wolfVector /= wolfCount;
			wolfVector = wolfVector.normalized;

			if (flight) {
				// Set fleeing velocity.
				Vector2 fleeVector = -wolfVector * runSpeed;
				rb.velocity = Vector2.Lerp (rb.velocity, fleeVector, lerpSpeed * Time.deltaTime);

				// Set look rotation.
				Vector2 look = rb.velocity.normalized;
				var angle = Mathf.Atan2 (look.y, look.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis (angle - 90, Vector3.forward);
			}
		}
	}

	public bool Attacked(int damage) {
		health -= damage;
		if (health <= 0) {
			Die ();
			sfx.pitch = Random.Range(0.9f, 1.1f);
			sfx.PlayOneShot(deathClip[Random.Range(0, deathClip.Length)]);
			return true;
		} else {
			return false;
		}
	}

	void Die() {
		GetComponent<Collider2D> ().enabled = false;
		GetComponent<Animator> ().enabled = false;
		SpriteRenderer spr = GetComponent<SpriteRenderer> ();
		spr.sprite = deathSprite;
		spr.color = Color.white;
		alive = false;
		gameObject.tag = "dead";
		rb.Sleep ();
//		Destroy (gameObject, 4.5f);
	}
}
