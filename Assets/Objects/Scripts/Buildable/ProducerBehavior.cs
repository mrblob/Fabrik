using UnityEngine;
using System.Collections;

public class ProducerBehavior : MonoBehaviour {
	
	private GameObject container_in;
	private GameObject container_out;
	
	public int[] ressources;
	public int[] products;
	public bool hasProducts;
	public bool workDone;
	public int requiredRessourceType;
	
	// Use this for initialization
	void Start () {
		
		ressources = new int[1];
		products = new int[1];
		
		ressources[0] = 0;
		products[0] = 0;
		hasProducts = false;
		workDone = false;
		requiredRessourceType = 1;
		
		container_in = transform.Find ("Container_in").gameObject;
		container_out = transform.Find ("Container_out").gameObject;
		
		container_in.renderer.enabled = false;
		container_out.renderer.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	public bool loadProducer(int type){
		
		//if ressource Typ fits Producer and a free slot is available
		if(type == requiredRessourceType && ressources[0] == 0){
			
			ressources[0] = type; //fill ressource slot
			startWork();
			
			container_in.renderer.enabled = true;
			
			return true;
			
		}
		
		
		return false;
		
		
	}
	
	
	public int unloadProducer(){
		
		int returningProductType = products[0]; // temp save cargo type in products slot
		
		
		products[0] = 0; // set products slot empty
		container_out.renderer.enabled = false;
		
		hasProducts = false;
		
		return returningProductType;
		
		
	}
	
	public void startWork(){
		
		Invoke ("delay",5);
		
		
		
	}
	
	
	public void delay(){
		
		
		ressources[0] = 0;
		container_in.renderer.enabled = false;
		
		products[0] = 2;
		container_out.renderer.enabled = true;
		
		hasProducts = true;
		
		
	}
	
	
	
}
