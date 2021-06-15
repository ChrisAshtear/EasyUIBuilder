using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System.Reflection;
using System;
/// <summary>
/// This populates the contents of a dropbox with a number of elements in a data collection.
/// </summary>
[Serializable]
public class DataFieldMap
{
    public DataSource data;
    public string fieldName;
    public GameObject obj;
    public int fieldIdx;
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DataFieldMap))]
public class DataFieldDrawer : PropertyDrawer
{

    private float xOffset = 0;
    private float yHeight = 32;
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
         return yHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);


        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var amountRect = getRect(100, position);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        xOffset = position.x;
        // Calculate rects
        var labelRect = getRect(50, position);
        var unitRect = getRect(100, position);

        DataSource dataProp = (DataSource)property.FindPropertyRelative("data").objectReferenceValue;
        if (dataProp != null && dataProp.name != currentData)
        {
            allFields = dataProp.getFieldSimple();
            currentData = dataProp.name;
        }
        else if (dataProp == null)
        {
            allFields = new List<string>();
            currentData = "mull";
        }

        //dataProp.GetType()

        SerializedProperty userIndexProperty = property.FindPropertyRelative("fieldIdx");
        SerializedProperty fieldSelection = property.FindPropertyRelative("fieldName");

        GUIutil.doPrefixLabel(ref labelRect, "Label");
        EditorGUI.PropertyField(labelRect, property.FindPropertyRelative("obj"), GUIContent.none);

        EditorGUI.BeginChangeCheck();
        GUIutil.doPrefixLabel(ref unitRect, "FieldName");
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("fieldName"), GUIContent.none);


        GUIutil.doPrefixLabel(ref amountRect, "Data Source");
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("data"), GUIContent.none);
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

public class FillFromSource : MonoBehaviour
{

    public List<DataFieldMap> FieldMapping;
    public DataSource data;
    public string index;
    public Texture2D spriteSheet;
    public bool showLabel = true;

    public string tableName;

    GenericDataHandler dataHandler;


    Text label;

    public string comparisonVal;

    displayObjDetails display;

    // Start is called before the first frame update
    //SHOW # items. adjust how big viewport is.
    //Font to use. Maybe not for this particular script but in the prefab.

    public void fill(string displayCode="")
    {
        
        
        dataHandler = GetComponent<GenericDataHandler>();
        display = GetComponent<displayObjDetails>();
        display.source = data;
        display.initCallback();
        foreach (DataFieldMap mapping in FieldMapping)
        {
            if (mapping.data == null)
            {
                mapping.data = data;
            }
            Text text = mapping.obj.GetComponent<Text>();
            TMPro.TextMeshProUGUI tm = mapping.obj.GetComponent<TMPro.TextMeshProUGUI>();
            if (text == null && tm == null)
            {
                Image img = mapping.obj.GetComponent<Image>();
                object field = mapping.data.getFieldObjFromItemID(index.ToString(), mapping.fieldName);
                if(field.GetType().Equals(typeof(Sprite)))
                {
                    img.sprite = (Sprite)mapping.data.getFieldObjFromItemID(index.ToString(), mapping.fieldName);
                }
            }
            else
            {
                if(tm == null)
                {
                    text.text = mapping.data.getFieldFromItemID(index.ToString(), mapping.fieldName);
                }
                else
                {
                    tm.text = mapping.data.getFieldFromItemID(index.ToString(), mapping.fieldName);
                }
            }

        }
        if(data != null)
        {
            dataHandler.setData(data.getFieldObjsFromItemID(index));
            dataHandler.source = data;
        }
        
        if(display == null)
        {
            return;
        }


        if(displayCode != "")
        {
            display.displayCode = displayCode;
        }
        else
        {
            display.displayCode = data.displayCode;
        }
        if(display.uiObject == null)
        {
            //MenuManager.ins.detailsCard.GetComponentInChildren<listObjProps>().iconSheet = spriteSheet;
            
        }
        else
        {
            display.uiObject.iconSheet = spriteSheet;
        }

    }

    private void OnEnable()
    {
        initData();
    }
    private void Start()
    {
    }

    public void initData()
    {


        if (data != null && data.isDataReady() && index !=  "")
        {
            fill();
        }

        display = GetComponent<displayObjDetails>();
    }

    private void OnLevelWasLoaded(int level)
    {
       // data.onDataReady += populateButtons;
       if(data.isDataReady() && index != "")
        {
            fill();
        }
    }
    


    public void selectedVal(int chosen)
    {
        //should store all keys instead of doing a search like this
    }

    public void displayValFields(int chosen)
    {
        GenericDataHandler datah = GetComponent<GenericDataHandler>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateUI()
    {
        //getDataForUI.ins.dropdownSelected(this.name, dropdown.value.ToString());
    }

}

