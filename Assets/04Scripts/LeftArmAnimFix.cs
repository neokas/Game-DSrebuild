using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour {

    private Animator anim;

    public Vector3 a;
    public Vector3 OnGuard;
    public Vector3 UnGuard;

    public bool isOnGuard = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if(anim.GetBool("isGround"))
        {
            if (anim.GetBool("defense") == false)
            {
                if (isOnGuard)
                {
                    a = OnGuard;
                }
                else
                {
                    a = UnGuard;
                }

                //Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                //leftLowerArm.localEulerAngles += a;
                //anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
            }
        }
    }

    public void ChangeGuardState()
    {
        if (isOnGuard)
        {
            isOnGuard = false;
        }
        else
        {
            isOnGuard = true;
        }
    }

}
