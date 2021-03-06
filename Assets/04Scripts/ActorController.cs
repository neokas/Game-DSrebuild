﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public ActorManger am;
    public GameObject model;
    public CameraController CameraCon;
    public IUserInput pi;
    public float walkSpeed = 2.4f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 1.0f;
    public float jabVelocity = 3.0f;
    public float heightToRoll = 10.0f;

    [Header("===== Friction Settings =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;
    private CapsuleCollider col;

    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;

    private bool isCanAttack = true;
    private bool isCanJump = true;
    private bool isCanRoll = true;

    private bool islockPlanar = false;
    private bool trackDirection = false;

    //private float lerpTarget;

    private Vector3 deltaPos;


    public bool isLeftHandShield = true;

    private void Awake()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled == true)
            {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        am = GetComponent<ActorManger>();
    }

    private void Update()
    {
        anim.SetBool("run", pi.run);
        float targetRunMulti = (pi.run) ? 2.0f : 1.0f;
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.5f));

        if (pi.lockon)
        {
            CameraCon.LockUnLock();
        }
    }

    private void FixedUpdate()
    {
        //举盾
        if (isLeftHandShield && am.sm.isCanDefense)
        {
            anim.SetBool("defense", pi.defense);
            if (pi.defense)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);

            }
            else
            {
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
        }

        //跳跃
        if (pi.jump && isCanJump)
        {
            anim.SetTrigger("jump");
            isCanAttack = false;
        }

        //攻击
        if ((pi.rb || pi.lb)&& (CheckStateTag("ground")||CheckStateTag("attackL") || CheckStateTag("attackR")) && isCanAttack)
        {
            if (pi.rb)
            {
                anim.SetBool("leftHandAttack", false);
                anim.SetTrigger("attack");
            }
            else if(pi.lb && !isLeftHandShield)
            {
                anim.SetBool("leftHandAttack", true);
                anim.SetTrigger("attack");
            }

            isCanJump = false;
            isCanRoll = false;
        }

        if((pi.rt|| pi.lt)&& (CheckStateTag("ground") || CheckStateTag("attackL") || CheckStateTag("attackR")) && isCanAttack)
        {
            if (pi.rt)
            {

            }
            else
            {
                if(!isLeftHandShield)
                {

                }
                else
                {
                    anim.SetTrigger("counterBack");
                }
            }
        }

        if (pi.Dmag > 0.1f)
        {
            //model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.5f);
            model.transform.forward = pi.Dvec;
        }

        if (islockPlanar == false)
        {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
        }

        //位移
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;

        //翻滚
        if ((pi.roll && isCanRoll) || rigid.velocity.magnitude > 7f)
        {
            anim.SetTrigger("roll");
            isCanAttack = false;
        }

        //被击测试
        if(pi.rt)
        {
            anim.SetTrigger("hit");
        }
    }

    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        //判断当前的动画机是否属于某状态
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        //判断当前的动画机是否属于某状态
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    /// <summary>
    /// Message processing block
    /// </summary>

    public void OnJumpEnter()
    {
        pi.inputEnable = false;
        islockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
        trackDirection = true;
    }

    public void OnJumpExit()
    {
        //pi.inputEnable = true;
        //islockPlanar = false;
    }

    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        islockPlanar = false;
        col.material = frictionOne;

        isCanAttack = true;
        isCanJump = true;
        isCanRoll = true;
        trackDirection = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void  OnFallEnter()
    {
        pi.inputEnable = false;
        islockPlanar = true;
    }

    public void OnRollEnter()
    {
        pi.inputEnable = false;
        islockPlanar = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
        trackDirection = true;
    }

    public void OnRollExit()
    {
        //anim.SetFloat("forward", 1.5f);
    }

    public void OnJabEnter()
    {
        pi.inputEnable = false;
        islockPlanar = true;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;

    }

    public void OnUpdateRM(object _deltaPos)
    {//motion 控制动画位移
        if (CheckState("attack1hA")|| 
            CheckState("attack1hB")||
            CheckState("attack1hC"))
        {
            deltaPos += (0.2f *deltaPos+ 0.8f*(Vector3)_deltaPos)/1.0f;

        }
    }

    public void OnHitEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnHitExit()
    {
        pi.inputEnable = true;

    }

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnDieEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnStunnedEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBackEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }
}
