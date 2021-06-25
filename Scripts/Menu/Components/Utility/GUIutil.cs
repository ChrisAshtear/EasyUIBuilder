using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Timers;
using System.ComponentModel;
using System;
using System.Linq;
using System.Xml.Linq;
using Object = UnityEngine.Object;

public static class ObjectToDictionaryHelper
{
    public static IDictionary<string, object> ToDictionary(this object source)
    {
        return source.ToDictionary<object>();
    }

    /*public static Dictionary<string,object> ToFullDictionary(this object source)
    {
        Dictionary<string, object> dict = (Dictionary<string, object>)ToDictionary(source);//not fully converted. 
        //dict["Keys"]
        //dict["Values"]

        Dictionary<string,object> fulldict = new Dictionary<string, object>();
        foreach()

    }*/

    public static IDictionary<string, T> ToDictionary<T>(this object source)
    {
        if (source == null)
            ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in System.ComponentModel.TypeDescriptor.GetProperties(source))
            AddPropertyToDictionary<T>(property, source, dictionary);
        return dictionary;
    }

    private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
    {
        object value = property.GetValue(source);
        if (IsOfType<T>(value))
            dictionary.Add(property.Name, (T)value);
    }

    private static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    private static void ThrowExceptionWhenSourceArgumentIsNull()
    {
        throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
    }
}

public class EditorFieldProps
{
    public string label;
    public SerializedProperty prop;
    public string fieldName;
}

public class EditorRowProps
{
    public Rect pos;
    public List<EditorFieldProps> props = new List<EditorFieldProps>();

    public EditorRowProps(Rect pos)
    {
        props = new List<EditorFieldProps>();
        this.pos = pos;
    }

    public void AddProp(SerializedProperty obj, string fieldName, string label="none")
    {
        props.Add(new EditorFieldProps { fieldName = fieldName, prop = obj, label = label });
    }

    public void Draw()
    {
        pos.x -= 100;
        int width = (int)pos.width / props.Count;
        foreach (EditorFieldProps p in props)
        {
            EditorPropField(ref pos, p, width);
        }
    }

    public void EditorPropField(ref Rect pos, EditorFieldProps field,int width)
    {
        //none check is to allow for a passthrough of an object that has its own ongui call where it draws its own fields.
        if(field.label != "none")
        {
            EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(field.label));
            pos.y += 16;
            pos.height -= 16;
        }
        
        pos.width = width;
        EditorGUI.PropertyField(pos, field.prop.FindPropertyRelative(field.fieldName), GUIContent.none);
        pos.x += width;
        if (field.label != "none")
        {
            pos.y -= 16;
            pos.height += 16;
        }
    }

    public void EditorPopupMenu(ref Rect pos, EditorFieldProps field, int width)
    {
        
    }

}

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
#if UNITY_EDITOR
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(label));
#endif
        position.y += verticalSpacing;
        position.height -= verticalSpacing;
        return position;
    }

    public static void SetTimer(Timer t, float delay, ElapsedEventHandler callbackFunc)
    {
        if(t != null)
        {
            t.Stop();
        }
        
        // Create a timer with a two second interval.
        t = new System.Timers.Timer(delay);//in ms
                                           // Hook up the Elapsed event for the timer. 
        t.Elapsed += callbackFunc;
        t.AutoReset = true;
        t.Enabled = true;
    }

    public static void clearChildren(Transform t,string exception="none",bool immediate=false)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform child in t)
        {
            if(child.name != exception)
            {
                objs.Add(child.gameObject);
            }
            
        }
        foreach (GameObject c in objs)
        {
            if (immediate)
            {
                Object.DestroyImmediate(c);
            }
            else
            {
                Object.Destroy(c);
            }
        }
    }

    public static void changeChildPrefab(Transform t, GameObject newPrefab)
    {
        clearChildren(t);
        UnityEngine.Object.Instantiate(newPrefab, t);
    }
}
