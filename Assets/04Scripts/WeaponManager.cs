using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    //public ActorManger am;
    public Collider weaponColL;
    public Collider weaponColR;

    public GameObject whL;
    public GameObject whR;

    public WeaponControllor wcL;
    public WeaponControllor wcR;

    private void Start()
    {
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;

        wcL = BindWeaponController(whL);
        wcR = BindWeaponController(whR);

        weaponColL = whL.transform.GetComponentInChildren<Collider>();
        weaponColR = whR.transform.GetComponentInChildren<Collider>();
    }

    public WeaponControllor BindWeaponController(GameObject targetObj)
    {
        WeaponControllor tempWc;
        tempWc = targetObj.GetComponent<WeaponControllor>();
        if(tempWc == null)
        {
            tempWc = targetObj.AddComponent<WeaponControllor>();
        }
        tempWc.wm = this;

        return tempWc;
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

    public void CounterBackEnable()
    {
        am.SetIsCounterBack(true);
    }

    public void CounterBackDisable()
    {
        am.SetIsCounterBack(false);
    }

}
