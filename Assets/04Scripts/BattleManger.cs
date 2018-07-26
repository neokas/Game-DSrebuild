using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManger : IActorManagerInterface
{

    //public ActorManger am;

    private CapsuleCollider defCol;

    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up * 1.0f;
        defCol.height = 2.0f;
        defCol.radius = 0.5f;
        defCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider col)
    {    
        if(col.tag == "Weapon" && col.name == "Cylinder")
        {
            am.TryDoDamage();
        }
    }

}
