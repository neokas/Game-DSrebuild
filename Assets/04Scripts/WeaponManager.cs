using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    //public ActorManger am;
    public Collider weaponColL;
    public Collider weaponColR;

    public GameObject WhL;
    public GameObject WhR;

    private void Start()
    {
        WhL = transform.DeepFind("weaponHandleL").gameObject;
        WhR = transform.DeepFind("weaponHandleR").gameObject;

        weaponColL = WhL.transform.GetComponentInChildren<Collider>();
        weaponColR = WhR.transform.GetComponentInChildren<Collider>();
    }

    public void WeaponEnable()
    {
        if (am.ac.CheckStateTag("attackL"))
        {
            weaponColL.enabled = true;
        }

        if (am.ac.CheckStateTag("attackR"))
        {
            weaponColR.enabled = true;
        }

    }

    public void WeaponDisable()
    {
        weaponColL.enabled = false;
        weaponColR.enabled = false;
    }

}
