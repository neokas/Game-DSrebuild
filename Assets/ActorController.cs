﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 1.4f;


    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 movingVec;

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        anim.SetFloat("forward", pi.Dmag);
        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = pi.Dvec;
        }
        movingVec = pi.Dmag * model.transform.forward*walkSpeed;
    }


    private void FixedUpdate()
    {
        rigid.position += movingVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z);
    }
}
