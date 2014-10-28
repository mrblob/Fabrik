using UnityEngine;
using System.Collections;

public class BuildableBehavior : MonoBehaviour {

	public int buildNumber;

	public bool hasDock;
	public bool isHub;

	public int posX;
	public int posZ;

	public int sizeX;
	public int sizeZ;

	public int offsetX;
	public int offsetZ;

	public int startX;
	public int startZ;

	public int direction = 0;
	public int modX;
	public int modZ;



	// Use this for initialization
	void Start () {
	
		modX = 1;
		modZ = -1;

	}
	
	// Update is called once per frame
	void Update () {


	
	}
	

	public void rotateRight(){

		transform.Rotate (0,90.0f,0);

		if(direction < 3){

			direction++;

		}else{

			direction = 0;

		}

	}


	public void rotateLeft(){

		transform.Rotate (0,-90.0f,0);

		if(direction > 0){
			direction--;
		}else{
			direction = 3;
		}
	}


	public void setPossible(){

		Component[] allChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer child in allChildren) {

			child.material.color = Color.green;

		}

	}


	public void setImpossible(){
		
		Component[] allChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer child in allChildren) {
			
			child.material.color = Color.red;
			
		}
		
	}



	public bool checkSpace(int getX, int getZ){


		CreateLevel gamedata = GameObject.Find ("ScriptObject").GetComponent<CreateLevel>();



		if(isHub){


			if (direction == 0 && getX == gamedata.mapSize-1)	{	return true; }
			if (direction == 1 && getZ == 0)					{	return true; }
			if (direction == 2 && getX == 0)					{	return true; }
			if (direction == 3 && getZ == gamedata.mapSize-1)	{	return true; }

			return false;


		}



			//Rotation Modulation
			if (direction == 0) { modX = 1 	; modZ = -1	; startX = 	offsetX 	; startZ = offsetZ		;}
			if (direction == 1) { modX = -1 ; modZ = -1	; startX = 	offsetZ		; startZ = -offsetX 	;}
			if (direction == 2) { modX = -1 ; modZ = 1	; startX = -offsetX		; startZ = -offsetZ		;}
			if (direction == 3) { modX = 1 	; modZ = 1	; startX = -offsetZ		; startZ = offsetX		;}



			for (int i=0; i < sizeX; i++){

				for (int j=0; j <sizeZ; j++){
				

					int x = getX + startX + i * modX;
					int z = getZ + startZ + j * modZ;

					if(x >= gamedata.mapSize || x < 0 || z >= gamedata.mapSize || z < 0){

						return false;

					}

					if(gamedata.allTiles[x,z].GetComponent<TileBehavior>().isTaken){

						return false;

					}

				}//for
			}//for



		return true;



	}


	public void registerSpace(int getX, int getZ){


		CreateLevel gamedata = GameObject.Find ("ScriptObject").GetComponent<CreateLevel>();


		//Rotation Modulation
		if (direction == 0) { modX = 1 	; modZ = -1	; startX = 	offsetX 	; startZ = offsetZ		;}
		if (direction == 1) { modX = -1 ; modZ = -1	; startX = 	offsetZ		; startZ = -offsetX 	;}
		if (direction == 2) { modX = -1 ; modZ = 1	; startX = -offsetX		; startZ = -offsetZ		;}
		if (direction == 3) { modX = 1 	; modZ = 1	; startX = -offsetZ		; startZ = offsetX		;}


		for (int i=0; i < sizeX; i++){
			
			for (int j=0; j <sizeZ; j++){
				
				
				int x = getX + startX + i * modX;
				int z = getZ + startZ + j * modZ;
				
				
				gamedata.allTiles[x,z].GetComponent<TileBehavior>().isTaken = true;
				gamedata.allTiles[x,z].GetComponent<TileBehavior>().renderer.material.color = Color.black;

			}//for
		}//for



		//if the buildable has a dock, register it in the Tile instance
		if(hasDock){
			gamedata.allTiles[getX,getZ].GetComponent<TileBehavior>().addDock (buildNumber, gameObject);
		}

		
	}


	public void unregisterSpace(int getX, int getZ){



		CreateLevel gamedata = GameObject.Find ("ScriptObject").GetComponent<CreateLevel>();
		
		//Rotation Modulation
		if (direction == 0) { modX = 1 	; modZ = -1	; startX = 	offsetX 	; startZ = offsetZ		;}
		if (direction == 1) { modX = -1 ; modZ = -1	; startX = 	offsetZ		; startZ = -offsetX 	;}
		if (direction == 2) { modX = -1 ; modZ = 1	; startX = -offsetX		; startZ = -offsetZ		;}
		if (direction == 3) { modX = 1 	; modZ = 1	; startX = -offsetZ		; startZ = offsetX		;}
		
		
		
		for (int i=0; i < sizeX; i++){
			
			for (int j=0; j <sizeZ; j++){
				
				
				int x = getX + startX + i * modX;
				int z = getZ + startZ + j * modZ;
				
				
				gamedata.allTiles[x,z].GetComponent<TileBehavior>().isTaken = false;
				gamedata.allTiles[x,z].GetComponent<TileBehavior>().renderer.material.color = Color.gray;


				
			}//for
		}//for


		if(hasDock){
			gamedata.allTiles[getX,getZ].GetComponent<TileBehavior>().removeDock(buildNumber);
		}

		
	}
	


}
