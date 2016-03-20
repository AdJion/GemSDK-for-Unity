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
        gem.CalibrateOrigin();
	}
		
	// Use this for initialization
	void Start () {
        GemManager.Instance.Connect();

        gem = GemManager.Instance.GetGem(0);
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
