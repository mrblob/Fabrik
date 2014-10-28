using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for Lists
using System.Linq; // for Last() Function

public class BotBehavior : MonoBehaviour {



	CreateLevel gamedata;

	// MOVEMENT

	public float moveSpeed = 10;
	public bool isMoving;
	public bool stopNext;
	public bool clearNext;
	public bool isActive;

	private int direction = -1; // 0 - right, 1 - up, 2 - left, 3 - down
	private int currentWaypointNr;
	private GameObject nextTile;

	public int posX;
	public int posZ;



	// WAYPOINTS

	public int firstWaypointX;
	public int firstWaypointZ;

	public int currentWaypointX;
	public int currentWaypointZ;

	public int nextWaypointX;
	public int nextWaypointZ;

	public int waypointHeight;

	public GameObject Waypoint;
	public GameObject MeshLine;

	public List<GameObject> allWaypoints;
	List<GameObject> allLines;

	private bool isRepeating;
	private bool backToStart;
	

	// CONTAINER

	public GameObject container;
	public int cargoType;


	public Material c1; // Plastics (RAW)
	public Material c2; // Plastics (Processed)
	public Material c3; // Metals (RAW)
	public Material c4; // Metals (Processed)
	public Material c5; // Products





	// Use this for initialization
	void Start () {
	
		gamedata = GameObject.Find ("ScriptObject").GetComponent<CreateLevel>();

		allWaypoints = new List<GameObject>();
		allLines = new List<GameObject>();

		posX = gameObject.GetComponent<BuildableBehavior>().posX;
		posZ = gameObject.GetComponent<BuildableBehavior>().posZ;

		currentWaypointX = posX;
		currentWaypointZ = posZ;

		isMoving = false; // bot is currently moving
		stopNext = false; // bot was ordered to stop ASAP
		clearNext = false; // all waypoints shall be deleted ASAP

		isActive = false; //bot is selected
		isRepeating = false;
		backToStart = false;

		container = transform.Find("Container").gameObject;

		cargoType = 0; // = empty

	}



	// Update is called once per frame
	void Update () {
	
		if(isMoving){

			if(allWaypoints.Count > 0){
				moveBot ();
			}else{
				isMoving = false;
			}

		}

		if(clearNext){

			stopNext = true;

			if(!isMoving){

				clearWaypoints();
				stopNext = false;
				clearNext = false;

			}

		}


	}





	public void selectBot(){


		isActive = true;

		for(int i=0; i < allWaypoints.Count; i++){

			allWaypoints[i].GetComponent<WaypointBehavior>().show();
			allLines[i].renderer.enabled = true;

		}

	}



	public void unselectBot(){


		isActive = false;

		for(int i=0; i < allWaypoints.Count; i++){
			
			allWaypoints[i].GetComponent<WaypointBehavior>().hide();
			allLines[i].renderer.enabled = false;

		}

	}









	//##################################################### WAYPOINTS ############################################



	public bool checkConnection (int getX, int getZ){
	

		if(getX == currentWaypointX || getZ == currentWaypointZ){

			return true;

		}else{

			return false;
		}

	}//checkConnection




	public void addWaypoint (int getX, int getZ){

	

		if(allWaypoints.Count == 0){ // if no waypoints set yet
		
			//add a waypoint + line to the current Position

			allWaypoints.Add ( Instantiate (Waypoint, gamedata.allTiles[posX,posZ].transform.position, Quaternion.identity) as GameObject );
			allLines.Add ( Instantiate (MeshLine, new Vector3(0,0,0), Quaternion.identity) as GameObject ); // add useless Line to make both Lists symmetric

			// set its internal int-coords

			allWaypoints[0].GetComponent<WaypointBehavior>().posX = posX;
			allWaypoints[0].GetComponent<WaypointBehavior>().posZ = posZ;


			//store for readability

			firstWaypointX = posX;
			firstWaypointZ = posZ;


		}



		//store for readability

		nextWaypointX = getX;
		nextWaypointZ = getZ;

		waypointHeight = 0;

		for (int i=0; i<allWaypoints.Count; i++){


			if( allWaypoints[i].GetComponent<WaypointBehavior>().posX == nextWaypointX &&
			    allWaypoints[i].GetComponent<WaypointBehavior>().posZ == nextWaypointZ )

			{

				waypointHeight++;

			}

		}


		//add new Waypoint

		allWaypoints.Add ( Instantiate (Waypoint, gamedata.allTiles[getX,getZ].transform.position, Quaternion.identity) as GameObject );


		// set internal waypoint int-coords

		allWaypoints.Last().GetComponent<WaypointBehavior>().posX = getX;
		allWaypoints.Last().GetComponent<WaypointBehavior>().posZ = getZ;
		allWaypoints.Last().GetComponent<WaypointBehavior>().height = waypointHeight;


		//add line

		allLines.Add ( Instantiate (MeshLine, new Vector3 (0,0,0) , Quaternion.identity) as GameObject );
		allLines.Last().GetComponent<MeshLineBehavior>().drawLine(

			gamedata.allTiles[currentWaypointX,currentWaypointZ].transform.position, 
			gamedata.allTiles[nextWaypointX,nextWaypointZ].transform.position

			);
	

		currentWaypointX = nextWaypointX;
		currentWaypointZ = nextWaypointZ;




	}//addWaypoint





	public void clearWaypoints() {

			if(allWaypoints.Count > 0){

				for(int i=0; i < allLines.Count; i++){

					Destroy(allWaypoints[i]);
					Destroy(allLines[i]);


				}

				allWaypoints.Clear();
				allLines.Clear();

				allWaypoints = new List<GameObject>();
				allLines = new List<GameObject>();

				currentWaypointNr = 0;
				currentWaypointX = posX;
				currentWaypointZ = posZ;
				nextTile = null;
				direction = -1;

			}


	}//clearWaypoints






	//######################################### MOVEMENT ####################################################



	public void moveBot(){

		//if Bot has more then one Waypoint and at least one Waypoint to go
		if(currentWaypointNr < allWaypoints.Count-1){ 

			//if there's no direction set (-1)
			if(direction == -1){

				checkDirection();

			}

			//if there's no target tile set
			if(nextTile == null){

				findNextTile();

			}


			moveTowardsTile();


		}else{

			isMoving = false;

			if(allWaypoints[0].GetComponent<WaypointBehavior>().posX == allWaypoints.Last().GetComponent<WaypointBehavior>().posX &&
			   allWaypoints[0].GetComponent<WaypointBehavior>().posZ == allWaypoints.Last().GetComponent<WaypointBehavior>().posZ)

			{

				currentWaypointNr = 0;
				isMoving = true;

			}


		}




	}


	

	public void checkDirection(){


		WaypointBehavior current = allWaypoints[currentWaypointNr].GetComponent<WaypointBehavior>();
		WaypointBehavior next = allWaypoints[currentWaypointNr + 1].GetComponent<WaypointBehavior>();
	


		if(current.posX == next.posX){

			if(current.posZ < next.posZ){
				direction = 1;
			}

			if(current.posZ > next.posZ){
				direction = 3;
			}
		}

		if(current.posZ == next.posZ){

			if(current.posX < next.posX){
				direction = 0;
			}
				
			if(current.posX > next.posX){
				direction = 2;
			}

		}


	}//check Direction








	public void findNextTile(){


		if(direction == 0){
			nextTile = gamedata.allTiles[ posX+1,posZ ];
		}

		if(direction == 1){
			nextTile = gamedata.allTiles[ posX,posZ+1 ];
		}

		if(direction == 2){
			nextTile = gamedata.allTiles[ posX-1,posZ ];
		}

		if(direction == 3){
			nextTile = gamedata.allTiles[ posX,posZ-1 ];
		}


		if(nextTile.GetComponent<TileBehavior>().isTaken){

			nextTile = null;
			isMoving = false;

		}



	}//findNextTile







	public void moveTowardsTile(){


		if(nextTile != null){

			//Move
			transform.position = Vector3.MoveTowards(
				
				transform.position, 
				nextTile.transform.position,
				moveSpeed * Time.deltaTime * 5
				
				);

			//if Bot reaches center of next tile
			if(transform.position == nextTile.transform.position){

				//update Bot position
				posX = nextTile.GetComponent<TileBehavior>().idX;
				posZ = nextTile.GetComponent<TileBehavior>().idZ;

				nextTile = null;

				// if bot was told to stop ASAP
				if(stopNext){

					isMoving = false;
					stopNext = false;

				}


				//if Bot reaches Waypoint
				if(posX == allWaypoints[currentWaypointNr+1].GetComponent<WaypointBehavior>().posX &&
				   posZ == allWaypoints[currentWaypointNr+1].GetComponent<WaypointBehavior>().posZ){

					currentWaypointNr++;

					direction = -1; //reset direction
					cargoTasks();


				}

			}

		}


	}//moveTowardsTile();


	//Unloading and Loading cargo
	void cargoTasks(){



		WaypointBehavior wp = allWaypoints[currentWaypointNr].GetComponent<WaypointBehavior>();


		if( wp.unload ){ // UNLOAD BOT AT WAYPOINT



			if(cargoType != 0){ // if Bot has any cargo


				// PRODUCER

				if(wp.unloadTarget.tag == "Producer"){

					if(wp.unloadTarget.GetComponent<ProducerBehavior>().loadProducer(cargoType)){

						isMoving = false;

						cargoType = 0;
						
						Invoke ("delay",1);

					}

				}


				// MERGER
				
				if(wp.unloadTarget.tag == "Merger"){
					
					if(wp.unloadTarget.GetComponent<MergerBehavior>().loadMerger(cargoType)){
						
						isMoving = false;
						
						cargoType = 0;
						
						Invoke ("delay",1);
						
					}
					
				}





				// STORAGE

				if(wp.unloadTarget.tag == "Storage" || wp.unloadTarget.tag == "Hub"){

					if(wp.unloadTarget.GetComponent<StorageBehavior>().loadStorage(cargoType)){

						isMoving = false;

						cargoType = 0;

						Invoke ("delay",1);

					}

				}

			}
			

			
		}
		


		if( wp.load ){ // LOAD BOT AT WAYPOINT




			//PRODUCER

			if(wp.loadTarget.tag == "Producer"){

				if(cargoType == 0){ // if bot is empty

					if(wp.loadTarget.GetComponent<ProducerBehavior>().hasProducts){

						isMoving = false;

						cargoType = wp.loadTarget.GetComponent<ProducerBehavior>().unloadProducer();

						Invoke ("delay",1);

					}
				}

			}


			//MERGER
			
			if(wp.loadTarget.tag == "Merger"){
				
				if(cargoType == 0){ // if bot is empty
					
					if(wp.loadTarget.GetComponent<MergerBehavior>().hasProducts){
						
						isMoving = false;
						
						cargoType = wp.loadTarget.GetComponent<MergerBehavior>().unloadMerger();
						
						Invoke ("delay",1);
						
					}
				}
				
			}



			// STORAGE or HUB

			if(wp.loadTarget.tag == "Storage" || wp.loadTarget.tag == "Hub"){
			
				if(cargoType == 0){ // if bot is empty


					if(wp.loadTarget.GetComponent<StorageBehavior>().unloadStorage( 1 )){ // placeholder - insert selected type later

						isMoving = false;

						cargoType = 1;

						Invoke ("delay",1);

					}

				}

			}

			
		}





		if( cargoType != 0){


			if ( cargoType == 1 ){

				container.renderer.material = c1;

			}
		
			if ( cargoType == 2 ){
				
				container.renderer.material = c2;
				
			}

			if ( cargoType == 3 ){
				
				container.renderer.material = c3;
				
			}
			
			if ( cargoType == 4 ){
				
				container.renderer.material = c4;
				
			}

			if ( cargoType == 5 ){
				
				container.renderer.material = c5;
				
			}

			container.renderer.enabled = true; 

			
		}else{
			
			container.renderer.enabled = false;
			
		}



	}

	void delay(){

		isMoving = true;

	}


}
