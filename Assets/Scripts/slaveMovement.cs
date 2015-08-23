using UnityEngine;
using System.Collections;

public class slaveMovement : MonoBehaviour {
	
	private float offsetX = 0;
	private float offsetY = 0;
	private Vector3 offset;		
	private bool isFollowing;
	
	public masterMovement master;
			
	// Use this for initialization
	void Start () {
		isFollowing = false;
	}
	
	void OnTriggerEnter(){
		print ("Detected");
		isFollowing = true;
		offsetX = transform.position.x;
		offsetY = transform.position.y;
		offset = new Vector3(offsetX, offsetY, 0);	
	}
	
	// Update is called once per frame
	void Update () {
		// Update object transform
		if(isFollowing){
			this.transform.position = master.transform.position + offset;
		}	
	}
}
