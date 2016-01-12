using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour {
	public Logic logic;
	public GameObject TopList;
	public GameObject TopItemPrefab;
	private bool btnTrigger = false;
	
	// Use this for initialization
	void Start () {

	}

	void OnEnable() {
		btnTrigger = true;

		LevelProgress prog = logic.levelProgress;

		for (int i = 0; i < prog.scores.Count; i++) {
			Text t = TopList.transform.GetChild(i).GetComponent<Text>();
			t.text = String.Format("{1} - {0} s", prog.scores[i].LevelTime.ToString(".00"),
			                       prog.scores[i].UserName);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return) && !btnTrigger) {
			logic.StartGame();
		}
		
		if (Input.GetKey (KeyCode.Escape) && !btnTrigger) {
			logic.TerminateGame();
		}

		if (!Input.GetKey (KeyCode.Return) && !Input.GetKey(KeyCode.Escape)) 
			btnTrigger = false;
	}
}
