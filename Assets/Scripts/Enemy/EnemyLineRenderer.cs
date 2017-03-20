using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineRenderer : MonoBehaviour {

	private EnemyController ec;
	public Directions[] patrolPathWorld;
	private Vector3[] patrolCoordinates;
	private Vector3 _pos;

	public LineRenderer patrolLine;

	void Start () {
		ec = gameObject.GetComponent<EnemyController>();
		patrolPathWorld = new Directions[ec.patrolPath.Length];

		//calculate the path's directions in world space
		patrolPathWorld[0] = ec.patrolPath[0];
		for(int i=1; i<patrolPathWorld.Length; i++){
			patrolPathWorld[i] = CalculateWorldDirection(patrolPathWorld[i-1], ec.patrolPath[i]);
		}

		// save a copy of the path's directions (in world space) as world coordinates
		patrolCoordinates = new Vector3[ec.patrolPath.Length+1];
		
		
		 _pos = new Vector3(transform.position.x, patrolLine.startWidth, transform.position.z);

		patrolCoordinates[0] = _pos;
		for(int i=1; i<=patrolPathWorld.Length; i++){
			patrolCoordinates[i] = patrolCoordinates[i-1] + GetDirectionsCoordinatesWorld(patrolPathWorld[i-1]);
		}

		//set line renderer positions
		Instantiate(patrolLine, transform.position, Quaternion.identity);
		patrolLine.numPositions = ec.patrolPath.Length+1;
		patrolLine.SetPositions(patrolCoordinates);
	}

	private Directions CalculateWorldDirection (Directions dir1, Directions dir2) {
		if(dir1.Equals(Directions.Forward)){
			return dir2; //GetDirectionsCoordinates(dir2);
		} else if(dir1.Equals(dir2)){
			return Directions.Backward; //-transform.forward;
		} else if(dir2.Equals(Directions.Forward)){
			return Directions.Right; //transform.right;
		} else return Directions.Forward; //transform.forward;
	}

	private Vector3 GetDirectionsCoordinatesWorld(Directions dir){
		if(dir.Equals(Directions.Forward)){
			return Vector3.forward;
		} else if(dir.Equals(Directions.Right)){
			return Vector3.right;
		} else if(dir.Equals(Directions.Left)){
			return -Vector3.right;
		} else return -Vector3.forward;
	}
}
