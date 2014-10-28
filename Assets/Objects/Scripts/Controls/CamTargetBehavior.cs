using UnityEngine;
using System.Collections;

public class CamTargetBehavior : MonoBehaviour {

	public Camera cam;

	private float moveSpeed;
	private Vector3 offset;


	// Use this for initialization
	void Start () {

		moveSpeed = 100.0f;

	
	}
	
	// Update is called once per frame
	void Update () {

		offset = new Vector3 (

			moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,
			0,
			moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime
			);


		offset = cam.transform.TransformDirection(offset); //sets controls relative to cam
		offset.y = 0;
		transform.position = transform.position + offset;
	
	}
}
