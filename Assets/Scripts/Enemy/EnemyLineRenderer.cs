using UnityEngine;

	public class EnemyLineRenderer : MonoBehaviour {

		private EnemyController ec;
		private Vector3[] patrolCoordinates;
		private Vector3 _pos;

		public LineRenderer patrolLine;

		void Start () {
			ec = gameObject.GetComponent<EnemyController>();
			//Debug.Log(gameObject.name);

			// save a copy of the path's directions (in world space) as world coordinates
			patrolCoordinates = new Vector3[ec.patrolPathWorld.Length+1];
			
			_pos = new Vector3(transform.position.x, patrolLine.startWidth, transform.position.z);

			patrolCoordinates[0] = _pos;
			for(int i=1; i<patrolCoordinates.Length; i++){
				patrolCoordinates[i] = patrolCoordinates[i-1] + ec.GetDirectionsCoordinatesWorld(ec.patrolPathWorld[i-1]);
			}

			//set line renderer positions
			Instantiate(patrolLine, transform.position, Quaternion.identity);
			patrolLine.numPositions = patrolCoordinates.Length;
			patrolLine.SetPositions(patrolCoordinates);
		}
	}