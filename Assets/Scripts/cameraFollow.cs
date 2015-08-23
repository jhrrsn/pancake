using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {


	private float watchZ = -10f;
	private float watchX = 10f;
	private float watchY = 10f;

	private Transform target;
	private Camera thisCamera;
	public float zoomSpeed = 20f;
	public float minZoomFOV = 1f;
	public float maxZoomFOV = 100f;

	private bool isOrthographic;
	float camZoom;
	
	void Start(){
		thisCamera = GetComponent<Camera> ();
		target = GameObject.Find ("AlphaWolf").GetComponent<Transform>();
		if(thisCamera.orthographic){
			isOrthographic = true;
			camZoom = thisCamera.fieldOfView;
		}else{
			isOrthographic = false;
			camZoom = thisCamera.orthographicSize;
		}
	}
	
	// HAndle for covering both camera types in the future
	void Update(){
		if(isOrthographic){
			camZoom = thisCamera.fieldOfView;
		}else{
			camZoom = thisCamera.orthographicSize;
		}
		
		if(Input.GetKeyDown("e")){
			zoomTo(500);
		}else if(Input.GetKeyDown("q")){
			zoomTo (1);
		}
	}
	
	void LateUpdate(){
		if(target){
			watchX = target.transform.position.x;
			watchY = target.transform.position.y;
			transform.position = new Vector3(watchX, watchY, watchZ);
		}
	}
	
	// TO DO: Dynamic resizing
	void dynamicResize(){
		
	}

	public void zoomTo(float zoomPoint){
		float difference;
		if(thisCamera.orthographicSize != zoomPoint){
			if(zoomPoint>maxZoomFOV){
				zoomPoint = maxZoomFOV;
			}else if(zoomPoint<minZoomFOV){
				zoomPoint = minZoomFOV;
			}
			difference = Mathf.Abs(zoomPoint-thisCamera.orthographicSize);
			for(int i=0; i<10;i++){
				if(thisCamera.orthographicSize < zoomPoint){
					thisCamera.orthographicSize += difference/10;
				}else{
					thisCamera.orthographicSize -= difference/10;
				}
			}
		}
	}
	
	public void zoomTo(float zoomPoint, int smoothing){
		float difference;
		if(thisCamera.orthographicSize != zoomPoint){
			if(zoomPoint>maxZoomFOV){
				zoomPoint = maxZoomFOV;
			}else if(zoomPoint<minZoomFOV){
				zoomPoint = minZoomFOV;
			}
			difference = Mathf.Abs(zoomPoint-thisCamera.orthographicSize);
				for(int i=0; i<smoothing;i++){
					if(thisCamera.orthographicSize < zoomPoint){
						thisCamera.orthographicSize += difference/smoothing;
					}else{
						thisCamera.orthographicSize -= difference/smoothing;
					}
				}
			}
	}
	
	public void ZoomIn()
	{
		if (thisCamera.orthographicSize > minZoomFOV){
			thisCamera.orthographicSize -= zoomSpeed/8;
		}
	}
	
	public void ZoomOut()
	{
		if (thisCamera.orthographicSize < maxZoomFOV){
			thisCamera.orthographicSize += zoomSpeed/8;
		}
	}
	
}
