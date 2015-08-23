using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WolfpackStrengthController : MonoBehaviour {

	public Text strengthText;

	private int wolfpackStrength; 

	void Start () {
		wolfpackStrength = 1;
	}

	public void IncreaseStrength(int s) {
		wolfpackStrength += s;
		strengthText.text = wolfpackStrength.ToString ();
	}

	public void ReduceStrength(int s) {
		wolfpackStrength -= s;
		strengthText.text = wolfpackStrength.ToString ();
	}

	void Update () {
		Debug.Log (wolfpackStrength);
	}
}
