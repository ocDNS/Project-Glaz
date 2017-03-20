using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static int enemyCount;

	private	GameObject[] enemies;
	private static bool playerTurn = true;

	void Start () {
		if(enemies == null){
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			enemyCount = enemies.Length;
		}
	}

	public void EndPlayerTurn () {
		playerTurn = false;
		DoEnemiesActions();
	}
	public void BeginPlayerTurn () {
		playerTurn = true;
	}
	public static bool ReturnPlayerTurn () {
		return playerTurn;
	}

	public void DoEnemiesActions () {
			foreach(GameObject enemy in enemies){
				EnemyController _ec = enemy.GetComponent<EnemyController>(); 
				_ec.DoNextAction();
			}
	}
}
