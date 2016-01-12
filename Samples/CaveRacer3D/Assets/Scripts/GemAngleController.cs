using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GemSDK.QuaternionUtils;
using GemSDK.Unity;

public class GemAngleController : MonoBehaviour {
    //Gem connection state text
	public Text stateText;

    //Gem instance
	private	IGem gem;

    //Tilt and elvecation holder
	private Vector2 angles = Vector2.zero;
    
	public float Tilt {
		get {
			return angles.x;
		}
	}

	public float Elevation {
		get {
			return angles.y;
		}
	}
	
	public void Calibrate() {
        //Use current orientation as an origin
        gem.Calibrate();
	}
		
	// Use this for initialization
	void Start () {
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
	
    void FixedUpdate()
    {
        if (gem != null)
        {
            //Calculate azimuth and elevation from current gem orientation
            var azEl = AzimuthElevationConverter.Convert(gem.Rotation);

            //Calculate tilt and elevation from current gem orientation
            var tiEl = TiltElevationConverter.Convert(gem.Rotation);

            angles = new Vector2(azEl.x, tiEl.x);

            //Indicate gem connection state
            stateText.text = "Gem: " + gem.State.ToString();
        }
    }

    void OnApplicationQuit()
    {
        GemManager.Instance.Disconnect();
    }

    //For Android to unbind Gem Service when application is not in focus
    void OnApplicationPause(bool paused)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (paused)
                GemManager.Instance.Disconnect();
            else
                GemManager.Instance.Connect();
        }
    }
}
