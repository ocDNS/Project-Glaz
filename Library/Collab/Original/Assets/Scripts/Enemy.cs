using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	[System.Serializable]
	public enum Directions {Left, Right, Forward, Backward, TurnLeft, TurnRight, Stationary};
	public List<Directions> path;

	private  float distance = 1;
	private float timer;
	public  float maxTimerValue = 10;
	private  int pathIndex;
	private  int rotationAmount = 45;
	public  bool reverse = true;
	private bool rev_path;
	void Start () {
		pathIndex = 0;
		//GameController.enemyCount++;
	}

	void Update () {

		if (path.Count != 0) {
			timer += Time.deltaTime;

			if (timer > maxTimerValue) {
				timer = 0;
				if (pathIndex > path.Count - 1) {
					pathIndex = 0;
					if (reverse) {
						path.Reverse ();
						rev_path = !rev_path;
						Rotate (Directions.TurnLeft);
						Rotate (Directions.TurnLeft);
					}
				} else {
					if (path [pathIndex] != Directions.Stationary) {
						if (isHozAndVert (path [pathIndex]))
							Movement (getHozAndVert (path [pathIndex]));
						else
							Rotate ((rev_path && reverse) ? rev_dir (path [pathIndex]) : path [pathIndex]);
					}

					pathIndex += 1;
				}
			}
		}
	}

	void Movement (Vector3 movementDirection) {
		//Debug.DrawRay(transform.position, distance * movementDirection(), Color.magenta, 10f);
		//Debug.Log(distance * movementDirection());
		//Debug.Log("pos " + transform.position);
		if (Physics.Raycast (transform.position, distance * movementDirection, distance) == false) {
			transform.Translate (distance * movementDirection, Space.World);
		}
	} 

	void Rotate(Directions way){
		transform.Rotate(new Vector3(0, rotationAmount * ((way.Equals(Directions.TurnLeft)) ? -1 : 1), 0));
		transform.Rotate(new Vector3(0, rotationAmount * ((way.Equals(Directions.TurnLeft)) ? -1 : 1), 0));
	}

	Vector3 getHozAndVert(Directions way){
		if (way.Equals( Directions.Forward))
			return transform.forward;
		else if (way.Equals(Directions.Backward))
			return -transform.forward;
		else if (way.Equals(Directions.Right))
			return transform.right;
		else if (way.Equals( Directions.Left))
			return -transform.right;
		else
			return Vector3.zero;
	}

	bool isHozAndVert(Directions way){
		return (way.Equals (Directions.Forward) ||
			way.Equals (Directions.Backward) ||
			way.Equals (Directions.Left) ||
			way.Equals (Directions.Right));
	}

	Directions rev_dir(Directions way){
		if (way.Equals (Directions.Left))
			return Directions.Right;
		else if(way.Equals (Directions.Right))
			return Directions.Left;
		else if(way.Equals (Directions.Forward))
			return Directions.Backward;
		else if(way.Equals (Directions.Backward))
			return Directions.Forward;
		else if(way.Equals (Directions.TurnLeft))
			return Directions.TurnRight;
		else 
			return Directions.TurnLeft;
	}
}
