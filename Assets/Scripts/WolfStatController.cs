using UnityEngine;
using System.Collections;

public class WolfStatController : MonoBehaviour {

	public int startingPower = 1;
	public float biteDelay = 1f;
	public float baseSize = 0.5f;

	public AudioClip biteClip;
	private AudioSource sfx;

	public int power;
	private int xp;
	private int health;
	private float nextBite;
	private WolfpackStrengthController wpsController;
	private int id;

	void Start () {
		id = gameObject.GetInstanceID ();
		power = startingPower;
		health = power;
		float size = baseSize + 6f * (power / 100f);
		xp = 0;
		nextBite = Time.time;
		transform.localScale = new Vector2(size, size);
		sfx = GetComponent<AudioSource> ();
		wpsController = GameObject.Find("GameController").GetComponent<WolfpackStrengthController> ();
	}

	void OnCollisionStay2D (Collision2D coll) {
		string colliderTag = coll.gameObject.tag;
		if (Time.time > nextBite && (colliderTag == "villager" || colliderTag == "marshall")) {
			sfx.pitch = Random.Range(0.9f, 1.1f);
			sfx.PlayOneShot(biteClip);
			bool killedIt = false;
			if (colliderTag == "villager") {
				killedIt = coll.gameObject.GetComponent<VillagerBehaviour> ().Attacked(power);
			} else if (colliderTag == "marshall") {
				killedIt = coll.gameObject.GetComponent<MarshallBehaviour> ().Attacked(power);
			}
			if (killedIt) {
				xp++;
				if (xp % power == 0) {
					LevelUp ();
					xp = 0;
				}
				gameObject.BroadcastMessage("StopPursuing");
			}

			nextBite = Time.time + biteDelay;
		}
	}

	void LevelUp () {
		power += 1;
		health += 1;
		float size = baseSize + 6f * (power / 100f);
		transform.localScale = new Vector2(size, size);
		wpsController.UpdateWolf (id, power);
	}

	public int GetPower() {
		return power;
	}
}
