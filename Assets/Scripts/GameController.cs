using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Enemy;

public class GameController : MonoBehaviour {
	public static int enemyCount;

	private	static GameObject[] enemies;
	private static bool playerTurn = true;
	private static int turnsCount = 1;

	public Text turnsCountText;

	void Start () {
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			enemyCount = enemies.Length;

			if(enemies == null){
				Debug.LogWarning("No enemies in scene");
			}
			turnsCountText.text = turnsCount.ToString();
	}

	

	public void EndPlayerTurn () {
		playerTurn = false;
		turnsCount++;
		turnsCountText.text = turnsCount.ToString();
		DoEnemiesActions();
		BeginPlayerTurn();
	}
	public void BeginPlayerTurn () {
		playerTurn = true;
	}
	public static bool ReturnPlayerTurn () {
		return playerTurn;
	}

	public static int returnTurnsCount () {
		return turnsCount;
	}

	public static void DoEnemiesActions () {
			foreach(GameObject enemy in enemies){
				if(enemy != null){
					EnemyController _ec = enemy.GetComponent<EnemyController>() as EnemyController;
					if(_ec != null) {
						_ec.DoNextAction();
					}
				}
			}
	}

	public static void RemoveEnemy (GameObject enemy) {
		//update enemy list
		for(int i=0; i<enemies.Length; i++){
			if(enemies[i].Equals(enemy)){
				enemies[i] = null;
			}
		}

		foreach(GameObject en in enemies){
			if(en != null){
				Debug.Log(en.name);
			}
		}
		//remove enemy from scene
		Destroy(enemy);
	}
}
