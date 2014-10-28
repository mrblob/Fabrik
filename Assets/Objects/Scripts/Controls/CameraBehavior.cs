using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

	public GameObject target;

	private Vector3 offset;

	//The default distance of the camera from the target.
	public float distance = 200;
	
	//Control the speed of zooming and dezooming.
	public float zoomStep = 50;
	
	//The speed of the camera. Control how fast the camera will rotate.
	public float xSpeed = 100;
	public float ySpeed = 100;
	
	//The position of the cursor on the screen. Used to rotate the camera.
	private float x = 0;
	private float y = 0;
	
	//Distance vector. 
	private Vector3 distanceVector;



	void Start () {
	
		offset = target.transform.position - transform.position;

		distanceVector = new Vector3(0.0f,0.0f,-distance);
		
		Vector2 angles = this.transform.localEulerAngles;
		x = angles.x;
		y = angles.y;
		
		this.Rotate(x, y);

	}
	

	void Update () {

		this.RotateControls();
		this.Zoom();
		this.Rotate(x,y);

	}


	void RotateControls()
	{
		if ( Input.GetMouseButton(2) )
		{
			x += Input.GetAxis("Mouse X") * xSpeed;
			y += -Input.GetAxis("Mouse Y")* ySpeed;
			
			this.Rotate(x,y);
		}
		
	}
	

	//Transform the cursor mouvement in rotation and in a new position for the camera.

	void Rotate( float x, float y )
	{
		//Transform angle in degree in quaternion form used by Unity for rotation.
		Quaternion rotation = Quaternion.Euler(y,x,0.0f);
		
		//The new position is the target position + the distance vector of the camera rotated at the specified angle.
		Vector3 position = rotation * distanceVector + target.transform.position;
		
		//Update the rotation and position of the camera.
		transform.rotation = rotation;
		transform.position = position;
	}

	void Zoom()
	{
		if ( Input.GetAxis("Mouse ScrollWheel") < 0 )
		{
			this.ZoomOut();
		}
		else if ( Input.GetAxis("Mouse ScrollWheel") > 0 )
		{
			this.ZoomIn();
		}
		
	}
	

	void ZoomIn()
	{
		distance = distance - zoomStep;
		distanceVector = new Vector3(0,0,-distance*15);
		this.Rotate(x,y);
	}
	

	void ZoomOut()
	{
		distance = distance + zoomStep;
		distanceVector = new Vector3(0,0,-distance*15);
		this.Rotate(x,y);
	}

}
