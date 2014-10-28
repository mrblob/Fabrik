using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for Lists
using System.Linq; // for Last() Function

public class MeshLineBehavior : MonoBehaviour {

	private float width = 4;
	private float height1 = 2;
	private float height2 = 2;
	private Vector3 point1;
	private Vector3 point2;


	private float length;


	// Use this for initialization
	void Start () {




	}
	
	// Update is called once per frame
	void Update () {


	
	}


	public void drawLine(Vector3 p1, Vector3 p2) {

		point1 = p1;
		point2 = p2;


		Vector3[] vertices = new Vector3[4];
		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mf.mesh = mesh;



		//Vertices
		if(point1.x == point2.x){ //Vertical
		
			vertices[0] = point1 + new Vector3(	-width/2,		height1,		0	);
			vertices[1] = point1 + new Vector3(	width/2,		height1,		0	); 
			vertices[2] = point2 + new Vector3(	-width/2,		height2,		0	);
			vertices[3] = point2 + new Vector3(	width/2,		height2,		0	);

			length = Mathf.Abs(point1.z - point2.z) / 10;
					
				
		}

		if(point1.z == point2.z){ //Horizontal

					
			vertices[0] = point1 + new Vector3(	0,		height1,		-width/2	);
			vertices[1] = point1 + new Vector3(	0,		height1,		width/2		); 
			vertices[2] = point2 + new Vector3(	0,		height2,		-width/2	); 
			vertices[3] = point2 + new Vector3(	0,		height2,		width/2		);

			length = Mathf.Abs(point1.x - point2.x) / 10;
					
				

		}
			
		//Triangles
		int[] triangles = new int[6];
			
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 1;
			
		triangles[3] = 2;
		triangles[4] = 3;
		triangles[5] = 1;
			
			
		//Normals
		Vector3[] normals = new Vector3[4];
			
		normals[0] = Vector3.forward;
		normals[1] = Vector3.forward;
		normals[2] = Vector3.forward;
		normals[3] = Vector3.forward;
			

		//UVs
		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(0,0);
		uv[1] = new Vector2(1,0);
		uv[2] = new Vector2(0,length);
		uv[3] = new Vector2(1,length);
			

		//Assign Arrays
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		/*
		if(height2 >= 5){ renderer.material.SetColor("_TintColor", Color.green); }
		if(height2 >= 10){ renderer.material.SetColor("_TintColor", Color.red); }
		if(height2 >= 15){ renderer.material.SetColor("_TintColor", Color.yellow);  }
		*/

		}//draw




}
