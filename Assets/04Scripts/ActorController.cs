﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 1.4f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 3.0f;


    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;

    public bool islockPlanar = false;

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float targetRunMulti = (pi.run) ? 2.0f : 1.0f;
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.5f));
        if (pi.jump == true)
        {
            anim.SetTrigger("jump");
        }

        if (pi.Dmag > 0.1f)
        {
            //model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.5f);
            model.transform.forward = pi.Dvec;
        }

        if (islockPlanar==false)
        {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
        }
    }

    private void FixedUpdate()
    {
        //rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z)+thrustVec;
        thrustVec = Vector3.zero;
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
    }

}
