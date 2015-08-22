using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettlementGeneration : MonoBehaviour {

	public int roadLength;
	public GameObject[] buildingObjects;
	public int[] buildingSizes;
	public int buildingSpacerSize;
	public int buildingGapSize;
	public int roadOffset;
	public float roadRotation;

	[Range (10, 90)]
	public float buildingDensity;
	
//	private List<int> rightBuildings = new List<int>();

	void Start () {
		InstantiateBuildings (GenerateRowBuildings (), true);
		InstantiateBuildings (GenerateRowBuildings (), false);
		transform.rotation = Quaternion.Euler(0, 0, roadRotation);
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Application.LoadLevel(Application.loadedLevel);
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
				if ((rowLength + buildingLength + buildingSpacerSize) > roadLength) {
					break;
				} else {
					// Add selected building to array.
					buildings.Add(building);
					rowLength += buildingLength+buildingSpacerSize;
					previousSpacer = false;
				}
			} else {
				if ((rowLength + buildingGapSize) > roadLength) {
					break;
				} else {
					// Add spacer ID (99) to array.
					buildings.Add(99);
					rowLength += buildingGapSize;
					previousSpacer = true;
				}
			}
		}

		return buildings;
	}

	void InstantiateBuildings(List<int> buildingList, bool sideA) {
		Vector2 cursorPosition = transform.position;

		foreach (int b in buildingList) {
			if (b < 99) {
				int buildingSize = buildingSizes[b];

				// Move cursor along.
				Vector2 newCursor = cursorPosition;
				newCursor.x += buildingSize/2.0f;
				cursorPosition = newCursor;

				float yValue;

				if (sideA) yValue = buildingSize/2.0f + roadOffset;
				else yValue = -roadOffset - buildingSize/2.0f;

				GameObject newBuilding = (GameObject) Instantiate(buildingObjects[b], new Vector2(cursorPosition.x, yValue), transform.rotation);
				newBuilding.transform.parent = this.transform;

				// Move cursor along.
				newCursor = cursorPosition;
				newCursor.x += buildingSize/2.0f;
				cursorPosition = newCursor;
			} else {
				Vector2 newCursor = cursorPosition;
				newCursor.x += buildingGapSize;
				cursorPosition = newCursor;
			}
		}
	}
}
