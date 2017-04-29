using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour {

	public GameObject inGame;
	public GameObject GameMenu;


	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			ToggleGameMenu();
		}
	}

	void ToggleGameMenu () {
		inGame.SetActive(!inGame.activeSelf);
		GameMenu.SetActive(!GameMenu.activeSelf);
	}

	public void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void GoToLevelSelection () {
		//to implement
	}

	public void Quit () {
		Application.Quit();
	}
}
