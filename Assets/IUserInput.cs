﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour {

    [Header("===== Output signals =====")]
    //方向，运动
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    //摄影机控制 
    public float Camera_up;
    public float Camera_right;

    //跑步
    public bool run;
    //跳跃
    public bool jump;
    protected bool lastJump;
    //攻击
    public bool attack;
    protected bool lastAttack;

    [Header("===== Others =====")]
    public bool inputEnable = true;

    protected float tragetDup;
    protected float tragetDright;
    protected float velocityDup;
    protected float velocityDright;

    //坐标转换 矩形→圆形
    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

}