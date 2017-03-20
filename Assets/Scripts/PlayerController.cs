using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private bool isHorizontalInUse;
	private bool isVerticalInUse;
	private bool isRotationInUse;
	private GameController gc;
	
	
	delegate Vector3 moveDirDelegate();

	public int distance = Constants.gridSize;
	public float diagonalDistance = Constants.sqrt;
	public int rotationAmount = 45;
	


	void Start () {
		gc = FindObjectOfType<GameController>();
		if(gc == null){
			Debug.LogError("GameController not found");
		}
	}
	

	void Update () {
		// if it is the player's turn and a key is pressed
		// do corresponding action and end turn
		gc.BeginPlayerTurn();
		if(Input.GetButtonDown("Sprint")){
			ToggleSprint();
		}
			
		if(GameController.ReturnPlayerTurn() && CheckAxisUsage("Vertical",ref isVerticalInUse) == false){
			Move(GetVerticalMovement);
			gc.EndPlayerTurn();
		}

		if(GameController.ReturnPlayerTurn() && CheckAxisUsage("Horizontal", ref isHorizontalInUse) == false){
			Move(GetHorizontalMovement);
			gc.EndPlayerTurn();
		}
		
		if(GameController.ReturnPlayerTurn() && CheckAxisUsage("Rotation", ref isRotationInUse) == false){
			transform.Rotate(rotationAmount * Input.GetAxisRaw("Rotation") * transform.up);
		}
	}

	public void ToggleSprint () {
		if(GameController.ReturnPlayerTurn()){
			distance = (distance==Constants.gridSize ? Constants.gridSize*2 : Constants.gridSize);
			diagonalDistance = (distance == Constants.sqrt ? Constants.sqrt*2 : Constants.sqrt);
		}
	}

	private bool CheckAxisUsage (string axisString, ref bool isAxisInUse) {		//returns false is axis is in use
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
			//Debug.Log(transform.position);
			//Debug.Log("local" + transform.localPosition);
			if((int)transform.rotation.eulerAngles.y % 10 == 0){
				if(Physics.Raycast(transform.position, distance * movementDirection(), distance) == false){
					transform.Translate(distance * movementDirection(), Space.World);
				}
			}
			else {
				if(Physics.Raycast(transform.position, diagonalDistance * movementDirection(), distance) == false){
					transform.Translate(diagonalDistance * movementDirection(), Space.World);
				}
			}
				

	}

	private Vector3 GetVerticalMovement () {
		//Debug.Log(Input.GetAxisRaw("Vertical"));
		if(Input.GetAxisRaw("Vertical") == 1){
			return transform.forward;
		}
		else return -transform.forward;
	}
	private Vector3 GetHorizontalMovement () {
		//Debug.Log(Input.GetAxisRaw("Horizontal"));
		if(Input.GetAxisRaw("Horizontal") == 1){
			return transform.right;
		}
		else return -transform.right;
	}

}
