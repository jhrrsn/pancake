using UnityEngine;
using System.Collections;

public class WolfBehaviour : MonoBehaviour {

	public float maxSpeed;
	public int activeDistance;
	public int inactiveDistance;

	public float pingFrequency;

	public float alignmentWeight;
	public float alignmentDistance;
	public float cohesionWeight;
	public float cohesionDistance;
	public float separationWeight;
	public float separationDistance;
	public float targetWeight;
	public float targetDistanceMod;
	public float catchUpWeight;

	public float pursuitDamper;
	public LayerMask villagerLayer;

	public float velocityLerp;

	private bool active;
	private bool pursuing;
	private float noiseOffset;
	private Rigidbody2D rb;
	private Transform target;
	private Transform villager;
	private Rigidbody2D targetRb;
	private WolfStatController stats;
	private WolfpackStrengthController wpsController;
	private Animator anim;
	private bool moving;
	private int id;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		target = GameObject.Find ("AlphaWolf").GetComponent<Transform>();
		targetRb = target.GetComponent<Rigidbody2D> ();
		stats = GetComponent<WolfStatController> ();
		wpsController = GameObject.Find("GameController").GetComponent<WolfpackStrengthController> ();
		noiseOffset = Random.value * 10.0f;
		active = false;
		pursuing = false;
		anim = GetComponent<Animator> ();
		moving = false;
		id = gameObject.GetInstanceID ();
		StartCoroutine("StartTick", pingFrequency);
	}

	void Update () {
		float targetDistance = Vector2.Distance (transform.position, target.transform.position);

		// Active check.
		if (active && targetDistance > inactiveDistance) {
			active = false;
			wpsController.RemoveWolf (id);
		} else if (!active && targetDistance <= activeDistance) {
			active = true;
			wpsController.AddWolf (id, stats.GetPower());
		}

		// Animation
		if (!moving && rb.velocity.sqrMagnitude > 0) {
			moving = true;
			anim.SetBool ("moving", true);
		} else if (moving && rb.velocity.sqrMagnitude < 3f) {
			moving = false;
			anim.SetBool ("moving", false);
		}
	}

	void FixedUpdate() {
		if (!active && rb.velocity.sqrMagnitude > 0.1f && !pursuing) {
			Vector2 newVelocity = rb.velocity;
			newVelocity *= 0.1f;
			rb.velocity = newVelocity;
		} else if (!active && !pursuing) {
			rb.velocity = Vector2.zero;
		}
	}

	private IEnumerator StartTick(float freq){
		
		yield return new WaitForSeconds(freq);

		EvaluateSeparation ();
		LookAround ();
		
	}

	void LookAround() {
		// if there's a villager, chase him! else flock
		Collider2D [] nearbyVillagers = new Collider2D[0];

		if (!villager) {
			pursuing = false;
		}

		if (!pursuing) {
			nearbyVillagers = Physics2D.OverlapCircleAll (transform.position, activeDistance, villagerLayer);
		}

		if ((pursuing && villager) || nearbyVillagers.Length > 0) {
			// Pursuing

			if (!pursuing) {
				pursuing = true;
				float closestDistace = Vector2.Distance(transform.position, nearbyVillagers[0].transform.position);
				villager = nearbyVillagers[0].transform;
				foreach (Collider2D v in nearbyVillagers) {
					float distance = Vector2.Distance(transform.position, v.transform.position);
					if (distance < closestDistace) {
						villager = v.transform;
					}
				}
			}

			Vector2 targetVector = villager.transform.position - transform.position;
			targetVector = targetVector.normalized;
			float noise = 1f + (Mathf.PerlinNoise (Time.time, noiseOffset) - 0.5f);
			Vector2 pursueVelocity = targetVector * noise * maxSpeed / pursuitDamper;
			rb.velocity = Vector2.Lerp (rb.velocity, pursueVelocity, velocityLerp);
		} else {
			// Flocking

			GameObject[] wolves = GameObject.FindGameObjectsWithTag ("wolf");
			Vector2 alignment = Vector2.zero;
			float alignmentCount = 0;
			Vector2 cohesion = Vector2.zero;
			float cohesionCount = 0;
			Vector2 separation = Vector2.zero;
			float separationCount = 0;

			foreach (GameObject wolf in wolves) {
				if (wolf != this.gameObject) {
					float distance = Vector2.Distance (transform.position, wolf.transform.position);
					Rigidbody2D wolf_rb = wolf.GetComponent<Rigidbody2D> ();

					// Alignment!
					if (distance < alignmentDistance) {
						alignmentCount++;
						alignment += wolf_rb.velocity;
					}

					// Cohesion!
					if (distance < cohesionDistance) {
						cohesionCount++;
						cohesion += wolf_rb.position;
					}

					// Separation!
					if (distance < separationDistance) {
						separationCount++;
						Vector2 diff = wolf_rb.position - (Vector2)transform.position;
						separation += diff;
					}
				}
			}

			if (alignmentCount > 0) {
				// Alignment!
				alignment = alignment / alignmentCount;
				alignment = alignment.normalized;
			}
		 
			if (cohesionCount > 0) {
				// Cohesion!
				cohesion = cohesion / cohesionCount;
				cohesion = cohesion - (Vector2)transform.position;
				cohesion = cohesion.normalized;
			}

			if (separationCount > 0) {
				// Separation!
				separation = separation / separationCount;
				separation *= -1f;
				separation = separation.normalized;
			}
			
			float targetDistance = Vector2.Distance (transform.position, target.transform.position) + targetDistanceMod;
			float distanceWeighting = map (targetDistance, 1f, 20f, 1f, catchUpWeight);
			Vector2 targetVector = target.position - transform.position;
			targetVector = targetVector.normalized;

			Vector2 newVelocity = targetVector * targetWeight + alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight;
			newVelocity = newVelocity.normalized;

			float noise = 1f + (Mathf.PerlinNoise (Time.time, noiseOffset) - 0.5f);
			float speed = Mathf.Clamp ((targetRb.velocity.magnitude * distanceWeighting * noise), 0f, maxSpeed);
			rb.velocity = Vector2.Lerp (rb.velocity, newVelocity * speed, velocityLerp);
		}

		// Set look rotation.
		Vector2 look = rb.velocity.normalized;
		var angle = Mathf.Atan2 (look.y, look.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (angle - 90, Vector3.forward);

		StartCoroutine("StartTick", pingFrequency);
	}

	void EvaluateSeparation () {
		float targetVelocity = targetRb.velocity.sqrMagnitude;
		separationDistance = map (targetVelocity, 10f, 25f, 2.5f, 1.5f);
	}

	void StopPursuing() {
		pursuing = false;
	}

	public bool GetActive () {
		return active;
	}

	float map(float value, float a1, float a2, float b1, float b2)
	{
		return b1 + (value-a1)*(b2-b1)/(a2-a1);
	}

}
