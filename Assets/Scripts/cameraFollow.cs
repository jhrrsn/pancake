using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public Transform target;
	private float watchZ = -10f;
	private float watchX = 10f;
	private float watchY = 10f;

	void LateUpdate(){
		if(target){
			float watchX = target.GetComponent<Rigidbody>().transform.position.x;
			float watchY = target.GetComponent<Rigidbody>().transform.position.y;
			transform.position = new Vector3(watchX, watchY, watchZ);
		}
		
		// Debug: Zoom in/out manually
		if(Input.GetKeyDown("e")){
			watchZ++;
		}else if(Input.GetKeyDown("q")){
			watchZ--;
		}
	}
	
	// TO DO: Dynamic resizing
	void dynamicResize(){
		
	}
}
