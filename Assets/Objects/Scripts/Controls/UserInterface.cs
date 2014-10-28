using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for Lists

public class UserInterface : MonoBehaviour {
	
	public GameObject ScriptObject;
	private UserControl userControl;
	private CreateLevel gamedata;
	private Dictionary<int,GameObject> docks;

	public Texture type1;
	public Texture type2;
	public Texture type3;
	public Texture type4;
	public Texture type5;

	private int menuX = Screen.width - 210;

	void Start () {

		userControl = ScriptObject.GetComponent<UserControl>();
		gamedata = ScriptObject.GetComponent<CreateLevel>();
		docks = new Dictionary<int, GameObject>();

	}

	void OnGUI () {


		if(GUI.Button (new Rect(10,300,80,20), "Bot")){
			
			userControl.setType(0);
		}
		
		if(GUI.Button (new Rect(10,330,80,20), "SMALL")){
			
			userControl.setType(1);
			
		}

		if(GUI.Button (new Rect(10,360,80,20), "MID")){
			
			userControl.setType(2);
			
		}


		if(GUI.Button (new Rect(10,390,80,20), "LARGE")){
			
			userControl.setType(3);
			
		}

		if(GUI.Button (new Rect(10,420,80,20), "HUB")){
			
			userControl.setType(4);
			
		}


		if(userControl.selectedBuildable != null){

			GameObject build = userControl.selectedBuildable.transform.parent.gameObject;
			BuildableBehavior b = build.GetComponent<BuildableBehavior>();


			if(build.tag == "Hub"){

				GUI.Box(new Rect(menuX,10,180,200), build.name);


				GUI.Label(new Rect(menuX+10,40,120,20), "BUY");

				if(GUI.Button (new Rect(menuX+10, 70, 20,20), type1)){ build.GetComponent<StorageBehavior>().loadStorage(1); }
				if(GUI.Button (new Rect(menuX+30, 70, 20,20), type2)){ build.GetComponent<StorageBehavior>().loadStorage(2); }
				if(GUI.Button (new Rect(menuX+50, 70, 20,20), type3)){ build.GetComponent<StorageBehavior>().loadStorage(3); }
				if(GUI.Button (new Rect(menuX+70, 70, 20,20), type4)){ build.GetComponent<StorageBehavior>().loadStorage(4); }


				GUI.Label(new Rect(menuX+10,100,120,20), "SELL");

				if(GUI.Button (new Rect(menuX+10, 130, 20,20), type1)){ build.GetComponent<StorageBehavior>().unloadStorage(1); }
				if(GUI.Button (new Rect(menuX+30, 130, 20,20), type2)){ build.GetComponent<StorageBehavior>().unloadStorage(2); }
				if(GUI.Button (new Rect(menuX+50, 130, 20,20), type3)){ build.GetComponent<StorageBehavior>().unloadStorage(3); }
				if(GUI.Button (new Rect(menuX+70, 130, 20,20), type4)){ build.GetComponent<StorageBehavior>().unloadStorage(4); }
				if(GUI.Button (new Rect(menuX+90, 130, 20,20), type5)){ build.GetComponent<StorageBehavior>().unloadStorage(5); }



			}


			if(build.tag != "Bot"){

				GUI.Box(new Rect(10,10,110,80), build.name );
				if(GUI.Button (new Rect(20,40,90,20), "Remove")){

					gamedata.removeBuildable( userControl.selectedBuildable.transform.parent.gameObject );

				}

			}



			if(build.tag == "Bot"){
				
				GUI.Box(new Rect(10,10,110,270), build.name );
				if(GUI.Button (new Rect(20,40,90,20), "Remove")){
					
					gamedata.removeBuildable( userControl.selectedBuildable.transform.parent.gameObject );
					userControl.setUserMode(1);

				}

				if(GUI.Button (new Rect(20,70,90,20), "Add WP")){
					
					userControl.setUserMode(3);
					
				}

				if(GUI.Button (new Rect(20,100,90,20), "Clear WP")){
					
					build.GetComponent<BotBehavior>().clearNext = true;
					
				}

				if(GUI.Button (new Rect(20,130,90,20), "Move")){
					
					build.GetComponent<BotBehavior>().isMoving = true;
					
				}

				if(GUI.Button (new Rect(20,160,90,20), "Stop")){
					
					build.GetComponent<BotBehavior>().stopNext = true;
					
				}
						
			}//if bot selected



		}//if buildable selected


		if(userControl.selectedWaypoint != null){


			WaypointBehavior wp = userControl.selectedWaypoint.GetComponent<WaypointBehavior>();
			docks = wp.availableDocks;
		
			GUI.Box(new Rect(menuX,10,190,200), userControl.selectedWaypoint.name);

			int i = 0;

			foreach(KeyValuePair<int, GameObject> dock in docks)

			{

				//Label
				GUI.color = Color.white;
				GUI.Label(new Rect(menuX+10,40+40*i,120,20), dock.Value.name);


				// color if active
				if(wp.unloadTarget == dock.Value && wp.unload){ GUI.color = Color.red; }else{ GUI.color = Color.white;}


				// UNLOAD Button
				if(GUI.Button (new Rect(menuX+10, 40*i+60, 80,20), "UNLOAD")){
				
					wp.toggleUnload( dock.Key );

				}




				// Recolor if active
				if(wp.loadTarget == dock.Value && wp.load){ GUI.color = Color.red; }else{ GUI.color = Color.white; }

				// LOAD Button
				if(GUI.Button (new Rect(menuX+90, 40*i+60, 80,20), "LOAD")){

					wp.toggleLoad( dock.Key );

				}





				i++;

			}



		}// if Waypoint selected


	}
}

