﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public ActorManger am;
    private Collider weaponCol;

    public GameObject WhL;
    public GameObject WhR;

    private void Start()
    {
        weaponCol = WhR.GetComponentInChildren<Collider>();
    }

    public void WeaponEnable()
    {
        weaponCol.enabled = true;
    }

    public void WeaponDisable()
    {

        weaponCol.enabled = false;
    }

}
