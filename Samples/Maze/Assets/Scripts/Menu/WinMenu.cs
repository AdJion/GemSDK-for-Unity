using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour {
	public Logic logic;
	public Text scoreText;

	public void setScore(float score) {
		scoreText.text = score.ToString (".00") + " s";
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return)) {
			logic.StartGame();
		}

		if (Input.GetKey (KeyCode.Escape)) {
			logic.MainMenu();
		}
	}
}
