using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    public float HPMax = 15.0f;
    public float HP = 15.0f;

    [Header("1st order state flags")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;

    [Header("2nd order state flags")]
    public bool isCanDefense;
    public bool isImmortal; //无敌状态

    private void Start()
    {
        HP = HPMax;
    }

    private void Update()
    {
        isGround = am.ac.CheckStateTag("ground");
        isJump = am.ac.CheckState("jump");
        isFall = am.ac.CheckState("fall");
        isRoll = am.ac.CheckState("roll");
        isJab = am.ac.CheckState("jab");
        isAttack = am.ac.CheckStateTag("attackR") || am.ac.CheckStateTag("attackL");
        isHit = am.ac.CheckState("hit");
        isDie = am.ac.CheckState("die");
        isBlocked = am.ac.CheckState("blocked","defense");
        

        isCanDefense = isGround || isBlocked;
        isDefense = isCanDefense && am.ac.CheckState("defense", "defense");
        isImmortal = isRoll || isJab;
    }

    public void EffectHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax); //范围限制

    }

}

