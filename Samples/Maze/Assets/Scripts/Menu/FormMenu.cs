using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FormMenu : MonoBehaviour {
	public Logic logic;
	public Text ScoreText;
	public InputField input;
	public string DefaultPlayerName;

	private UserScore userScore;

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable() {
		input.Select ();
		input.ActivateInputField ();
	}

	public void SetUserScore(UserScore score) {
		userScore = score;
		ScoreText.text = userScore.LevelTime.ToString (".00") + " s";
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return)) {
            SubmitName();
		}
		
		if (Input.GetKey (KeyCode.Escape)) {
            CancelName();
		}	
	}

    public void CancelName()
    {
        userScore.UserName = DefaultPlayerName;
        logic.MainMenu();
    }

    public void SubmitName()
    {
        if (input.text.Trim().Length == 0)
            userScore.UserName = DefaultPlayerName;
        else
            userScore.UserName = input.text;
        logic.MainMenu();
    }
}
