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


        string[] gemsKnown = new string[0];

        if (Application.platform == RuntimePlatform.Android)
        {
            //Get white list from the "Gem SDK Utility" app
            gemsKnown = GemSDKUtilityApp.GetWhiteList();
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer)
        {
            //Get gems paired with Windows bluetooth settings
            gemsKnown = WindowsBleManager.GetPairedGems();
        }


        if (gemsKnown.Length > 0)
        {
            //Get the Gem instance for the curtain MAC address
            gem = GemManager.Instance.GetGem(gemsKnown[0]);
        }
	}

	public void Calibrate() {
        //gem.Calibrate ();
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
