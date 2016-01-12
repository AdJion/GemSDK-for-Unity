using UnityEngine;
using System.Collections;

public class CoinGenerator : MonoBehaviour {
    public int MinLineLength = 3;
    public int MaxLineLength = 8;
    
    public int MinSpaceLength = 3;
    public int MaxSpaceLength = 20;

    public float MinDirectionOffset = -0.1f;
    public float MaxDirectionOffset = 0.1f;

    public int Odd;

    private float direction = 0f;
    private bool coinMode = true;
    private int counter = 0;
    private int count = 0;

	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public bool isCoin { get { return coinMode; } }
    public float Angle { get; private set; }

    public bool Next()
    {
        if (counter >= count)
        {
            switchMode();
        }

        if (coinMode)
        {
            Angle += direction;            
        }

        counter++;

        return coinMode;
    }

    public void Reset()
    {
        coinMode = true;
        switchMode();
    }

    private void switchMode() {
        coinMode = !coinMode;

        if (coinMode)
        {
            count = Random.Range(MinLineLength, MaxLineLength);
            direction = Random.Range(MinDirectionOffset, MaxDirectionOffset);
            Angle = Random.Range(0, Mathf.PI * 2f);
        }
        else
        {
            count = Random.Range(MinSpaceLength, MaxSpaceLength);            
        }

        counter = 0;
    }
}
