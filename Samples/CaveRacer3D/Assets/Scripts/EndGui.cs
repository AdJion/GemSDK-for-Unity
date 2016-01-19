using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGui : MonoBehaviour {
	private const string ScoreKey = "score";
    private float best;
    private bool inited = false;

	public Text BestScore;
	public Text YourScore;

	public float Score { 
		set {
			SetValue(YourScore, ((int)value).ToString());
		}
	}

	// Use this for initialization
	void Start () {

    }

    void OnEnable()
    {
        best = PlayerPrefs.GetFloat(ScoreKey, 0f);
        inited = true;
        SetValue(BestScore, ((int)best).ToString());
    }

	private void SetValue(Text t, string val) {
		string[] arr =  t.text.Split (new char[] {':'});
		if (arr.Length > 1)
			t.text = arr [0] + ": " + val;
		else
			t.text = val;
	}
}
