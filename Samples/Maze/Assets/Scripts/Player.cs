using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public bool RealGame;
	public Logic gameLogic;

	private AudioSource loopedAudioPlayer;
	private AudioSource oneShotAudioPlayer;

    private Rigidbody body;
    private Vector3 initialPos;

	// Use this for initialization
	void Start () {
		AudioSource[] aSources = GetComponents<AudioSource> ();
		loopedAudioPlayer = aSources [0];
		oneShotAudioPlayer = aSources [1];

        body = GetComponent<Rigidbody>();
        
        initialPos = body.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		float vel = body.velocity.magnitude;
		loopedAudioPlayer.volume = Mathf.Lerp(0, 1, vel * 0.5f);	
	}

	void OnTriggerEnter(Collider collision) {
		if (!RealGame) return;

		string tag = collision.gameObject.tag;

		if (tag == "Endgame") {
			gameLogic.EndGame(false);
		}

		if (tag == "Finish") {
			gameLogic.EndGame(true);
		}
	}

	void OnCollisionEnter(Collision col) {
		string tag = col.gameObject.tag;

		if (tag == "Ground") {
			loopedAudioPlayer.Play();		
		}
		
		if (tag == "Wall") {
			oneShotAudioPlayer.volume = Mathf.Lerp(0, 1f, body.velocity.magnitude * 0.3f);
			oneShotAudioPlayer.Play();
		}
	}

	void OnCollisionExit(Collision col) {
		string tag = col.gameObject.tag;
	
		if (tag == "Ground") {
			loopedAudioPlayer.Pause();
		}
	}

    public void Reset()
    {
        body.position = initialPos;
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
    }
}
