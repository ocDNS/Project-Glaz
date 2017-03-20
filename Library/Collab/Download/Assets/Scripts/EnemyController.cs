using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	[System.Serializable]
	public enum Directions {Forward, Backward, TurnLeft, TurnRight, Stationary};
	public List<Directions> path, rev_path;
	public Directions current;

	public int distance = Constants.gridSize;
	public int pathIndex;
	public float time; 
    public float maxMovementTime = 2;
	public float diagonalDistance = Constants.sqrt;
	public bool isRunning;
	private bool reverse;

	void Start () {
		initRevPath ();
		pathIndex = 0;
		time = 0;
		reverse = false;
		isRunning = true;
	}

	void Update () {
		Debug.Log (Mathf.Round(transform.forward.x) + " " + Mathf.Round(transform.forward.z));
		if (isRunning) {
			time += Time.fixedDeltaTime;	

			if (time >= maxMovementTime) {
				if (pathIndex == path.Count) {
					pathIndex = -1;
					reverse = !reverse;
					Rotate (Directions.TurnLeft, 180);

				} else {
					current = ((!reverse) ? path[pathIndex] : rev_path[pathIndex]);

					if (!current.Equals (Directions.Stationary)) {
						if (isHozMove (current))
							Move (current);
						else
							Rotate (current, 90);
					}
				}
				pathIndex += 1;
				time = 0;
			}
		}
	}

	void initRevPath(){
		rev_path = path.ToList();
		rev_path.Reverse ();
	}

	void Move (Directions way) {
		if (Physics.Raycast (transform.position, distance * transform.forward, distance) == false) {
			transform.Translate (distance * transform.forward, Space.World);
		}
	} 


	void Rotate(Directions way, int rotationAmount){
		int facing = ((way.Equals (Directions.TurnLeft)) ? -1 : 1);
		transform.Rotate (rotationAmount * facing * transform.up);
	}

	bool isHozMove(Directions way){
		return ((way.Equals (Directions.Forward) || way.Equals (Directions.Backward)) ? true : false);
	}
}
