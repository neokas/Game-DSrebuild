using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    //在Inspector中绑定按键
    [Header("===== Key settings =====")]
    //方向控制 ， 左摇杆
    public string left_keyUp;
    public string left_keyDown;
    public string left_keyLeft;
    public string left_keyRight;

    //功能键
    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    //摄像机控制 ，右摇杆
    public string right_keyUp; //奔跑
    public string right_keyDown; //跳跃
    public string right_keyLeft;
    public string right_keyRight;

    //在Inspector观察值的变化
    [Header("===== Output signals =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    //摄影机控制 
    public float Camera_up;
    public float Camera_right;

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

        //摄影机控制
        Camera_up = (Input.GetKey(right_keyUp) ? 1.0f : 0) - (Input.GetKey(right_keyDown) ? 1.0f : 0);
        Camera_right = (Input.GetKey(right_keyRight) ? 1.0f : 0) - (Input.GetKey(right_keyLeft) ? 1.0f : 0);


        //奔跑及方向控制
        tragetDup = (Input.GetKey(left_keyUp) ? 1.0f : 0) - (Input.GetKey(left_keyDown) ? 1.0f : 0);
        tragetDright = (Input.GetKey(left_keyRight) ? 1.0f : 0) - (Input.GetKey(left_keyLeft) ? 1.0f : 0);

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

        //跳跃控制
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

    //坐标转换 矩形→圆形
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }




}
