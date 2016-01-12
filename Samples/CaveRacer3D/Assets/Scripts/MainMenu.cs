using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Text BestScoresText;

	public void Start() {
		float score = PlayerPrefs.GetFloat ("score", 0f);

		if (score > 0) {
			BestScoresText.text = "BEST SCORE: " + Mathf.Floor(score).ToString();
		} else {
			BestScoresText.text = "";
		}
	}

	public void StartGame() {
		Application.LoadLevel (1);
	}

	public void ExitGame() {
		Application.Quit ();
	}
	
	public void Update() {
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		if (Input.GetKey (KeyCode.Return)) 
			StartGame ();
		if (Input.GetKey (KeyCode.Escape)) 
			ExitGame ();
		#endif
	}
}
