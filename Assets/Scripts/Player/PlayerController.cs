using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
	public class PlayerController : MonoBehaviour {

		private bool isHorizontalInUse;
		private bool isVerticalInUse;
		private bool isRotationInUse;
		public static bool isFrozen = false;
		private GameController gc;
		private PlayerGridsHandler gh = null;
		private Toggle sprintToggle;


		delegate Vector3 moveDirDelegate();

		public int distance = Constants.gridSize;
		public float diagonalDistance = Constants.sqrt;
		public int rotationAmount = 45;
		public GameObject killGrid;
		public GameObject stunGrid;

		


		void Start () {
			gc = FindObjectOfType<GameController>();
			if(gc == null){
				Debug.LogError("GameController not found");
			}
			sprintToggle = GameObject.Find("Sprint Toggle").GetComponent<Toggle>();
		}
		

		void Update () {
			// if it is the player's turn and a key is pressed
			// do corresponding action and end turn

			if(isFrozen){
				return;
			}

			//movement
			if(Input.GetButtonDown("Sprint")){
				//apparently, upon changing the value of isOn, OnValueChanged is triggered too
				sprintToggle.isOn = !sprintToggle.isOn;
			}
				
			if(GameController.ReturnPlayerTurn() && CheckAxisUsage("Vertical", ref isVerticalInUse) == false){
				Move(GetVerticalMovement);
			}

			if(GameController.ReturnPlayerTurn() && CheckAxisUsage("Horizontal", ref isHorizontalInUse) == false){
				Move(GetHorizontalMovement);
			}

			if(GameController.ReturnPlayerTurn() && CheckAxisUsage("Rotation", ref isRotationInUse) == false){
				transform.Rotate( rotationAmount * Input.GetAxisRaw("Rotation") * Vector3.up );
			}

			//other actions
			if(Input.GetButtonDown("SkipTurn")){
				gc.EndPlayerTurn();
			}
		}

		//toggles
		public void ToggleKillGrid (bool val) {
			if(gh == null){
				gh = gameObject.AddComponent<PlayerGridsHandler>() as PlayerGridsHandler;
			}
			if(val){
				gh.SetValues(killGrid, Kill);
				gh.EnableSelectionGrid();
			} else {
				gh.DisableSelectionGrid();
				gh = null;
			}
		}

		public void ToggleStunGrid (bool val) {
			if(gh == null){
				gh = gameObject.AddComponent<PlayerGridsHandler>() as PlayerGridsHandler;
			}
			if(val){
				gh.SetValues(stunGrid, Stun);
				gh.EnableSelectionGrid();
			} else {
				gh.DisableSelectionGrid();
				gh = null;
			}
		}

		public void ToggleSprint (bool val) {
			if(GameController.ReturnPlayerTurn()){
				distance = (distance == Constants.gridSize ? Constants.gridSize*2 : Constants.gridSize);
				diagonalDistance = (distance == Constants.sqrt ? Constants.sqrt*2 : Constants.sqrt);
			}
		}

		private bool CheckAxisUsage (string axisString, ref bool isAxisInUse) {	//returns false if axis is in use
			if(isAxisInUse == false && Input.GetAxisRaw(axisString) != 0){
				isAxisInUse = true;
				return false;
			}

			if(Input.GetAxisRaw(axisString) == 0){
				isAxisInUse = false;
			}

			return true;
		}

		//MOVEMENT
		
		private void Move (moveDirDelegate movementDirection) {
				Debug.DrawRay(transform.position, distance * movementDirection(), Color.magenta, 10f);
				if(Mathf.Approximately( Mathf.RoundToInt( transform.rotation.eulerAngles.y ) % 10, 0)){ //not working properly
					if(Physics.Raycast(transform.position, distance * movementDirection(), distance) == false){
						transform.Translate(distance * movementDirection(), Space.World);
						gc.EndPlayerTurn();
					}
				}
				else {
					if(Physics.Raycast(transform.position, diagonalDistance * movementDirection(), distance) == false){
						transform.Translate(diagonalDistance * movementDirection(), Space.World);
						gc.EndPlayerTurn();
					}
				}
					

		}

		/* TO BE USED IN CASE transform.rotate() GOES HAYWIRE
		*
		*private Quaternion GetRotationTarget (int direction) {
		*	direction *= rotationAmount;
		*	Debug.Log(transform.rotation.eulerAngles.y);
		*	int _y = Mathf.RoundToInt( transform.rotation.eulerAngles.y );
		*	_y %= 360;
		*	Debug.Log(_y);
		*	Quaternion target = Quaternion.Euler(0, _y, 0);
		*
		*	return target;
		}*/

		private Vector3 GetVerticalMovement () {
			if(Input.GetAxisRaw("Vertical") == 1){
				return transform.forward;
			}
			else return -transform.forward;
		}
		private Vector3 GetHorizontalMovement () {
			if(Input.GetAxisRaw("Horizontal") == 1){
				return transform.right;
			}
			else return -transform.right;
		}

		//COMBAT

		void Kill (GameObject obj) {
			GameController.RemoveEnemy(obj);
			gc.EndPlayerTurn();
		}

		void Stun (GameObject obj) {
			StartCoroutine(obj.GetComponent<EnemyController>().FreezeForTurns(4));
			gc.EndPlayerTurn();
		}

	}
}