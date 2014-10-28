using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for Lists
using System.Linq; // Last()

public class UserControl : MonoBehaviour {


	public CreateLevel gamedata;

	RaycastHit hit;
	private float raycastLength = 500;

	public GameObject Cursor;

	public GameObject Bot;
	public GameObject Waypoint;
	public GameObject MeshLine;

	public GameObject Building_s;
	public GameObject Building_m;
	public GameObject Building_l;

	public GameObject Hub;
	
	public GameObject[] Buildables; 
	public int currentType;


	public bool CursorOnGround;
	public bool BuildingPossible;
	public int userMode;


	public GameObject selectedTile;
	public TileBehavior tile;

	public GameObject selectedBuildable;
	//public BuildableBehavior build;
	
	public GameObject selectedBot;
	private BotBehavior bot;

	public GameObject selectedWaypoint;
	private WaypointBehavior wp;

	
	private int layerMaskTiles; 
	private int layerMaskWaypoints;
	private int layerMaskBuildables;





	// Use this for initialization
	void Start () {


		gamedata = gameObject.GetComponent<CreateLevel>();
		
		Cursor = Instantiate (Cursor, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Cursor.SetActive(false);
		CursorOnGround = false;
		BuildingPossible = false;

		Buildables = new GameObject[5];

		Buildables[0] = Instantiate(Bot, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Buildables[0].SetActive(false);

		Buildables[1] = Instantiate(Building_s, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Buildables[1].SetActive(false);

		Buildables[2] = Instantiate(Building_m, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Buildables[2].SetActive(false);

		Buildables[3] = Instantiate(Building_l, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Buildables[3].SetActive(false);

		Buildables[4] = Instantiate(Hub, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Buildables[4].SetActive(false);

		Waypoint = Instantiate (Waypoint, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Waypoint.SetActive(false);

		MeshLine = Instantiate (MeshLine, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		MeshLine.SetActive(false);

		layerMaskTiles = 1 << 8; // Bit Shift -> Layer 8 (Tiles)
		layerMaskWaypoints = 1 << 9; // Bit Shift -> Layer 9 (Waypoints)
		layerMaskBuildables = 1 << 10; // Bit Shift -> Layer 10 (Buildables)

		setUserMode(1);

	}



	// Update is called once per frame
	void Update () {



		//cast ray from camera to mouse
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 

		// draw ray in editor
		Debug.DrawRay(ray.origin, ray.direction*raycastLength, Color.yellow);

		// Deselect last tile
		if(selectedTile != null){ selectedTile.GetComponent<TileBehavior>().isActive = false; }




		//################################# userMode 1 - Selecting ######################################


		//select a buildable GameObject on the map
		
		if(userMode == 1){


			if(Physics.Raycast(ray, out hit, raycastLength, layerMaskBuildables)){ 
				
				if(Input.GetMouseButtonDown(0)){



					if(selectedBuildable != null){ //reset color
						
						selectedBuildable.renderer.material.color = Color.yellow;
						selectedBuildable = null;
						
					}

					if(selectedBot != null){
						
						selectedBot.GetComponent<BotBehavior>().unselectBot();
						selectedBot = null;
						
					}

					if(selectedWaypoint != null){
						
						selectedWaypoint.GetComponent<WaypointBehavior>().unselectWaypoint();
						selectedWaypoint = null;
						
					}



					selectedBuildable = hit.collider.gameObject;
					selectedBuildable.renderer.material.color = Color.white;





					if(selectedBuildable.transform.parent.gameObject.tag == "Bot"){

						selectedBuildable.renderer.material.color = Color.green;
						selectedBot = selectedBuildable.transform.parent.gameObject;
						selectedBot.GetComponent<BotBehavior>().selectBot();

					}



				}
				
			}//raycast



			if(Physics.Raycast(ray, out hit, raycastLength, layerMaskWaypoints)){
				
				
				if(Input.GetMouseButtonDown(0)){
					
					if(selectedWaypoint != null){
						
						selectedWaypoint.GetComponent<WaypointBehavior>().unselectWaypoint();
						
					}
					
					selectedWaypoint = hit.collider.gameObject.transform.parent.gameObject;
					selectedWaypoint.GetComponent<WaypointBehavior>().selectWaypoint();
					
				}
				
				
			}



			
			
		}//user Mode 1




		//################################# userMode 2 - Building ######################################


		// Build Buildable
		if(userMode == 2){ 


			if(Physics.Raycast(ray, out hit, raycastLength, layerMaskTiles)){ 


				CursorOnGround = true;

				selectedTile = hit.collider.gameObject;
				tile = selectedTile.GetComponent<TileBehavior>();
				tile.isActive = true; //set tile to active


				Cursor.SetActive(true);
				Cursor.transform.position = selectedTile.transform.position; // reposition cursor



				Buildables[currentType].SetActive(true);
				Buildables[currentType].transform.position = selectedTile.transform.position;




				//Check if building is possible in this place

				if( Buildables[currentType].GetComponent<BuildableBehavior>().checkSpace (tile.idX , tile.idZ) ){

					Buildables[currentType].GetComponent<BuildableBehavior>().setPossible();
					BuildingPossible = true;
					
				}else{

					Buildables[currentType].GetComponent<BuildableBehavior>().setImpossible();
					BuildingPossible = false;

				}


				//Rotate with Q and E


				if(Input.GetKeyDown(KeyCode.E)){

					Buildables[currentType].GetComponent<BuildableBehavior>().rotateRight();

				}

				if(Input.GetKeyDown (KeyCode.Q)){

					Buildables[currentType].GetComponent<BuildableBehavior>().rotateLeft();

				}

				if(Input.GetMouseButtonDown(0)){ // left click to build

					if(BuildingPossible){

						gamedata.addBuildable ( 

						                       tile.idX, 
						                       tile.idZ, 
						                       currentType,
						                       Buildables[currentType].transform.rotation,
						                       Buildables[currentType].GetComponent<BuildableBehavior>().direction

						                       );

					}

				}



			}else{

				CursorOnGround = false;
				Cursor.SetActive(false);
				Buildables[currentType].SetActive(false);

			}//raycast


		}//user Mode 2


		//################################# userMode 3 - Waypoints ######################################


		if(userMode == 3){


			if(Physics.Raycast(ray, out hit, raycastLength, layerMaskTiles)){ 
				
				
				CursorOnGround = true;
				
				selectedTile = hit.collider.gameObject;
				tile = selectedTile.GetComponent<TileBehavior>();
				tile.isActive = true; //set tile to active

				selectedBot = selectedBuildable.transform.parent.gameObject;
				bot = selectedBot.GetComponent<BotBehavior>();

				Cursor.SetActive(true);
				Cursor.transform.position = selectedTile.transform.position; // reposition cursor


				if( bot.checkConnection( tile.idX , tile.idZ ) ) {

					Waypoint.SetActive (true);
					Waypoint.transform.position = selectedTile.transform.position;

					MeshLine.SetActive (true);
					MeshLine.GetComponent<MeshLineBehavior>().drawLine(
							gamedata.allTiles[bot.currentWaypointX,bot.currentWaypointZ].transform.position,   	// last Waypoint position
							tile.transform.position 															//  new Waypoint position
							);

					if(Input.GetMouseButtonDown(0)){

						bot.addWaypoint( tile.idX , tile.idZ );

					}



				}else{

					Waypoint.SetActive (false);
					MeshLine.SetActive (false);
				}


			}//ray



		}//user Mode 3











		// Unselect all with right-mouse

		if(Input.GetMouseButtonDown(1)){ 

			if(userMode == 2 || userMode == 1){ // if User is in Building mode or selection mode

				setUserMode(1);

				if(selectedBuildable != null){ 

					//reset color

					selectedBuildable.renderer.material.color = Color.yellow;
					selectedBuildable = null;


					if(selectedBot !=null){

						selectedBot.GetComponent<BotBehavior>().unselectBot();
						selectedBot = null;

					}

					if(selectedWaypoint !=null){

						selectedWaypoint.GetComponent<WaypointBehavior>().unselectWaypoint();
						selectedWaypoint = null;

					}
				}

			}

			if(userMode == 3){ // if User is in Waypoint-Building Mode

				setUserMode(1);

			}
			
		}


	}//update







	public void setUserMode (int nr){

		userMode = nr;

		if(userMode == 1){ // Nothing selected

			Cursor.SetActive(false);
			Waypoint.SetActive(false);
			MeshLine.SetActive(false);
			Buildables[currentType].SetActive(false);

		}

		if(userMode == 2){ // Building 

			Cursor.renderer.material.SetColor ("_TintColor",Color.blue);
			Buildables[currentType].renderer.material.SetColor ("_TintColor",Color.blue);
			Waypoint.SetActive(false);
			MeshLine.SetActive(false);
		}

		if(userMode == 3){ // Waypoints

			Cursor.renderer.material.SetColor ("_TintColor",Color.green);

		}

	}


	
	// change type of Building you want to place

	public void setType (int nr){ 

		Buildables[currentType].SetActive(false);
		currentType = nr;
		Buildables[currentType].SetActive(true);
		setUserMode (2);

	}



}
