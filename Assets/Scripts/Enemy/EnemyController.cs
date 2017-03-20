using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions {Forward, Right, Backward, Left};

public class EnemyController : MonoBehaviour {

	private int rotationAmount = 90;
	private int distance = Constants.gridSize;
	private int pathIndex = 0;
	private Directions[] patrolPathReverse;
	private bool isReversed;

	public Directions[] patrolPath;

	void Start () {
		// initialize the return path for the enemy
		patrolPathReverse = new Directions[patrolPath.Length];
		patrolPathReverse[0] = GetReverseDirection(patrolPath[0]);
		for(int i=1; i<patrolPath.Length; i++){
			patrolPathReverse[i] = GetReverseDirection(patrolPath[patrolPath.Length-i]);
		}

		

		/*for(int i=0; i<patrolPathReverse.Length; i++){
			Debug.Log(i + "  " + patrolPathReverse[i]);
		}*/
	}

	void Update () {
	}

	public void DoNextAction () {
		//Debug.Log(gameObject.name + "  " + pathIndex + "  " + isReversed);
		if(pathIndex < patrolPath.Length){
			if(!isReversed){
				Move(patrolPath, pathIndex);
			} else Move(patrolPathReverse, pathIndex);
			pathIndex++;
		}
		if(pathIndex == patrolPath.Length){
			Turn180();
			pathIndex = 0;
			isReversed = !isReversed;
		}
	}

	private void Move (Directions[] path, int pathIndex) {
		Rotate(path[pathIndex]);
		transform.Translate(transform.forward * distance, Space.World);
		
	}

	private void Turn180 () {
		Rotate(Directions.Left);
		Rotate(Directions.Left);
	}

	private void Rotate (Directions dir) {
		transform.Rotate(GetRotationDirection(dir) * rotationAmount *transform.up);
	}

	private int GetRotationDirection (Directions dir) {
		if(dir == Directions.Left){
			return -1;
		} else if(dir == Directions.Right){
			return 1;
		} else return 0;
	}

	private Directions GetReverseDirection (Directions dir) {
		if(dir.Equals(Directions.Forward)){
			return dir;
		} else if(dir.Equals(Directions.Left)) {
			return Directions.Right;
		} else return Directions.Left;
	}

	public Vector3 GetDirectionsCoordinates(Directions dir) { //function does not return the correct coordinates
		if(dir.Equals(Directions.Forward)){
			return transform.forward;
		} else if(dir.Equals(Directions.Right)){
			return transform.right;
		} else if(dir.Equals(Directions.Left)){
			return -transform.right;
		} else return -transform.forward;
	}

}
