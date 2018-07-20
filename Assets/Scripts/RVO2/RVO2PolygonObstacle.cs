using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVO2PolygonObstacle : MonoBehaviour {

	// Please specify vertex location in counterclockwise order~
	public Vector2[] vertices = new Vector2[4];

	// the obstacle initialization MUST be done in the Awake() function
	// since obstacles need to be processed in the managers' Start() function
	// in order to account it in simulation
	void Awake () {
		IList<RVO.Vector2> data = new List<RVO.Vector2>();
		for (int i = 0; i < vertices.Length; ++i) {
			data.Add(new RVO.Vector2(transform.position.x + vertices[i].x, transform.position.z + vertices[i].y));
		}
		RVO.Simulator.Instance.addObstacle(data);
	}

	void OnDrawGizmos() {
		Color originalColor = Gizmos.color;

        // Draw lines surrounding the box;
		Gizmos.color = Color.magenta;
		Vector3[] pos = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; ++i) {
			pos[i] = new Vector3(transform.position.x + vertices[i].x, 
				transform.position.y, transform.position.z + vertices[i].y);
		}
		for (int i = 0; i < vertices.Length; ++i) {
			Vector3 begin = pos[i];
			Vector3 end = pos[(i + 1) % vertices.Length];
			Gizmos.DrawLine(begin, end);
			// Draw lines pointing outward marking the obstacle blocking area
			Vector3 dir = Vector3.Cross(end - begin, Vector3.up).normalized;
			for (int j = 0; j < 20; ++j) {
				Vector3 mark = begin + (end - begin) * j / 20;		
				Gizmos.DrawLine(mark, mark + dir * 0.2f);			
			}
		}
	
        Gizmos.color = originalColor;
	}
}
