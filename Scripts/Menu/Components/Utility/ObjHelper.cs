using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Object = UnityEngine.Object;

public static class ObjHelper
{
    public static void ClearChildren(Transform t, string exception = "none")
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform child in t)
        {
            if (child.name != exception)
            {
                objs.Add(child.gameObject);
            }

        }
        foreach (GameObject c in objs)
        {
            if (Application.isEditor)
            {
                Object.DestroyImmediate(c);
            }
            else
            {
                Object.Destroy(c);
            }
        }
    }

}