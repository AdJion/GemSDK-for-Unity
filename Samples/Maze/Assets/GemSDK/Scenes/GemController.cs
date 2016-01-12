using UnityEngine;
using UnityEngine.UI;
using GemSDK.Unity;
using System.Collections;

public class GemController : MonoBehaviour
{
    public Text stateText;
    private IGem gem;
    
	// Use this for initialization
	void Start()
    {
        string[] gemsKnown = new string[0];
        
        if (Application.platform == RuntimePlatform.Android) 
        {
            //Get white list from the "Gem SDK Utility" app
            gemsKnown = GemSDKUtilityApp.GetWhiteList();
        }
        else if(Application.platform == RuntimePlatform.WindowsEditor ||
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

        GemManager.Instance.Connect();
    }

    void FixedUpdate()
    {
        if (gem != null)
        {
            if (Input.GetMouseButton(0))
                gem.Calibrate();

            transform.rotation = gem.Rotation;
            stateText.text = gem.State.ToString();
        }
    }

    void OnApplicationQuit()
    {
        GemManager.Instance.Disconnect();
    }

    //For Android to unbind Gem Service when the app is not in focus
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
