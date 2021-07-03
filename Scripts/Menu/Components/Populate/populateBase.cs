using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity;
using UnityEngine.Events;
using UnityEditor;


// Custom serializable class
[Serializable]
public class BaseProps
{
    public string label = "";
    public Color32 color = Color.grey;
    public UnityEvent ev;

    public DataSource data;
    public string field;
    public int fieldIdx;
}


#if UNITY_EDITOR
/*
[CustomPropertyDrawer(typeof(DropDownProps))]
public class DropDownDrawer : PropertyDrawer
{
    
    private float xOffset = 0;
    private float yHeight = 32;
    private float expandedHeight = 50;//extra space for event control +/- buttons
    // Draw the property inside the given rect
    int _choiceIndex;

    List<string> allFields = new List<string>();
    string currentData = "null";
    // Have we loaded the prefs yet
    private static bool prefsLoaded = false;

    // The Preferences
    public static bool boolPreference = false;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty p = property.FindPropertyRelative("onSelect");
        
        if (p.intValue == 2)//custom prop
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ev1")) + expandedHeight ;
        }
        else
        {
            return yHeight;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        
        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
       // EditorGUIUtility.labelWidth = 14f;
       // position.width /= 2f;
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        xOffset = position.x;
        // Calculate rects
        var labelRect = getRect(50,position);
        var amountRect = getRect(100, position);
        var unitRect = getRect(100, position);
        var colorRect = getRect(100, position);
        var nameRect = getRect(100, position);
        var acRect = getRect(100, position);

        DataSource dataProp = (DataSource)property.FindPropertyRelative("data").;
        if(dataProp != null && dataProp.name != currentData)
        {
            allFields = dataProp.getFieldSimple();
            currentData = dataProp.name;
        }
        else if(dataProp == null)
        {
            allFields = new List<string>();
            currentData = "mull";
        }
        
        //dataProp.GetType()

        SerializedProperty userIndexProperty = property.FindPropertyRelative("fieldIdx");
        SerializedProperty fieldSelection = property.FindPropertyRelative("field");

        GUIutil.doPrefixLabel(ref labelRect, "Label");
        EditorGUI.PropertyField(labelRect, property.FindPropertyRelative("label"), GUIContent.none);

        EditorGUI.BeginChangeCheck();
        GUIutil.doPrefixLabel(ref unitRect, "Field");
        _choiceIndex = EditorGUI.Popup(unitRect, userIndexProperty.intValue, allFields.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            userIndexProperty.intValue = _choiceIndex;
            fieldSelection.stringValue = allFields[_choiceIndex];
        }



        //when changing fields, field is first set to null.
        //keep a list of choices, when field is null, reset choices. if list is null, get choices.
        SerializedProperty p = property.FindPropertyRelative("onSelect");

        if (p.intValue == 2)//custom prop
        {
            var eventRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            position.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ev1"));


            var lblRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            EditorGUI.PrefixLabel(lblRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("OnPress : "));

            eventRect.y += 16;
            EditorGUI.PropertyField(eventRect, property.FindPropertyRelative("ev1"), GUIContent.none);

        }


        if (p.intValue == 0 || p.intValue == 2)
        {
            GUIutil.doPrefixLabel(ref nameRect, "Display Object");
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("displayObj"), GUIContent.none);
        }

        GUIutil.doPrefixLabel(ref amountRect, "Data Source");
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("data"), GUIContent.none);

        GUIutil.doPrefixLabel(ref colorRect, "DropDown Type");
        EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("onSelect"), GUIContent.none);
        
        
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public Rect getRect(int size, Rect position)
    {
        Rect newR = new Rect(xOffset, position.y, size, yHeight);
        xOffset += size;
        return newR;
    }
}
*/
#endif

public class populateBase : MonoBehaviour
{

    public List<BaseProps> props;
    public GameObject layoutGroup; // where to place generated buttons
    public GameObject prefab;//object prefab
    public bool populateOnStart = true;

    void Reset()
    {
        props = new List<BaseProps>();
        layoutGroup = gameObject;
        //prefab = ProjectSettings.data.defaultButton;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (populateOnStart)
        {
            Populate();
        }

    }

    public virtual void Populate()
    {
    }

    public void Clear()
    {
        GUIutil.clearChildren(transform);
        props.Clear();
    }

    private void OnValidate()
    {
        foreach (Transform child in transform)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (child != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }
            };
        }
        Populate();
    }

}
