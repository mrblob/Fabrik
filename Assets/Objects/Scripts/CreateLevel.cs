using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for Lists
using System.Linq; // for Last() Function

public class CreateLevel : MonoBehaviour {



	public GameObject Tile;
	
	public int mapSize;

	public GameObject[,] allTiles;

	private GameObject currentTile;


	public GameObject Bot;
	public GameObject Building_s;
	public GameObject Building_m;
	public GameObject Building_l;
	public GameObject Hub;


	public GameObject[] Buildables;

	public List<GameObject> allBuildables;
	public int buildableCount;


	// Use this for initialization
	void Start () {

		//Build Level

		mapSize = 9;
		allTiles = new GameObject[mapSize,mapSize]; //2-dimensional Array


		for(int i=0; i < mapSize; i++){

			for(int j=0; j < mapSize; j++){

				Tile.name = i+"-"+j;

				currentTile = Instantiate (Tile, new Vector3 (i*20,0,j*20), Quaternion.identity) as GameObject;

				currentTile.GetComponent<TileBehavior>().idX = i;
				currentTile.GetComponent<TileBehavior>().idZ = j;

				allTiles[i,j] = currentTile;


			}
		}


		allBuildables = new List<GameObject>();
		Buildables = new GameObject[5];

		Buildables[0] = Bot;
		Buildables[1] = Building_s;
		Buildables[2] = Building_m;
		Buildables[3] = Building_l;
		Buildables[4] = Hub;

		buildableCount = 0;


	}// start


	void Update() {



	}


	public void addBuildable (int x, int z, int type, Quaternion pos, int dir) {

		buildableCount++;

		GameObject newBuildable = Instantiate (Buildables[type], allTiles[x,z].transform.position, pos) as GameObject;
		BuildableBehavior build = newBuildable.GetComponent<BuildableBehavior>();

		//Construction
		build.buildNumber = buildableCount;
		build.direction = dir;
		build.posX = x;
		build.posZ = z;
		build.registerSpace(x,z);

		allBuildables.Add (newBuildable);


	}


	public void removeBuildable (GameObject build){


		int x = build.GetComponent<BuildableBehavior>().posX;
		int z = build.GetComponent<BuildableBehavior>().posZ;
		build.GetComponent<BuildableBehavior>().unregisterSpace(x,z);

		allBuildables.Remove(build);

		if(build.tag == "Bot"){


			build.GetComponent<BotBehavior>().clearWaypoints();

		}

		Destroy (build);


	}



}
