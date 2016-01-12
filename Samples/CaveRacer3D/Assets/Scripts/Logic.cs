using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Tube;


/// <summary>
/// Manages game logic
/// </summary>
[RequireComponent(typeof(GemAngleController))]
public class Logic : MonoBehaviour {
    private GemAngleController gemController; 
	private const string ScoreKey = "score";

	public float ScoreFactor;
    public float BonusScore;
	public int MaxLives;
	public float PlayerSpeed;
	public float PlayerSpeedMax;
	public Text ScoreText;
	public Text LivesText;
    public GameObject Cursor;

	public float BloodLength;
	public Light MainLight;
	public Color BloodColor;
	private Color mainLightColor;
	private float bloodTimer;

	public float Speed;

	public float SafetyTime;
	private float safetyLastTime;

	public TubeManager tubeManager;

	public Player player; 

	private float Score;
	private int Lives;
	private bool paused;

	private bool finished = false;
	private bool started = false;
	private bool enterTrigger = false;
	
	public GameObject GuiWrapper;
	public GameObject StartWrapper;
	public GameObject EndWrapper;

	// Use this for initialization
	void Start () {
		mainLightColor = MainLight.color;

        gemController = GetComponent<GemAngleController>();
		resetParams ();
	}

	public void removeLive() {
		if (Time.timeSinceLevelLoad - safetyLastTime < SafetyTime)
			return;
		else
			safetyLastTime = Time.timeSinceLevelLoad;

		Lives--;

		bloodTimer = 0f;

		if (Lives < 0)
			EndGame ();

		SetValue(LivesText, Lives.ToString ());
	}

	// Update is called once per frame
	void Update () {
		if (finished || !started)
			return;
		
		bloodTimer += Time.deltaTime;

		float bloodIntensity;
		if (bloodTimer <= BloodLength) {
			bloodIntensity = bloodTimer / BloodLength;
		}
		else bloodIntensity = Mathf.Max(0f, 1f - (bloodTimer - BloodLength) / BloodLength);
		MainLight.color = Color.Lerp (mainLightColor, BloodColor, bloodIntensity);
	}

	void FixedUpdate() {
		processInput ();

        float ds = tubeManager.speed / Speed;
        Score += Time.deltaTime * ScoreFactor * ds * ds;
        SetValue(ScoreText, ((int)Score).ToString());

		if(!finished && started) {
			float dSpeedUp = Speed / 2f;
            float dSpeedDown = Speed / 3f;

            tubeManager.speed = Speed + Mathf.Clamp(-gemController.Elevation * 3.7f, -dSpeedDown, dSpeedUp);
			
			player.speed = Mathf.Clamp(gemController.Tilt, -PlayerSpeedMax, PlayerSpeedMax)  * PlayerSpeed;
		}
	}

	private void processInput() {
		bool action = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return) || (Input.touches.Length == 1);
        
		if (action && !started && !enterTrigger) {
			StartGame();
			enterTrigger = true;
		}
		
		if (action && finished && !enterTrigger) {
			RestartGame();
			enterTrigger = true;
		}
		
		if (action && !finished && started && !enterTrigger) {
			EndGame();
			enterTrigger = true;
		}
		
		if(!action)
			enterTrigger = false;
	}

	private void SetValue(Text t, string val) {
		string[] arr =  t.text.Split (new char[] {':'});
		if (arr.Length > 1)
			t.text = arr [0] + ": " + val;
		else
			t.text = val;
	}

    public void EndGame()
    {
        Debug.Log("EndGame");
		finished = true;

		tubeManager.speed = 0f;
		player.speed = 0f;

		GuiWrapper.gameObject.SetActive (false);
        EndWrapper.gameObject.SetActive(true);
        Cursor.SetActive(true);
		(EndWrapper.GetComponent<EndGui> () as EndGui).Score = Score;

		float best = PlayerPrefs.GetFloat(ScoreKey, 0f);
		if (Score > best) {
			PlayerPrefs.SetFloat(ScoreKey, Score);
		}
	}

    public void RestartGame()
    {
        Debug.Log("RestartGame");

		EndWrapper.gameObject.SetActive (false);
		//StartWrapper.gameObject.SetActive (true);

		tubeManager.Restart ();

		bloodTimer = 2f * BloodLength;

		finished = false;
		started = false;

        StartGame();
	}

	public void  StartGame() {
        Debug.Log("StartGame");

		GuiWrapper.gameObject.SetActive (true);
		StartWrapper.gameObject.SetActive (false);
        Cursor.SetActive(false);

		resetParams ();
		gemController.Calibrate ();

		tubeManager.speed = Speed;		

		started = true;	
	}

	private void resetParams() {
		Lives = MaxLives + 1;
		removeLive ();
		Score = 0;
		tubeManager.speed = 0;
		safetyLastTime = 0f;
		bloodTimer = 2f * BloodLength;
	}

    internal void applyBonus()
    {
        Score += BonusScore;
        SetValue(ScoreText, ((int)Score).ToString());
    }
}
