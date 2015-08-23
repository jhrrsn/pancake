using UnityEngine;
using System.Collections;

public class WolfStatController : MonoBehaviour {

	public int startingPower = 1;
	public float biteDelay = 1f;

	private int power;
	private int health;
	private float nextBite;
	private WolfpackStrengthController wpsController;

	void Start () {
		power = startingPower;
		health = power;
		float size = 0.3f + 3f * (power / 100f);
		nextBite = Time.time;
		transform.localScale = new Vector2(size, size);
		wpsController = GameObject.Find("GameController").GetComponent<WolfpackStrengthController> ();
	}

	void OnCollisionStay2D (Collision2D coll) {
		if (Time.time > nextBite && coll.gameObject.tag == "villager") {
			bool killedIt = coll.gameObject.GetComponent<VillagerBehaviour> ().Attacked(power);
			if (killedIt) {
				LevelUp ();
				gameObject.BroadcastMessage("StopPursuing");
			}

			nextBite = Time.time + biteDelay;
		}
	}

	void LevelUp () {
		power += 1;
		float size = 0.4f + 3f * (power / 100f);
		transform.localScale = new Vector2(size, size);
		wpsController.IncreaseStrength (1);
	}

	public int GetPower() {
		return power;
	}
}
