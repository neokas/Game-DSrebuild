using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public ActorManger am;
    private Collider weaponColL;
    private Collider weaponColR;

    public GameObject WhL;
    public GameObject WhR;

    private void Start()
    {
        WhL = transform.DeepFind("WeaponHandleL").gameObject;
        WhR = transform.DeepFind("WeaponHandleR").gameObject;

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
