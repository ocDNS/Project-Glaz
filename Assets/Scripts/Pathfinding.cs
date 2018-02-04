using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour {

	NavMeshPath navPath;

	void Start () {
		navPath = new NavMeshPath();
	}

	 List<Vector3> RasterizeLine (Vector3 start, Vector3 end) {
		List<Vector3> corners = new List<Vector3>();
		corners.Add(start);
		while(start != end){
			if(start.z != end.z){
				start.z += Mathf.Sign(end.z - start.z);
				corners.Add(start);
			}
			if(start.x != end.x){
				start.x += Mathf.Sign(end.x - start.x);
				corners.Add(start);
			}
		}
		return corners; //convert to array if necessary
	}
	
	public Vector3[] CalculatePath (Transform target) {
		bool _aux = NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, navPath);

		if(!_aux){
			Debug.LogError("Can't calculate path for " + transform.name);
			return null;
		}

		List<Vector3> pathCoordinates = new List<Vector3>();
		for(int i = 0; i < navPath.corners.Length - 1; i++){
			Vector3 start, end;

			start = navPath.corners[i];
			start.x = Mathf.RoundToInt(navPath.corners[i].x);
			start.z = Mathf.RoundToInt(navPath.corners[i].z);

			end = navPath.corners[i + 1];
			end.x = Mathf.RoundToInt(navPath.corners[i + 1].x);
			end.z = Mathf.RoundToInt(navPath.corners[i + 1].z);

			pathCoordinates.AddRange(RasterizeLine(start, end));
		}

		//remove duplicate coordinates from the list
		for (int i = 1; i < pathCoordinates.Count - 1; i++){
			if(pathCoordinates[i] == pathCoordinates[i-1]){
				pathCoordinates.Remove(pathCoordinates[i]);
				i--;
			}
		}

		//turn coordinates into directions
		Vector3[] pathDirections = new Vector3[pathCoordinates.Count-1]; //each coordinate is a node, direction is line, thus lines = nodes - 1

		for(int i = 0; i < pathDirections.Length; i++){
			pathDirections[i] = pathCoordinates[i+1] - pathCoordinates[i];
		}

		//draw debug line
		for (int i = 0; i < pathCoordinates.Count - 1; i++){
        	Debug.DrawLine(pathCoordinates[i], pathCoordinates[i + 1], Color.green, 900);
		}

		return pathDirections;
	}
}
