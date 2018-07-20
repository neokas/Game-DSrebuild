using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{
    [Header("===== Joystick Settings =====")]
    //左摇杆
    private string axisLX = "axisX";
    private string axisLY = "axisY";
    //右摇杆
    private string axisRX = "axis4";
    private string axisRY = "axis5";
    //A:0 B:1 X:2 Y:3
    private string btnA = "btn0";
    private string btnB = "btn1";
    private string btnX = "btn2";
    private string btnY = "btn3";
    //LB:4 RB:5
    private string btnLB = "btn4";
    private string btnRB = "btn5";
    //LT RT (是以axis方式接受的，使用需要转换为bool型)
    private string btnLT = "axis9";
    private string btnRT = "axis10";
    //back:6 menu:7
    private string btn6 = "btn6";
    private string btn7 = "btn7";
    //L3:8 R3:9  Left/Right Stick Click
    private string btnLSC = "btn8";
    private string btnRSC = "btn9";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonX = new MyButton();
    public MyButton buttonY = new MyButton();
    public MyButton buttonLB = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonRB = new MyButton();
    public MyButton buttonRT = new MyButton();
    public MyButton buttonLSC = new MyButton();
    public MyButton buttonRSC = new MyButton();
    
    
    //[Header("===== Output signals =====")]
    ////方向，运动
    //public float Dup;
    //public float Dright;
    //public float Dmag;
    //public Vector3 Dvec;
    ////摄影机控制 
    //public float Camera_up;
    //public float Camera_right;

    ////跑步
    //public bool run;
    ////跳跃
    //public bool jump;
    //public bool lastJump;
    ////攻击
    //public bool attack;
    //public bool lastAttack;

    //[Header("===== Others =====")]
    //public bool inputEnable = true;

    //private float tragetDup;
    //private float tragetDright;
    //private float velocityDup;
    //private float velocityDright;


    // Use this for initialization
    void Start () {
        lockon = false;
	}
	
	// Update is called once per frame
	void Update () {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonX.Tick(Input.GetButton(btnX));
        buttonY.Tick(Input.GetButton(btnY));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonLT.Tick((Input.GetAxis(btnLT) > 0 ? true : false));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonRT.Tick((Input.GetAxis(btnRT) > 0 ? true : false));
        buttonLSC.Tick(Input.GetButton(btnLSC));
        buttonRSC.Tick(Input.GetButton(btnRSC));

        //摄影机控制
        Camera_up = Input.GetAxis(axisRY);
        Camera_right = Input.GetAxis(axisRX);

        tragetDup = Input.GetAxis(axisLY);
        tragetDright = Input.GetAxis(axisLX);

        if (inputEnable == false)
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

        run = (buttonA.isPressing && !buttonA.isDelaying) || buttonA.isExtending;   //跑
        jump = buttonA.isExtending && buttonA.onPressed;//跳跃

        roll = buttonA.onReleased && buttonA.isDelaying; //翻滚
        defense = buttonLB.isPressing;//防御
        attack = buttonX.onPressed;//攻击

        lockon = buttonRSC.onPressed; //锁定
    }

    ////坐标转换 矩形→圆形
    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

    //    return output;
    //}
}
