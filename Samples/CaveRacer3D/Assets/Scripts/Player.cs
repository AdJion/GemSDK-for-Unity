using System;
using UnityEngine;
using System.Collections;
using Tube;

public class Player : MonoBehaviour {

	private TubeManager tubeManager;
	private AudioSource audioSource;

    private Logic logic;

    //Sounds
	public AudioClip ouchSound;
    public AudioClip coinSound;

	public Transform light;

    public float playerBias;
	public float speed;
	private float playerPos = 3f * Mathf.PI / 2f;

	// Use this for initialization
	void Start () {
		tubeManager = GetComponentInParent<TubeManager>();
		audioSource = GetComponent<AudioSource> ();

		logic = (Logic)GameObject.Find ("Logic").GetComponent ("Logic");
	}
	
	void FixedUpdate () {
		playerPos += speed * Time.deltaTime;

		Vector3 dir = tubeManager.GetDirection ();
		Vector3 pos = tubeManager.GetPoint ();
		Vector3 localPos = new Vector3 (Mathf.Cos (playerPos), Mathf.Sin (playerPos));
		
		Vector3 downDir = Quaternion.LookRotation(dir) * localPos;

		transform.position = pos + downDir * tubeManager.radius * playerBias;
		transform.rotation = Quaternion.LookRotation (dir, - downDir );	

		light.position = pos - dir * 1.5f;
	}

	void OnTriggerEnter(Collider col) {

		if (col.gameObject.tag == "Obstacle") {
			Destroy(col.gameObject);

			audioSource.PlayOneShot(ouchSound);
			logic.removeLive ();
		}

        if (col.gameObject.tag == "Bonus")
        {
            Destroy(col.gameObject);

            audioSource.PlayOneShot(coinSound);
            logic.applyBonus();
        }
	}
}
