using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
    public float RotationSpeed = 90f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    transform.rotation *= Quaternion.AngleAxis(RotationSpeed * Time.deltaTime, Vector3.forward);
	}
}
