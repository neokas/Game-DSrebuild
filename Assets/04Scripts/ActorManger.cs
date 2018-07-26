using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManger : MonoBehaviour {

    public ActorController ac;
    [Header("=== Auto Generate if Null ===")]
    public BattleManger bm;
    public WeaponManager wm;
    public StateManager sm;

    private void Awake()
    {
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = transform.Find("sensor").gameObject;

        bm = Bind<BattleManger>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        
    }

    private T Bind<T>(GameObject go) where T : IActorManagerInterface
    {
        T tempInstance;
        tempInstance = go.GetComponent<T>();
        if(tempInstance == null)
        {
            tempInstance = go.AddComponent<T>();
        }
        tempInstance.am = this;
        
        return tempInstance;
    }

    public void SetIsCounterBack(bool value)
    {
        sm.isCounterBackEnable = value;
    }

    public void TryDoDamage(WeaponControllor targetWc)
    {
        if(sm.isCounterBackSuccess)
        {
            targetWc.wm.am.Stunned();
        }
        else if(sm.isCounterBackFailure)
        {
            HitOrDie(false);
        }
        else if(sm.isImmortal)
        {
            //无敌 do nothing
        }
        else if(sm.isDefense || sm.isBlocked)
        {
            //blocked
            Blocked();
        }
        else
        {
            HitOrDie(true);
        }
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    public void HitOrDie(bool doHitAnimation)
    {
        if (sm.HP <= 0)
        {
            //already dead.
        }
        else
        {
            //sm.EffectHP(-5);
            if (sm.HP > 0)
            {
                if (doHitAnimation)
                {
                    Hit();
                }
            }
            else
            {
                Die();
            }
        }
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnable = false;
        if(ac.CameraCon.lockState==true)
        {
            ac.CameraCon.LockUnLock();
        }
        ac.CameraCon.enabled = false;
    }

}
