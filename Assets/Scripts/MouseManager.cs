using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	public static GameObject[] DrawMouseRay () {
		//doesn't work without static keyword, for some reason
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll(mouseRay);
		GameObject[] obj = new GameObject[hits.Length];
		for(int i=0; i<hits.Length; i++){
			obj[i] = hits[i].transform.gameObject;
		}
		return obj;
	}
}
