using UnityEngine;
using System.Collections;

public class slaveMovement : MonoBehaviour {
	
//	private float offsetX = 0;
//	private float offsetY = 0;
//	private Vector3 offset;		
//	private bool isFollowing;
//	
//	public masterMovement master;
//			
//	// Use this for initialization
//	void Start () {
//		isFollowing = false;
//	}
//	
//	void OnTriggerEnter(){
//		print ("Detected");
//		isFollowing = true;
//		offsetX = transform.position.x;
//		offsetY = transform.position.y;
//		offset = new Vector3(offsetX, offsetY, 0);	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		// Update object transform
//		if(isFollowing){
//			this.transform.position = master.transform.position + offset;
//		}	
//	}
	public int moveSpeed;
	public int minDistance;

	private Rigidbody2D rb;
	
	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {

	}

	void LookAround() {
		GameObject[] wolves = GameObject.FindGameObjectsWithTag ("wolf");
		float wolfCount = 0;
		float rotation = 0;

		foreach (GameObject wolf in wolves) {
			float distance = Vector2.Distance(transform.position, wolf.transform.position);
			if (distance < minDistance) {
				wolfCount++;
				Rigidbody2D wolf_rb = wolf.GetComponent<Rigidbody2D>();
				rb.velocity.x += wolf_rb.velocity.x;
				rb.velocity.y += wolf_rb.velocity.y;
			}
		}

		if (wolfCount > 0) {
			rb.velocity.x = rb.velocity.x/wolfCount;
		}
	}
}
