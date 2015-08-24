using UnityEngine;
using System.Collections;

public class fXManager : MonoBehaviour {

	public AudioSource fxSource = null;                   //Reference to the audio source which will play the sound effects.
	public fXManager instance = null;     //Allows other scripts to call functions from fxManager.             	
	
	public AudioClip[] fxList;
	public AudioClip[] howlList;
	
	public float fxDelay = 10f;
	private float nextFx;
			
	void Awake ()
	{
		//Check if there is already an instance of fxManager
		if (instance == null){
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		}else if (instance != this){
			//Destroy this, this enforces our singleton pattern so there can only be one instance of fxManager.
			Destroy (gameObject);
		//Set fxManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
		}
		
		if(fxSource == null){
			fxSource = GetComponent<AudioSource>();
		}
	}
	
	void Start(){
		nextFx = Time.time + fxDelay + Random.Range(-fxDelay/2f, fxDelay/2f);	
	}

	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip)
	{				
		//Play the clip.
		fxSource.PlayOneShot (clip);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Howl")){
			RandomizeSfx(howlList, 1f);
		}
		
		if(Time.time > nextFx){
			RandomizeSfx(fxList, 0.5f);
			nextFx = Time.time + fxDelay + Random.Range(-fxDelay/2f, fxDelay/2f);
		}
		
	}
	
	//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
	public void RandomizeSfx (AudioClip[] clips, float volume)
	{
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Length);
		
		//Play the clip.
		fxSource.PlayOneShot(clips[randomIndex], volume);
	}
}
