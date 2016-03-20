using UnityEngine;
using System.Collections;
using System;

using GemSDK.Unity;
using GemSDK.QuaternionUtils;

public class GameField : MonoBehaviour {
	public float anglesMultiplier;
	public float speed;
	public bool isContralable = false;

	private IGem gem;
    private TiltElevationController controller;

	// Use this for initialization
	void Start () {
        controller = new TiltElevationController();
        controller.Tilt.Configure(anglesMultiplier, 0, 0);
        controller.Elevation.Configure(anglesMultiplier, 0, 0);

        GemManager.Instance.Connect();

        //To get gem by number instead of address, on Android the Gem should be paired to Gem SDK Utility app
        gem = GemManager.Instance.GetGem(0);
	}

	public void Calibrate() {
        gem.CalibrateAzimuth ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (gem != null && isContralable) {
            Quaternion q = gem.Rotation;
            transform.rotation = controller.UpdateQuat(q);
		}
	}

    void OnApplicationPause(bool paused)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (paused)
            {
                GemManager.Instance.Disconnect();
            }
            else
            {
                GemManager.Instance.Connect();
            }
        }
    }

    void OnDestroy()
    {
        GemManager.Instance.Disconnect();
    }
}
