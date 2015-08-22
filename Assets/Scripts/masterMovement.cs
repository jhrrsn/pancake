using UnityEngine;
using System.Collections;

public class masterMovement : MonoBehaviour {
	
	public float maxSpeed = 0f;
	private float moveX = 0f;
	private float moveY = 0f;
	private float speed = 0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		moveX = 0f;
		moveY = 0f;		
		
		// Basic movement controls
		if(Input.GetKey("a")){
			moveX = -1;
		}
		if(Input.GetKey("d")){
			moveX = 1;
		}
		if(Input.GetKey("w")){
			moveY = 1;
		} 
		if(Input.GetKey("s")){
			moveY = -1;
		}
		
		// Acceleration
		if(moveX != 0f || moveY!=0f){
			if(speed<maxSpeed){
				speed += 0.001f;
			}
		}else{
			speed = 0;
		}
			
		transform.Translate(moveX * speed, moveY * speed, 0);
			
	}
}
	