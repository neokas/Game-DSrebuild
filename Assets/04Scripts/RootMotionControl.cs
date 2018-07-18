﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour {

    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM",(object)anim.deltaPosition);
    }

}
