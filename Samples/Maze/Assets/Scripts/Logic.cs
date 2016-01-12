using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour {
	public GameField field;
	public Player player;

	public GameObject DarkTex;
	public WinMenu WinMenuScript;
	public MainMenu MainMenuScript;
	public LooseMenu LooseMenuScript;
	public FormMenu FormMenuScript;

	public AudioClip winMusic;
	public AudioClip looseMusic;

	private AudioSource audioPlayer;
	private bool isPlaying = false;

	private ProgressManager progress;
	private float levelStartTime;

	public LevelProgress levelProgress {
		get {
			if (progress == null) {
				progress = new ProgressManager ();

				LevelProgress lp;
				if (!progress.levels.TryGetValue (Application.loadedLevelName, out lp)) {
					lp = new LevelProgress();
					progress.levels.Add(Application.loadedLevelName, lp);
				}

				return lp;
			}
			else {
				return progress.levels[Application.loadedLevelName];
			}
		}
	}

	public void Start() {
		audioPlayer = GetComponent<AudioSource> ();
	}

	public void EndGame(bool win) {
		if(!isPlaying) return;
		isPlaying = false;
		field.isContralable = false;

		DarkTex.SetActive(true);

		if (win) {
			audioPlayer.PlayOneShot (winMusic);

			var tDelta = Time.timeSinceLevelLoad - levelStartTime;
			UserScore score = levelProgress.AddScore(tDelta);

			if(score == null) {
				WinMenuScript.gameObject.SetActive (true);
				WinMenuScript.setScore(tDelta);
			}
			else {
				FormMenuScript.gameObject.SetActive (true);
				FormMenuScript.SetUserScore(score);
			}

		} else {
			LooseMenuScript.gameObject.SetActive (true);
			audioPlayer.PlayOneShot (looseMusic);
		}
	}

	public void MainMenu() {
		DarkTex.SetActive(true);
		MainMenuScript.gameObject.SetActive(true);
		WinMenuScript.gameObject.SetActive(false);
		LooseMenuScript.gameObject.SetActive(false);
		FormMenuScript.gameObject.SetActive(false);		
		progress.Save ();
	}

	public void StartGame() {
		isPlaying = true;

		DarkTex.SetActive(false);
		MainMenuScript.gameObject.SetActive(false);
		WinMenuScript.gameObject.SetActive(false);
		LooseMenuScript.gameObject.SetActive(false);
		FormMenuScript.gameObject.SetActive(false);
		

		field.transform.rotation = Quaternion.identity;
        player.Reset();

		levelStartTime = Time.timeSinceLevelLoad;

        field.Calibrate();
        field.isContralable = true;
	}

	public void TerminateGame() {
		Application.Quit();
	}
	 
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
            if (MainMenuScript.enabled) TerminateGame();
            MainMenuAction();
		}
	}

    public void MainMenuAction()
    {
        if (isPlaying == false) return;
  
        isPlaying = false;
        field.isContralable = false;
        MainMenu();
    }

	public void StartLevel(int id) {
		Application.LoadLevel (id);
	}
}
