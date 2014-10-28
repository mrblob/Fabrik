using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for Lists

public class WaypointBehavior : MonoBehaviour {

	public CreateLevel gamedata;
	public Dictionary<int, GameObject> availableDocks;

	public int posX;
	public int posZ;

	public int height = 0;

	public bool isActive;
	public bool isLast;

	public bool unload;
	public GameObject unloadTarget;

	public bool load;
	public GameObject loadTarget;
	

	
	// Use this for initialization
	void Start () {

		isLast = false;
		isActive = false;

		unload = false;
		load = false;

		gamedata = GameObject.Find ("ScriptObject").GetComponent<CreateLevel>();
		availableDocks = new Dictionary<int, GameObject>();

		transform.position += new Vector3(0,height*5,0);

	}
	
	// Update is called once per frame
	void Update () {




	}

	public void toggleUnload (int dictKey){

		if(unloadTarget == availableDocks[dictKey]){

			unload = false;
			unloadTarget = null;

		}else{

		unload = true;
		unloadTarget = availableDocks[dictKey];

		}


	}


	public void toggleLoad (int dictKey){

		if(loadTarget == availableDocks[dictKey]){

			load = false;
			loadTarget = null;

		}else{

		load = true;
		loadTarget = availableDocks[dictKey];

		}

	}


	public void hide(){

		Component[] allChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer child in allChildren) {
			
			child.renderer.enabled = false;
			
		}

	}

	public void show(){

		Component[] allChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer child in allChildren) {
			
			child.renderer.enabled = true;
			
		}

	}


	public void selectWaypoint(){

		isActive = true;

		availableDocks = gamedata.allTiles[posX,posZ].GetComponent<TileBehavior>().myDocks;

		Component[] allChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer child in allChildren) {

		
			child.renderer.material.color = Color.white;
	
			
		}

	}

	public void unselectWaypoint(){

		isActive = false;

		Component[] allChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer child in allChildren) {
			
			child.renderer.material.color = Color.green;
			
		}

	}


}
