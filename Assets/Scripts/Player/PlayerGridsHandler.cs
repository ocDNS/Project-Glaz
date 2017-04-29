using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
	public class PlayerGridsHandler : MonoBehaviour {

		private bool isInSelectionMode = false;
		private GameObject grid;

		public delegate void gridActionDelegate(GameObject obj);
		public gridActionDelegate DoAction;


		IEnumerator CheckForInput () {
			GameObject[] objectsHovered;
			while(isInSelectionMode){
				objectsHovered = MouseManager.DrawMouseRay();
				if(objectsHovered == null){
					continue;
				}

				foreach(GameObject obj in objectsHovered){
					if(obj.tag == "Enemy"){
						if(isInGrid(obj) && Input.GetMouseButtonDown(0)){
							DoAction(obj);
							DisableSelectionGrid();
						}
					}
				}
			yield return null;
			}
		}

		public void SetValues (GameObject _grid, gridActionDelegate _action) {
			grid = _grid;
			DoAction = _action;
		}

		public void EnableSelectionGrid () {
			Vector3 _pos = new Vector3(transform.position.x, 0.001f, transform.position.z);
			grid = Instantiate(grid, _pos, Quaternion.identity);
			isInSelectionMode = true;
			PlayerController.isFrozen = true;
			StartCoroutine(CheckForInput());
		}

		public void DisableSelectionGrid () {
			StopCoroutine(CheckForInput());
			Destroy(grid);
			Destroy(this);
			PlayerController.isFrozen = false;
			isInSelectionMode = false;
		}

		bool isInGrid (GameObject obj) {
			RaycastHit[] hits = Physics.RaycastAll(obj.transform.position, Vector3.down);
			Debug.DrawRay(obj.transform.position, Vector3.down, Color.red, 10f);
			foreach(RaycastHit hit in hits){
				//if(hit.transform.parent != null) Debug.Log(hit.transform.parent.name);
				if(hit.transform.parent != null && hit.transform.parent.gameObject == grid){
					return true;
				}
			}
			return false;
		}

		/*bool isInGrid (GameObject grid, GameObject other) {
			if(Physics.Raycast(other.transform.position, Vector3.down, out RaycastHit hit)){
				if(hit.Equals(grid)){
					return true;
				}
			}
			return false;
		}*/
	}
}
