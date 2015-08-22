using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public Transform target;
	private float watchZ = -10f;
	private float watchX = 10f;
	private float watchY = 10f;

	public Camera thisCamera;
	public float zoomSpeed = 20f;
	public float minZoomFOV = 1f;
	public float maxZoomFOV = 100f;
	
	void LateUpdate(){
		if(target){
			watchX = target.GetComponent<Rigidbody>().transform.position.x;
			watchY = target.GetComponent<Rigidbody>().transform.position.y;
			transform.position = new Vector3(watchX, watchY, watchZ);
		}
	}
	
	// TO DO: Dynamic resizing
	void dynamicResize(){
		
	}

	public void zoomTo(float zoomPoint){
		float difference;
		if(thisCamera.fieldOfView != zoomPoint){
			if(zoomPoint>maxZoomFOV){
				zoomPoint = maxZoomFOV;
			}else if(zoomPoint<minZoomFOV){
				zoomPoint = minZoomFOV;
			}
			difference = Mathf.Abs(zoomPoint-thisCamera.fieldOfView);
			for(int i=0; i<10;i++){
				if(thisCamera.fieldOfView < zoomPoint){
					thisCamera.fieldOfView += difference/10;
				}else{
					thisCamera.fieldOfView -= difference/10;
				}
			}
		}
	}
	
	public void zoomTo(float zoomPoint, int smoothing){
		float difference;
		if(thisCamera.fieldOfView != zoomPoint){
			if(zoomPoint>maxZoomFOV){
				zoomPoint = maxZoomFOV;
			}else if(zoomPoint<minZoomFOV){
				zoomPoint = minZoomFOV;
			}
			difference = Mathf.Abs(zoomPoint-thisCamera.fieldOfView);
				for(int i=0; i<smoothing;i++){
					if(thisCamera.fieldOfView < zoomPoint){
						thisCamera.fieldOfView += difference/smoothing;
					}else{
						thisCamera.fieldOfView -= difference/smoothing;
					}
				}
			}
	}
	
	public void ZoomIn()
	{
		if (thisCamera.fieldOfView > minZoomFOV){
			thisCamera.fieldOfView -= zoomSpeed/8;
		}
	}
	
	public void ZoomOut()
	{
		if (thisCamera.fieldOfView < maxZoomFOV){
			thisCamera.fieldOfView += zoomSpeed/8;
		}
	}
	
}
