using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private bool isHorizontalInUse;
	private bool isVerticalInUse;
	private bool isRotationInUse;
	
	delegate Vector3 moveDirDelegate();

	public int distance = 1;
	public float diagonalDistance = 1.41421356237f;
	public int rotationAmount = 45;
	


	void Start () {
	}
	

	void Update () {
		if(Input.GetButtonDown("Sprint")){
			distance = (distance==1 ? 2 : 1);
			diagonalDistance = (distance<2f ? 2.82842712475f : 1.41421356237f);
		}
		
		if(CheckAxisUsage("Vertical",ref isVerticalInUse) == false){
			Move(GetVerticalMovement);
		}

		if(CheckAxisUsage("Horizontal", ref isHorizontalInUse) == false){
			Move(GetHorizontalMovement);
		}

		if(CheckAxisUsage("Rotation", ref isRotationInUse) == false){
			Debug.Log(rotationAmount * Input.GetAxisRaw("Rotation") * transform.up);
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
