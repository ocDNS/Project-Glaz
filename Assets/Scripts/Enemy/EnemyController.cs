using UnityEngine;
using System.Collections;

public enum Directions {Forward, Right, Left, Backward};

public class EnemyController : MonoBehaviour {

	//VARIABLES
	private int rotationAmount = 90;
	private int distance = Constants.gridSize;
	private int pathIndex = 0;
	private Directions[] patrolPathWorld;
	private Directions[] patrolPathWorldReverse;
	private bool isReversed = false;
	private bool isFrozen = false;
	private bool isInPursuit = false;
	private EnemyFieldOfViewController enemyFOV;
	private Pathfinding pf;
	private Vector3[] pathfindingDirections;
	private int pathfindingIndex = 0;
	public Directions[] patrolPath;

	//PROPERTIES
	public Directions[] PatrolPathWorld { get{ return patrolPathWorld; } }

	void Awake () {
		//convert patrolPath into world space
		patrolPathWorld = new Directions[patrolPath.Length];
		patrolPathWorld[0] = patrolPath[0];
		for(int i=1; i<patrolPathWorld.Length; i++){
			patrolPathWorld[i] = CalculateWorldDirection(patrolPathWorld[i-1], patrolPath[i]);
		}

		// initialize the return path for the enemy
		patrolPathWorldReverse = new Directions[patrolPathWorld.Length];
		for(int i=0; i<patrolPathWorldReverse.Length; i++){
			patrolPathWorldReverse[i] = GetReverseDirection( patrolPathWorld[patrolPathWorld.Length - 1 - i] );
		}
	}

	void Start () {
		enemyFOV = transform.GetComponent<EnemyFieldOfViewController>();
		if(enemyFOV == null){
			Debug.LogError("Enemy field of view not found");
		}
		pf = gameObject.AddComponent<Pathfinding>();
	}

	public void DoNextAction () {
		if(isFrozen){
			return;
		}

		if(enemyFOV.isPlayerInSight){
			pathfindingDirections = pf.CalculatePath(enemyFOV.playerTransform);
			pathfindingIndex = 0;
			isInPursuit = true;
		}

		if(isInPursuit){
			if(pathfindingIndex < pathfindingDirections.Length - 1){
				Move(pathfindingDirections, pathfindingIndex);
				pathfindingIndex++;
			}
			else{
				isInPursuit = false;
			}
		}
		else{
			if(pathIndex < patrolPathWorld.Length){
				if(!isReversed){
					MoveFrontBack(patrolPathWorld, pathIndex);
				} else MoveFrontBack(patrolPathWorldReverse, pathIndex);
				pathIndex++;
			}

			if(pathIndex == patrolPathWorld.Length){
				Directions _dir = !isReversed ? patrolPathWorld[patrolPathWorld.Length - 1] : patrolPathWorldReverse[patrolPathWorld.Length - 1]; //retrieve last element of the current directions array
				Turn180(_dir);
				pathIndex = 0;
				isReversed = !isReversed;
			}
		}
	}

	public IEnumerator FreezeForTurns (int turns) {
		isFrozen = true;
		turns +=  GameController.returnTurnsCount();
		while(GameController.returnTurnsCount() < turns){
			yield return null; //in case of performace issues, change to waitforseconds()
		}
		isFrozen = false;
	}

	void MoveFrontBack (Directions[] path, int pathIndex) {
		if(pathIndex == 0 || !path[pathIndex-1].Equals(path[pathIndex])){
			Rotate(path[pathIndex]);
		}
		transform.Translate(GetDirectionsCoordinatesWorld(path[pathIndex]) * distance, Space.World);
	}

	void Move(Vector3[] path, int pathIndex){
		transform.Translate(path[pathIndex]);
	}

	void Turn180 (Directions currentDir) {
		Directions _dir = GetReverseDirection(currentDir);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, GetRotationDirection(_dir), 0), rotationAmount*2);
	}

	void Rotate (Directions dir) {
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, GetRotationDirection(dir), 0), rotationAmount);
	}

	int GetRotationDirection (Directions dir) {
		if(dir.Equals(Directions.Left)){
			return -90;
		} else if(dir.Equals(Directions.Right)){
			return 90;
		} else if(dir.Equals(Directions.Forward)){
			return 0;
		} else return 180;
	}

	Directions GetReverseDirection (Directions dir) {
		if(dir.Equals(Directions.Forward)){
			return Directions.Backward;
		} else if(dir.Equals(Directions.Backward)){
			return Directions.Forward;
		} else if(dir.Equals(Directions.Left)) {
			return Directions.Right;
		} else return Directions.Left;
	}

	Directions CalculateWorldDirection (Directions prevDir, Directions curDir){
		if(prevDir.Equals(Directions.Forward)){
			return curDir;
		} else if(prevDir.Equals(curDir)){
			return Directions.Backward;
		} else if(curDir.Equals(Directions.Forward)){
			return prevDir.Equals(Directions.Left) ? Directions.Left : Directions.Right;
		} else return Directions.Forward;
	}

	public Vector3 GetDirectionsCoordinatesWorld (Directions dir){
		if(dir.Equals(Directions.Forward)){
			return Vector3.forward;
		} else if(dir.Equals(Directions.Right)){
			return Vector3.right;
		} else if(dir.Equals(Directions.Left)){
			return -Vector3.right;
		} else return -Vector3.forward;
	}

	/*public Vector3 GetDirectionsCoordinates(Directions dir) { //function does not return the correct coordinates
		if(dir.Equals(Directions.Forward)){
			return transform.forward;
		} else if(dir.Equals(Directions.Right)){
			return transform.right;
		} else if(dir.Equals(Directions.Left)){
			return -transform.right;
		} else return -transform.forward;
	}*/
}