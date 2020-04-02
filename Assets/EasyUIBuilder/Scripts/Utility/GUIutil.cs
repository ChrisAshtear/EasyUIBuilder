using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class GUIutil
{


    public static void matchObjSizeWithParent(GameObject child, GameObject parent)
    {
        RectTransform rectTransform = child.GetComponent<RectTransform>();

        rectTransform.position = parent.GetComponent<RectTransform>().position;
        rectTransform.anchorMin = new Vector2(0, 0);

        rectTransform.anchorMax = new Vector2(1, 1);

        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public static Rect doPrefixLabel(ref Rect position, string label, int verticalSpacing = 16)
    {
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(label));
        position.y += verticalSpacing;
        position.height -= verticalSpacing;
        return position;
    }

    public static void dropdownFromData(ref Rect position, DataSource source, string field)
    {
        /*

        EditorGUI.BeginChangeCheck();
        GUIutil.doPrefixLabel(ref unitRect, "Field");
        _choiceIndex = EditorGUI.Popup(unitRect, userIndexProperty.intValue, allFields.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            userIndexProperty.intValue = _choiceIndex;
            fieldSelection.stringValue = allFields[_choiceIndex];
        }*/
    }

    public static void clearChildren(Transform t)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform child in t)
        {
            objs.Add(child.gameObject);
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
