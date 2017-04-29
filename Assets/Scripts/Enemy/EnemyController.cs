using UnityEngine;

	public enum Directions {Forward, Right, Left, Backward};

	public class EnemyController : MonoBehaviour {

		private int rotationAmount = 90;
		private int distance = Constants.gridSize;
		private int pathIndex = 0;
		public Directions[] patrolPathWorld; //to build a read-only property
		private Directions[] patrolPathWorldReverse;
		private bool isReversed;
		
		public bool isFrozen = false;
		public Directions[] patrolPath;

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

		public void DoNextAction () {
			if(isFrozen){
				return;
			}

			if(pathIndex < patrolPathWorld.Length){
				if(!isReversed){
					Move(patrolPathWorld, pathIndex);
				} else Move(patrolPathWorldReverse, pathIndex);
				pathIndex++;
			}

			if(pathIndex == patrolPathWorld.Length){
				Directions _dir = !isReversed ? patrolPathWorld[patrolPathWorld.Length - 1] : patrolPathWorldReverse[patrolPathWorld.Length - 1]; //retrieve last element of the current directions array
				Turn180(_dir);
				pathIndex = 0;
				isReversed = !isReversed;
			}
		}

		void Move (Directions[] path, int pathIndex) {
			if(pathIndex == 0 || !path[pathIndex-1].Equals(path[pathIndex])){
				Rotate(path[pathIndex]);
			}
			transform.Translate(GetDirectionsCoordinatesWorld(path[pathIndex]) * distance, Space.World);
			
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