using UnityEngine;
using System.Collections;

public class MarshallBehaviour : MonoBehaviour {


	// fixed tether
	// pursues wolves, but attempts to maintain a distance
	// always faces wolves within proximity
	// if wolf is in range, shoot at a fixed (with random modifier) interval
	// if health is low, flee!

	public float detectDistance;
	public float shootDistance;
	public int maxHealth = 2;
	public LayerMask wolfLayer;
	public float lerpSpeed;
	public AudioClip shotClip;
	public AudioClip reloadClip;
	public AudioClip deathClip;
	public float shotDelay;
	public int shotDamage;
	public Sprite deathSprite;

	private bool alive;
	private int health;	
	private bool reloaded;
	private Rigidbody2D rb;
	private Transform targetWolf;
	private float nextShot;
	private AudioSource sfx;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		sfx = GetComponent<AudioSource> ();
		nextShot = Time.time;
		reloaded = true;
		alive = true;
		health = maxHealth;
	}

	void FixedUpdate () {
		if (alive) {
			Collider2D [] nearbyWolves = Physics2D.OverlapCircleAll (transform.position, detectDistance, wolfLayer);

			if (nearbyWolves.Length > 0) {

				float closestDistance = Vector2.Distance (transform.position, nearbyWolves [0].transform.position);
				targetWolf = nearbyWolves [0].transform;

				foreach (Collider2D wolf in nearbyWolves) {
					float distance = Vector2.Distance (transform.position, wolf.transform.position);
					if (distance < closestDistance) {
						targetWolf = wolf.transform;
					}
				}

				Vector2 wolfVector = targetWolf.transform.position - transform.position;
				wolfVector = wolfVector.normalized;

				// Set look rotation.
				Vector2 look = wolfVector;
				var angle = Mathf.Atan2 (look.y, look.x) * Mathf.Rad2Deg;
				Quaternion newRotation = Quaternion.AngleAxis (angle - 90, Vector3.forward);
				transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, lerpSpeed);

				// Take a shot, if able.
				if (Time.time > nextShot && closestDistance < shootDistance && targetWolf.name != "AlphaWolf") {
					nextShot = Time.time + shotDelay;
					reloaded = false;
					targetWolf.GetComponent<WolfStatController> ().Attacked (shotDamage);
					sfx.PlayOneShot (shotClip);
				} else if (!reloaded && Time.time > nextShot - 2f) {
					sfx.PlayOneShot (reloadClip);
					reloaded = true;
				}
			}
		}
	}

	public bool Attacked(int damage) {
		health -= damage;
		if (health <= 0) {
			Die ();
			sfx.PlayOneShot(deathClip);
			return true;
		} else {
			return false;
		}
	}

	void Die() {
		GetComponent<Collider2D> ().enabled = false;
		SpriteRenderer spr = GetComponent<SpriteRenderer> ();
		spr.sprite = deathSprite;
		alive = false;
		gameObject.tag = "dead";
	}
}
