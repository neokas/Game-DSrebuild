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

    public bool run; //跑步
    public bool jump; 
    public bool lastJump;

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

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(keyA);

        bool newJump = Input.GetKey(keyB);
        //jump = newJump;
        if (newJump != lastJump && newJump == true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;

    }

    //坐标转换
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }




}
