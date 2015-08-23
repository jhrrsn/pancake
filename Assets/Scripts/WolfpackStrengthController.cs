using UnityEngine;
using System.Collections;

public class WolfpackStrengthController : MonoBehaviour {

	private int wolfpackStrength;

	void Start () {
		wolfpackStrength = 1;
	}

	public void IncreaseStrength(int s) {
		wolfpackStrength += s;
	}

	public void ReduceStrength(int s) {
		wolfpackStrength -= s;
	}

	void Update () {
		Debug.Log (wolfpackStrength);
	}
}
