using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    //在Inspector中绑定按键
    [Header("===== Key settings =====")]
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    //在Inspector观察值的变化
    [Header("===== Output signals =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;

    public bool run;

    [Header("===== Others =====")]
    public bool inputEnable = true;

    private float tragetDup;
    private float tragetDright;
    private float velocityDup;
    private float velocityDright;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        tragetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        tragetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if(inputEnable==false)
        {
            tragetDup = 0;
            tragetDright = 0;
        }
        

        Dup = Mathf.SmoothDamp(Dup, tragetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, tragetDright, ref velocityDright, 0.1f);
        Dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));
        Dvec = Dright * transform.right + Dup * transform.forward;
    }
}
