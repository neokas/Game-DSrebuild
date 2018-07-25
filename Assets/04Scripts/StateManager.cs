using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    public float HPMax = 15.0f;
    public float HP = 15.0f;

    private void Start()
    {
        HP = HPMax;
    }

    public void EffectHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax); //范围限制

        if(HP>0)
        {
            am.Hit();
        }
        else if(HP==0)
        {
            am.Die();
        }
    }

}

