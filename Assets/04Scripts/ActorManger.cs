using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManger : MonoBehaviour {

    public ActorController ac;
    public BattleManger bm;
    public WeaponManager wm;

    private void Awake()
    {
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = transform.Find("sensor").gameObject;
        bm = sensor.GetComponent<BattleManger>();
        if(bm==null)
        {
            bm = sensor.AddComponent<BattleManger>();
        }
        bm.am = this;

        wm = model.GetComponent<WeaponManager>();
        if(wm ==null)
        {
            wm = model.AddComponent<WeaponManager>();
        }
        wm.am = this;
    }

    public void DoDamage()
    {
        ac.IssueTrigger("hit");
    }
}
