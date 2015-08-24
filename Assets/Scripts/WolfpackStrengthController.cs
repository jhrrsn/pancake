using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WolfpackStrengthController : MonoBehaviour {

	public Text strengthText;

	private Hashtable wolfpack;

	private int wolfpackStrength;

	void Start () {
		wolfpack = new Hashtable ();
		wolfpackStrength = 1;
	}

	public void AddWolf (int id, int power) {
		wolfpack.Add (id, power);
		CalculateStrength ();
	}

	public void RemoveWolf (int id) {
		wolfpack.Remove (id);
		CalculateStrength ();
	}

	public void UpdateWolf (int id, int power) {
		if (wolfpack.ContainsKey(id)) {
			wolfpack[id] = power;
			CalculateStrength ();
		}
	}

	void CalculateStrength () {
		wolfpackStrength = 1;

		foreach (int s in wolfpack.Values) {
			wolfpackStrength += s;
		};

		strengthText.text = wolfpackStrength.ToString ();
	}

	void Update () {
		Debug.Log (wolfpackStrength);
	}
}
