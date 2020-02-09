using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity;
using UnityEngine.Events;
using UnityEditor;

public enum dropdownFunction { showDetails, Form };

// Custom serializable class
[Serializable]
public class DropDownProps
{
    public string name = "Name";
    public dropdownFunction onSelect = dropdownFunction.Form;
    public Color32 color = Color.grey;
    public UnityEvent ev;
    public DataSource data;
    public string field;
    public int fieldIdx;
}

#if UNITY_EDITOR
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

    // Add preferences section named "My Preferences" to the Preferences window
    [PreferenceItem("My Preferences")]
    public static void PreferencesGUI()
    {
        // Load the preferences
        if (!prefsLoaded)
        {
            boolPreference = EditorPrefs.GetBool("BoolPreferenceKey", false);
            prefsLoaded = true;
        }

        // Preferences GUI
        boolPreference = EditorGUILayout.Toggle("Bool Preference", boolPreference);

        // Save the preferences
        if (GUI.changed)
            EditorPrefs.SetBool("BoolPreferenceKey", boolPreference);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        /*SerializedProperty p = property.FindPropertyRelative("onPress");
        if (p.intValue == 4)//custom prop
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ev")) + expandedHeight ;
        }
        else
        {*/
            return yHeight;
        //}
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
        var amountRect = getRect(100,position);
        var unitRect = getRect(100, position);
        var colorRect = getRect(100, position);
        var nameRect = getRect(200, position);
        var acRect = getRect(100, position);

        DataSource dataProp = (DataSource)property.FindPropertyRelative("data").objectReferenceValue;
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

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        //SerializedProperty p = property.FindPropertyRelative("onPress");
        /*if(p.intValue == 4)//custom prop
        {
            var eventRect = new Rect(position.x, position.y+yHeight, position.width, position.height-yHeight);
            position.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ev"));
            

            var labelRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("OnPress : "));

            eventRect.y += 16;
            EditorGUI.PropertyField(eventRect, property.FindPropertyRelative("ev"), GUIContent.none);

        }
        else if(p.intValue == 0 || p.intValue == 3 || p.intValue == 6)
        {
            EditorGUI.PrefixLabel(nameRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Option Field"));
            nameRect.y += 16;
            nameRect.height -= 16;
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("field"), GUIContent.none);
        }*/

        

        

        /*EditorGUI.PrefixLabel(acRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Custom Sound"));
        acRect.y += 16;
        acRect.height -= 16; 
        EditorGUI.PropertyField(acRect, property.FindPropertyRelative("AC"), GUIContent.none);
        */

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
#endif

public class populateDropDowns : MonoBehaviour
{

    public List<DropDownProps> props;
    public GameObject layoutGroup; // where to place generated buttons
    public GameObject prefab;//button prefab

    void Reset()
    {
        props = new List<DropDownProps>();
        props.Add(new DropDownProps());
        props[0].name = "DropDown";
        props[0].color = Color.grey;
        layoutGroup = gameObject;
        projectHandler.init();
        prefab = projectHandler.pData.defaultButton;
    }
    // Start is called before the first frame update
    void Start()
    {
        bool selected = false;
        foreach(DropDownProps b in props)
        {

            GameObject obj = Instantiate(prefab, layoutGroup.transform);
            obj.transform.parent = layoutGroup.transform;
            obj.name = b.name;
            Dropdown dropdown = obj.GetComponent<Dropdown>();
            if(!selected)
            {
                dropdown.Select();
                selected = true;
                obj.AddComponent<UIselectOnEnable>();
            }
            
            obj.transform.Find("Label").GetComponent<Text>().text = b.name;
            //obj.transform.Find("ButtonCenter").GetComponent<Image>().color = b.color;
            switch(b.onSelect)
            {
                case dropdownFunction.Form:
                    //dropdown.onValueChanged.AddListener(() => MenuManager.ins.changeMenu(b.argument,b.AC));
                    break;

                case dropdownFunction.showDetails:
                    //dropdown.onValueChanged.AddListener(() => MenuManager.ins.goBack());
                    break;


            }

            FillDropboxFromSource filldrop = obj.GetComponent<FillDropboxFromSource>();
            filldrop.data = b.data;
            filldrop.chosenField = b.field;
            filldrop.initData();

            /*if (b.AC != null)
            {
                obj.GetComponent<AudioSource>().clip = b.AC;
                button.onClick.AddListener(() => obj.GetComponent<AudioSource>().Play());

            }*/

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
