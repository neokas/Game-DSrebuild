using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelpers{

    //深度搜索子物体
    public static Transform DeepFind(this Transform parent,string targetName)
    {
        Transform tempTrans = null;
        foreach(Transform child in parent)
        {
            if (child.name == targetName)
            {
                return child;
            }
            else
            {
                tempTrans = DeepFind(child, targetName);
                if (tempTrans != null)
                {
                    return tempTrans;
                }
            }

        }
        return null;
    }
	


}
