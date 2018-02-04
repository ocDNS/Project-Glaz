using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class PathfindingDeprecated {

    private static HashSet<Vector3> rasterizedLine(Vector3 source, Vector3 target, float distance){
		HashSet<Vector3> temp = new HashSet<Vector3> (); 
		Vector3 moveTo;
		int source_x = (int)source.x; int source_z = (int)source.z;
		int target_x = (int)target.x; int target_z = (int)target.z;
		int distance_x = Mathf.Abs (target_x - source_x);
		int distance_z = Mathf.Abs (target_z - source_z);
		int sx = (source_x < target_x) ? 1 : -1;
		int sz = (source_z < target_z) ? 1 : -1;
		int err = (distance_x > distance_z ? distance_x : -distance_z) / 2;
		int e2;
		while(true) {
			moveTo = new Vector3 (source_x, target.y , source_z);
			temp.Add (moveTo);
			if (Vector3.Distance(moveTo, target) <= distance)
				break;
			e2 = err;
			if (e2 > -distance_x) {
				err -= distance_z;
				source_x += sx;
			}
			else if (err < distance_z) {
				err += distance_x;
				source_z += sz;
			}
		}
		return temp;
	}


    private static List<Vector3> GetWorldPath(Vector3 source, Vector3 target, float distance){

		NavMeshPath path = new NavMeshPath ();
		HashSet<Vector3> finalPath = new HashSet<Vector3>();
		Vector3 moveTo = source;

		NavMesh.CalculatePath (source, target, NavMesh.AllAreas, path);

		for (int index = 1; index < path.corners.Length; index++) {
			Vector3 endHere = new Vector3(Mathf.Round(path.corners[index].x), target.y, Mathf.Round(path.corners[index].z));
			finalPath.UnionWith (rasterizedLine(moveTo, endHere, distance));
			finalPath.Add (endHere);
			moveTo = endHere;
		}
		return new List<Vector3> (finalPath);
	}

		
	private static List<Vector3> GetDirections(List<Vector3> positions){
		List<Vector3> temp = new List<Vector3> ();
		for (int index = 0; index < positions.Count - 1; index++)
			temp.Add (positions[index + 1] - positions[index]);
		return temp;
	}


	private static Directions CalculateWorldCoord (Vector3 pos){
		if (pos == Vector3.forward)
			return Directions.Forward;
		else if (pos == Vector3.back)
			return Directions.Backward;
		else if (pos == Vector3.left)
			return Directions.Left;
		return Directions.Right;
	}

	public static Directions[] GetPath(Vector3 source, Vector3 target, float distance = 1.0f){
		List<Vector3> path = new List<Vector3> (GetDirections(GetWorldPath(source, target, distance)));
		Directions[] directions = new Directions[path.Count];
		for(int index = 0; index < path.Count; index++)
			directions[index] = CalculateWorldCoord(path[index]);
		return directions;
	}

	public static Vector3[] GetWorldCoordPath(Vector3 source, Vector3 target, float distance = 1.0f){
		return GetWorldPath (source, target, distance).ToArray ();
 	}




	/*
	void pathfinding(){
		NavMesh.CalculatePath (start.transform.position, end.transform.position, NavMesh.AllAreas, path);

		Vector3 moveTo = start.transform.position;

		for (int index = 1; index < path.corners.Length; index++) {
			float counter = 0.01f;
			float time = 0;
			float maxTime = 3;
			
			Vector3 endHere = new Vector3(Mathf.Round(path.corners[index].x), 0, Mathf.Round(path.corners[index].z));
			Instantiate (node, endHere, Quaternion.identity);

			float angle = Vector3.Angle (Vector3.back, moveTo - endHere);

			Debug.Log (angle);

			while (Vector3.Distance (moveTo, endHere) > 1 && maxTime > (time += counter)) {
				angle = Vector3.Angle (Vector3.back, moveTo - endHere);

				//Debug.Log (angle);

				if (angle < 90) {
					if (angle < 45)
						angle = 0;
					else
						angle = 90;
				} else {
					if (angle > 135)
						angle = 180;
					else
						angle = 90;
				}
				float sine = Mathf.Round (Mathf.Sin (angle));
				float cosine = Mathf.Round (Mathf.Cos (angle));

				int sign = (start.transform.position.x < endHere.x) ? 1 : -1;

				if (angle > 135 && cosine != 0)
					sine = 0;
				//int sign = (start.transform.position.y > end.transform.position.y) ? -1 : 1;
				moveTo += new Vector3 (sign * sine, 0, cosine);

				Debug.Log (sine + " " + cosine);

				end.GetComponent<MeshRenderer> ().material.color = Random.ColorHSV ();

				moveTo = new Vector3(Mathf.Round(moveTo.x), 0, Mathf.Round(moveTo.z));

				Instantiate (end, moveTo, Quaternion.identity);

			}

			moveTo = path.corners[index];
		}
	}
	*/
}
