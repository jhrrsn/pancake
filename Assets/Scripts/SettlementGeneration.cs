using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettlementGeneration : MonoBehaviour {

	public int roadLength;
	public GameObject[] buildingObjects;
	public GameObject road;
	public GameObject villager;
	public GameObject marshall;

	[Range(0, 1)]
	public float marshallChance;
	public float[] buildingSizes;
	public int buildingSpacerSize;
	public int buildingGapSize;
	public float roadOffset;
	public float roadRotation;

	[Range (10, 90)]
	public float buildingDensity;
	

//	void Start () {
//		SpawnBuildings ();
//	}

	void SpawnBuildings () {
		InstantiateBuildings (GenerateRowBuildings (), true);
		InstantiateBuildings (GenerateRowBuildings (), false);

		GameObject newRoad = (GameObject) Instantiate(road, transform.position, transform.rotation);
		Vector3 roadSize = newRoad.transform.localScale;
		roadSize.x = roadLength * 0.8f;
		newRoad.transform.localScale = roadSize;

		Vector2 roadPositon = newRoad.transform.position;
		roadPositon.x -= roadLength / 4f;
		newRoad.transform.position = roadPositon;

		newRoad.transform.parent = this.transform;

		transform.Rotate(0, 0, roadRotation);
//		transform.RotateAround(
		
	}

	List<int> GenerateRowBuildings () {
		// Create list to store the buildings.
		List<int> buildings = new List<int>();

		// Variable to track the total length of the row as we add buildings/space.
		float rowLength = 0;

		// Bool to ensure we don't have successful spacers.
		bool previousSpacer = false;

		while (true) {
			if (Random.value * 100f < buildingDensity || previousSpacer) {
				// Pick one of the building sizes at random from the supplied list.
				int building = Random.Range(0, buildingSizes.Length);
				float buildingLength = (float) buildingSizes.GetValue(building);
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
		cursorPosition.x -= roadLength / 2f;

		foreach (int b in buildingList) {
			if (b < 99) {
				float buildingSize = buildingSizes[b];

				// Move cursor along.
				Vector2 newCursor = cursorPosition;
				newCursor.x += buildingSize/2.0f;
				cursorPosition = newCursor;

				if (Random.Range(0f, 1f) < marshallChance) {
					GameObject newMarshall = (GameObject) Instantiate(marshall, cursorPosition, Quaternion.identity);
					newMarshall.transform.parent = this.transform;
				} else {
					GameObject newVillager = (GameObject) Instantiate(villager, cursorPosition, Quaternion.identity);
					newVillager.transform.parent = this.transform;
				}

				float yValue;

				if (sideA) yValue = transform.position.y + buildingSize/2.0f + roadOffset;
				else yValue = transform.position.y -roadOffset - buildingSize/2.0f;

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

	void SetSettlementRotation (float rot) {
		roadRotation = rot;
	}

	void SetSettlementSize (int size) {
		roadLength = size;
	}
}
