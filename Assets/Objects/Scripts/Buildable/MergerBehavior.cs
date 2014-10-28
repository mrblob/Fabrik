using UnityEngine;
using System.Collections;

public class MergerBehavior : MonoBehaviour {
	
	private GameObject container_in_1;
	private GameObject container_in_2;
	private GameObject container_out;
	
	public int[] ressources;
	public int[] products;
	public bool hasProducts;
	public bool workDone;

	public int requiredRessourceType_1;
	public int requiredRessourceType_2;
	
	// Use this for initialization
	void Start () {
		
		ressources = new int[2];
		products = new int[1];
		
		ressources[0] = 0;
		ressources[1] = 0;
		products[0] = 0;
		hasProducts = false;
		workDone = false;

		requiredRessourceType_1 = 2;
		requiredRessourceType_2 = 4;
		
		container_in_1 = transform.Find ("Container_in_1").gameObject;
		container_in_2 = transform.Find ("Container_in_2").gameObject;
		container_out = transform.Find ("Container_out").gameObject;
		
		container_in_1.renderer.enabled = false;
		container_in_2.renderer.enabled = false;
		container_out.renderer.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	public bool loadMerger(int type){
		
		//if ressource Typ fits Producer and a free slot is available
		if(type == requiredRessourceType_1 && ressources[0] == 0){
			
			ressources[0] = type; //fill ressource slot
			startWork();
			
			container_in_1.renderer.enabled = true;
			
			return true;
			
		}

		if(type == requiredRessourceType_2 && ressources[1] == 0){
			
			ressources[1] = type; //fill ressource slot
			startWork();
			
			container_in_2.renderer.enabled = true;
			
			return true;
			
		}
		
		
		return false;
		
		
	}
	
	
	public int unloadMerger(){
		
		int returningProductType = products[0]; // temp save cargo type in products slot
		
		
		products[0] = 0; // set products slot empty
		container_out.renderer.enabled = false;
		
		hasProducts = false;
		
		return returningProductType;
		
		
	}
	
	public void startWork(){

		if(ressources[0] != 0 && ressources[1] != 0){
			Invoke ("delay",5);
		}
		
		
	}
	
	
	public void delay(){
		
		
		ressources[0] = 0;
		ressources[1] = 0;

		container_in_1.renderer.enabled = false;
		container_in_2.renderer.enabled = false;
		
		products[0] = 2;
		container_out.renderer.enabled = true;
		
		hasProducts = true;
		
		
	}
	
	
	
}
