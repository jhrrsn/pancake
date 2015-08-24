using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveForce;
	public float lerpSpeed;
	public float mouseDeadzone;

	private Rigidbody2D rb;
	private Animator anim;
	private bool moving;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		moving = false;
	}

	void FixedUpdate() {
		Debug.Log (rb.velocity.sqrMagnitude);
		TestingLinesToCaves ();
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		// Set velocity.
		Vector2 dir = new Vector2 (h, v);
		dir = dir.normalized;
		Vector2 newVelocty = dir * moveForce;
		rb.velocity = Vector2.Lerp (rb.velocity, newVelocty, lerpSpeed * Time.deltaTime);

		// Set rotation.
		var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;

		if (Mathf.Abs (h) > 0 || Mathf.Abs (v) > 0) {
			// If moving, set anim parameter
			if (!moving) {
				moving = true;
				anim.SetBool ("moving", true);
			}
			Quaternion newRotation = Quaternion.AngleAxis (angle - 90, Vector3.forward);
			transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, lerpSpeed * Time.deltaTime);
		} else {
			if (moving) {
				moving = false;
				anim.SetBool ("moving", false);
			}
		}
	}

	void TestingLinesToCaves () {
		GameObject[] caves = GameObject.FindGameObjectsWithTag ("cave");
		foreach (GameObject cave in caves) {
			Vector2 cavePosition = cave.transform.position;
			float distance = Vector2.Distance(transform.position, cavePosition);
			float alphaValue = map (distance, 0f, 100f, 1f, 0.05f);
			Color lineColour = new Color(1f, 1f, 1f, alphaValue);
			Debug.DrawLine(transform.position, cavePosition, lineColour);
		}
	}

	float map(float value, float a1, float a2, float b1, float b2)
	{
		return b1 + (value-a1)*(b2-b1)/(a2-a1);
	}
}
