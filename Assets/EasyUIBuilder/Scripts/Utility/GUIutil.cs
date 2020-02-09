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
}
