using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for Lists
using System.Linq; // for Last() Function

public class TileBehavior : MonoBehaviour {


	public bool isActive;
	public bool isTaken;
	public bool isDock;

	public Dictionary<int,GameObject> myDocks;

	public int idX;
	public int idY;
	public int idZ;





	// Use this for initialization
	void Start () {

		isActive = false;
		isTaken = false;
		isDock = false;
		renderer.material.color = Color.gray;

		myDocks = new Dictionary<int, GameObject>();


	}
	
	// Update is called once per frame
	void Update () {


	}

	public void addDock (int id, GameObject dock){

		myDocks.Add(id,dock);
		isDock = true;


	}


	public void removeDock (int id){

		myDocks.Remove(id);

		if(myDocks.Count == 0){
			isDock = false;

		}



	}

}
