using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlatformText : MonoBehaviour {
	public string AndroidText;
	public string OtherText;

	
	// Use this for initialization
	void Start () {
		Text t = GetComponent<Text> () as Text;
		if (t == null)
			return;

		#if UNITY_ANDROID
		t.text = AndroidText;
		#else
		t.text = OtherText;
		#endif
	}
}
