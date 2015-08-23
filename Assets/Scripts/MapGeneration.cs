using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

	[Range(0, 100)]
	public float settlementDensity;

	[Range(0, 100)]
	public float caveDensity;

	public int gridDensity;
	public int mapSize;
	public GameObject settlement;
	public GameObject wolfCave;
	public GameObject map;

	private int grid;
	private int[,] populatedGrid;

	void Start () {
		grid = mapSize / gridDensity;
		populatedGrid = new int[grid, grid];
		float sumDensities = settlementDensity + caveDensity;
		settlementDensity /= sumDensities;
		caveDensity /= sumDensities;
		PopulateGrid ();
		InstantiateObjects ();
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void PopulateGrid () {
		for (int i = 0; i < grid; i++) {
			for (int j = 0; j < grid; j++) {
				if (i % grid == 0 || j % grid == 0) {
					populatedGrid[i,j] = 0;
				} else if (Random.value < caveDensity) {
					populatedGrid[i,j] = 2;
				} else if (Random.value < settlementDensity) {
					populatedGrid[i,j] = 1;
				} else {
					populatedGrid[i,j] = 0;
				}
			}
		}
	}

	void InstantiateObjects () {
		for (int i = 0; i < grid; i++) {
			for (int j = 0; j < grid; j++) {
				// Settlements
				if (populatedGrid[i,j] == 1) {
					int settlementSize = Random.Range(3, 8) * 10;
					int settlementRotation = Random.Range(0, 359);
					Vector2 settlementPosition = new Vector2((i * gridDensity)-mapSize/2f, (j * gridDensity)-mapSize/2f);
					float xOffset = Random.Range(-gridDensity/5f, gridDensity/5f);
					float yOffset = Random.Range(-gridDensity/5f, gridDensity/5f);
					Vector2 settlementOffset = new Vector2(xOffset, yOffset);
					GameObject newSettlement = (GameObject) Instantiate (settlement, settlementPosition + settlementOffset, transform.rotation);
					newSettlement.transform.parent = map.transform;
					SettlementGeneration sGenr = newSettlement.GetComponent<SettlementGeneration>();
					sGenr.BroadcastMessage("SetSettlementRotation", settlementRotation);
					sGenr.BroadcastMessage("SetSettlementSize", settlementSize);
					sGenr.BroadcastMessage("SpawnBuildings");
				} else if (populatedGrid[i,j] == 2) {
					Quaternion caveRotation = Quaternion.AngleAxis (Random.Range(0, 359), Vector3.forward);
					Vector2 cavePosition = new Vector2((i * gridDensity)-mapSize/2f, (j * gridDensity)-mapSize/2f);
					float xOffset = Random.Range(-gridDensity/2.5f, gridDensity/2.5f);
					float yOffset = Random.Range(-gridDensity/2.5f, gridDensity/2.5f);
					Vector2 caveOffset = new Vector2(xOffset, yOffset);
					GameObject newCave = (GameObject) Instantiate (wolfCave, cavePosition + caveOffset, caveRotation);
					newCave.transform.parent = map.transform;
				}
			}
		}
	}
}
