using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettlementGeneration : MonoBehaviour {

	public int roadLength;
	public int[] buildingSizes;
	public int buildingSpaceSize;

	[Range (10, 90)]
	public float buildingDensity;
	
//	private List<int> rightBuildings = new List<int>();
	private float roadOrientation;

	void Start () {
		List<int> testBuildings = GenerateRowBuildings ();
		foreach (int b in testBuildings) {
			Debug.Log(b);
		}
	}

	List<int> GenerateRowBuildings () {
		// Create list to store the buildings.
		List<int> buildings = new List<int>();

		// Variable to track the total length of the row as we add buildings/space.
		int rowLength = 0;

		// Bool to ensure we don't have successful spacers.
		bool previousSpacer = false;

		while (true) {
			if (Random.value * 100f < buildingDensity || previousSpacer) {
				// Pick one of the building sizes at random from the supplied list.
				int building = Random.Range(0, buildingSizes.Length);
				int buildingLength = (int) buildingSizes.GetValue(building);
				if ((rowLength + buildingLength) > roadLength) {
					break;
				} else {
					// Add selected building to array.
					buildings.Add(building);
					rowLength += buildingLength;
					previousSpacer = false;
				}
			} else {
				if ((rowLength + buildingSpaceSize) > roadLength) {
					break;
				} else {
					// Add spacer ID (99) to array.
					buildings.Add(99);
					rowLength += buildingSpaceSize;
					previousSpacer = true;
				}
			}
		}

		return buildings;
	}
}
