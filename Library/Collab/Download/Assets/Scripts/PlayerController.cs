using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private bool isHorizontalInUse;
	private bool isVerticalInUse;
	private bool isRotationInUse;
	
	delegate Vector3 moveDirDelegate();

	public int distance = Constants.gridSize;
	public float diagonalDistance = Constants.sqrt;
	public int rotationAmount = 45;
	


	void Start () {
	}
	

	void Update () {
		if(Input.GetButtonDown("Sprint")){
			distance = (distance==Constants.gridSize ? Constants.gridSize*2 : Constants.gridSize);
			diagonalDistance = (distance == Constants.sqrt ? Constants.sqrt*2 : Constants.sqrt);
		}
		
		if(CheckAxisUsage("Vertical",ref isVerticalInUse) == false){
			Move(GetVerticalMovement);
		}

		if(CheckAxisUsage("Horizontal", ref isHorizontalInUse) == false){
			Move(GetHorizontalMovement);
		}

		if(CheckAxisUsage("Rotation", ref isRotationInUse) == false){
			//Debug.Log(rotationAmount * Input.GetAxisRaw("Rotation") * transform.up);
			transform.Rotate(rotationAmount * Input.GetAxisRaw("Rotation") * transform.up);
		}
		
	}



	bool CheckAxisUsage (string axisString, ref bool isAxisInUse) {
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
	void Move (moveDirDelegate movementDirection) {
			Debug.DrawRay(transform.position, distance * movementDirection(), Color.magenta, 10f);
			//Debug.Log(transform.position);
			//Debug.Log("local" + transform.localPosition);
			if((int)transform.rotation.eulerAngles.y % 10 == 0){
				if(Physics.Raycast(transform.position, distance * movementDirection(), distance) == false){
					transform.Translate(distance * movementDirection(), Space.World);
				}
			}
			else{
				if(Physics.Raycast(transform.position, diagonalDistance * movementDirection(), distance) == false){
					transform.Translate(diagonalDistance * movementDirection(), Space.World);
				}
			}
				

	}

	Vector3 GetVerticalMovement () {
		//Debug.Log(Input.GetAxisRaw("Vertical"));
		if(Input.GetAxisRaw("Vertical") == 1){
			return transform.forward;
		}
		else return -transform.forward;
	}
	Vector3 GetHorizontalMovement () {
		//Debug.Log(Input.GetAxisRaw("Horizontal"));
		if(Input.GetAxisRaw("Horizontal") == 1){
			return transform.right;
		}
		else return -transform.right;
	}

}
