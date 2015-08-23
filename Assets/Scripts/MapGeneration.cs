using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

	[Range(0, 100)]
	public float settlementDensity;
	public int gridDensity;
	public int mapSize;
	public GameObject settlement;

	private int grid;
	private bool[,] settlements;

	void Start () {
		grid = mapSize / gridDensity;
		settlements = new bool[grid, grid];
		PopulateSettlements ();
		InstantiateSettlements ();
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void PopulateSettlements () {
		for (int i = 0; i < grid; i++) {
			for (int j = 0; j < grid; j++) {
				if (i % grid == 0 || j % grid == 0) {
					settlements[i,j] = false;
				} else if (Random.value * 100 < settlementDensity) {
					settlements[i,j] = true;
				} else {
					settlements[i,j] = false;
				}
			}
		}
	}

	void InstantiateSettlements () {
		for (int i = 0; i < grid; i++) {
			for (int j = 0; j < grid; j++) {
				if (settlements[i,j]) {
					int settlementSize = Random.Range(3, 8) * 10;
					int settlementRotation = Random.Range(0, 359);
					Vector2 settlementPosition = new Vector2((i * gridDensity)-mapSize/2f, (j * gridDensity)-mapSize/2f);
					GameObject newSettlement = (GameObject) Instantiate (settlement, settlementPosition, transform.rotation);
					SettlementGeneration sGenr = newSettlement.GetComponent<SettlementGeneration>();
					sGenr.BroadcastMessage("SetSettlementRotation", settlementRotation);
					sGenr.BroadcastMessage("SetSettlementSize", settlementSize);
					sGenr.BroadcastMessage("SpawnBuildings");
				}
			}
		}
	}
}
