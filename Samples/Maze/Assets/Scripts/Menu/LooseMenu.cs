using UnityEngine;
using System.Collections;

public class LooseMenu : MonoBehaviour {
	public Logic logic;
	
	// Use this for initialization
	void Start () {
	
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
