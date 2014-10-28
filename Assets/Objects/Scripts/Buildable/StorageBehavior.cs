using UnityEngine;
using System.Collections;

public class StorageBehavior : MonoBehaviour {

	public int[] storage;

	private GameObject[] myContainers;

	public Material c1;
	public Material c2;
	public Material c3;
	public Material c4;
	public Material c5;


	public bool requestedCargoFound;

	public bool requestedSpaceFound;

	// Use this for initialization
	void Start () {
	
		storage = new int[4];

		myContainers = new GameObject[4];

		myContainers[0] = transform.Find ("Container_1").gameObject;
		myContainers[1] = transform.Find ("Container_2").gameObject;
		myContainers[2] = transform.Find ("Container_3").gameObject;
		myContainers[3] = transform.Find ("Container_4").gameObject;


		for (int i=0; i < storage.Length; i++){


			storage[i] = 0; // 1 = ressource Nr1
			myContainers[i].renderer.enabled = false;


		}



		requestedCargoFound = false;
		requestedSpaceFound = false;


	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public bool loadStorage(int type){

		for(int i=0; i < storage.Length; i++){
			
			if(storage[i] == 0){


				storage[i] = type;

				if(type == 1){ myContainers[i].renderer.material = c1; }
				if(type == 2){ myContainers[i].renderer.material = c2; }
				if(type == 3){ myContainers[i].renderer.material = c3; }
				if(type == 4){ myContainers[i].renderer.material = c4; }
				if(type == 5){ myContainers[i].renderer.material = c5; }

				myContainers[i].renderer.enabled = true;


				return true;

			}
			
		}

		return false;

	}


	public bool unloadStorage(int type){


		for(int i=0; i < storage.Length; i++){


			if(storage[i] == type){

				storage[i] = 0;
				myContainers[i].renderer.enabled = false;

				return true;


			}

		}


		return false;

	}



}
