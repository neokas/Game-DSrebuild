﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

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
    private bool isCanGuard = true;
    private bool isCanRoll = true;
    private bool islockPlanar = false;

    private float lerpTarget;

    private Vector3 deltaPos;

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
    }

    private void Update()
    {
        if(pi.lockon)
        {
            CameraCon.LockUnLock();
        }
    }

    private void FixedUpdate()
    {

        //float targetRunMulti = (pi.run) ? 2.0f : 1.0f;
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), 1.0f, 0.5f));

        anim.SetBool("defense", pi.defense);
        anim.SetBool("run", pi.run);

        if (pi.jump && isCanJump)
        {
            anim.SetTrigger("jump");
            isCanAttack = false;
            isCanGuard = false;
        }

        if(pi.attack == true && CheckState("ground") && isCanAttack)
        {
            anim.SetTrigger("attack");

            isCanJump = false;
            isCanRoll = false;
        }

        if (CameraCon.LockState == false)
        {
            if (pi.Dmag > 0.1f)
            {
                //model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.5f);
                model.transform.forward = pi.Dvec;
            }

            if (islockPlanar == false)
            {
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }
        else
        {
            model.transform.forward = transform.forward;
            if (islockPlanar == false)
            {
                planarVec = pi.Dvec * ((pi.run) ? runMultiplier : 1.0f);
            }
        }

        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z)+thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;

        if((pi.roll && isCanRoll) || rigid.velocity.magnitude>7f)
        {
            anim.SetTrigger("roll");
            isCanAttack = false;
            isCanGuard = false;
        }
    }

    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        //判断当前的动画机是否属于某状态
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    /// <summary>
    /// Message processing block
    /// </summary>

    public void OnJumpEnter()
    {
        pi.inputEnable = false;
        islockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
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
        isCanGuard = true;
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

        lerpTarget = 1.0f;
    }


    public void OnAttack1hAUpdate()
    {//单手攻击A
        //前移
        //thrustVec = model.transform.forward * anim.GetFloat("attack1hVelocity");

        //改attack layer的权重 0→1
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f));
    }

    public void OnAttack1hBUpdate()
    {//单手攻击B
        //前移
        //thrustVec = model.transform.forward * anim.GetFloat("attack1hVelocity");
    }

    public void OnAttack1hCUpdate()
    {//单手攻击C
        //前移
        //thrustVec = model.transform.forward * anim.GetFloat("attack1hVelocity");
    }

    public void OnAttackIdleEnter()
    {
        pi.inputEnable = true;
        lerpTarget = 0;

        isCanJump = true;
        isCanRoll = true;
    }

    public void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f));
    }

    public void OnUpdateRM(object _deltaPos)
    {//motion 控制动画位移
        if (CheckState("attack1hA", "attack")|| 
            CheckState("attack1hB", "attack")||
            CheckState("attack1hC", "attack"))
        {
            deltaPos += (0.2f *deltaPos+ 0.8f*(Vector3)_deltaPos)/1.0f;
        }
    }
}
