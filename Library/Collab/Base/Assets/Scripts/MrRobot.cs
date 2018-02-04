using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrRobot : MonoBehaviour {

	public Transform start, end;

	public LineRenderer line;

	private Vector3[] patrolCoordinates;
	private Vector3 _pos;


    Vector3 GetDirectionsCoordinatesWorld (Directions dir){
		if(dir.Equals(Directions.Forward)){
			return Vector3.forward;
		} else if(dir.Equals(Directions.Right)){
			return Vector3.right;
		} else if(dir.Equals(Directions.Left)){
			return -Vector3.right;
		} else return -Vector3.forward;
	}

	void Start () {

		Directions[] dir = Pathfinding.GetPath (start.position, end.position, 1.0f);

		patrolCoordinates = new Vector3[dir.Length+1];

		_pos = new Vector3(transform.position.x, line.startWidth, transform.position.z);

		patrolCoordinates[0] = _pos;
		for(int i=1; i<patrolCoordinates.Length; i++){
			patrolCoordinates[i] = patrolCoordinates[i-1] + GetDirectionsCoordinatesWorld(dir[i-1]);
		}

		Instantiate(line, transform.position, Quaternion.identity);
 		line.numPositions = dir.Length;
		line.SetPositions(patrolCoordinates);
	}

	void Update () {
		
	}
}
