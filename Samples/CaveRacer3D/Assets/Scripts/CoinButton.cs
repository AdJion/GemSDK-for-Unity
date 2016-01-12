using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinButton : MonoBehaviour {
    public Camera camera;
    public float Speed = 90f;
    public float Limit = 180f;

    private float angle = 360f;
    
    public bool Pressed { get; private set; }

    private Collider collider;
    private Button button;

    void OnEnable()
    {
        angle = 360f;
    }

	void Start () {
        collider = GetComponent<Collider>();
        button = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hitInfo;
        bool hit = collider.Raycast(new Ray(camera.transform.position, camera.transform.forward), out hitInfo, float.MaxValue);

        float dir = hit ? -1f : 1f;


        Vector3 euler = transform.eulerAngles;

        //euler.x += dir * Speed * Time.deltaTime;
        //euler.x = Mathf.Clamp(euler.x + dir * Speed * Time.deltaTime, 0f, Limit);
        //Pressed = euler.x == Limit;

        angle = Mathf.Clamp(angle + dir * Speed * Time.deltaTime, Limit, 360f);
        Pressed = angle == Limit;

        if (Pressed)
        {
            button.onClick.Invoke();
            Debug.Log("Pressed");
        }
        //transform.eulerAngles = euler;

        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.left);
	}
}
