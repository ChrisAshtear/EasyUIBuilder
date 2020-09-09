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
        _choiceIndex = EditorGUI.Popup(unitRect, userIndexProperty.intValue, allFields.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            userIndexProperty.intValue = _choiceIndex;
            fieldSelection.stringValue = allFields[_choiceIndex];
        }


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

    GenericDataHandler dataHandler;


    Text label;

    public string comparisonVal;

    displayObjDetails display;

    // Start is called before the first frame update
    //SHOW # items. adjust how big viewport is.
    //Font to use. Maybe not for this particular script but in the prefab.

    public void fill()
    {
        dataHandler = GetComponent<GenericDataHandler>();
        display = GetComponent<displayObjDetails>();
        foreach (DataFieldMap mapping in FieldMapping)
        {
            Text text = mapping.obj.GetComponent<Text>();
            if(text == null)
            {
                Image img = mapping.obj.GetComponent<Image>();
                int idx = int.Parse(mapping.data.getFieldFromItemID(index.ToString(), mapping.fieldName));
                Sprite[] sprites = Resources.LoadAll<Sprite>("GUIicons/" + spriteSheet.name);
                img.sprite = sprites[idx];
            }
            else
            {
                text.text = mapping.data.getFieldFromItemID(index.ToString(), mapping.fieldName);
            }

        }
        dataHandler.setData(data.getFieldsFromItemID(index));
        display.displayCode = data.displayCode;
        /*opdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        
        //need to pair id to value when selecting.
        Dictionary<string,string> opts= data.getFieldFromAllItemsKeyed(chosenField,true,true);
        //options.Add("");//add extra at bottom because dropdown hides it. 
        optionKeys = opts.Values.ToList();
        dropdown.AddOptions(opts.Keys.ToList());*/
    }

    private void Start()
    {
        initData();
    }

    public void initData()
    {


        if (data.isDataReady())
        {
            fill();
        }

        display = GetComponent<displayObjDetails>();
    }

    private void OnLevelWasLoaded(int level)
    {
       // data.onDataReady += populateButtons;
       if(data.isDataReady())
        {
            fill();
        }
    }
    


    public void selectedVal(int chosen)
    {
        //should store all keys instead of doing a search like this
        /*Dictionary<string, string> opts = data.getFieldFromAllItemsKeyed(chosenField);
        string chosenVal = dropdown.options[chosen].text;
        string key = opts[chosenVal];
        Debug.Log("Chose " + chosenVal + ": " + key);

        Dictionary<string, string> dat = data.getFieldsFromItemID(key);

        string output = "";
        foreach (KeyValuePair<string, string> entry in dat)
        {

            output += entry.Key + " : " + entry.Value + "\n";


        }
        detailsPane.text = output;
        */
    }

    public void displayValFields(int chosen)
    {
        GenericDataHandler datah = GetComponent<GenericDataHandler>();
       // listObjProps uiObject = displayObj;

        //string key = this.optionKeys[chosen];


        //Dictionary<string, string> dat = data.getFieldsFromItemID(key);

        //uiObject.resetVals();

        //parseFields(key);



    }

    public void parseFields(string id)
    {
        string[] lines = data.displayCode.Split('\n');
        Dictionary<string, string> comparisons = new Dictionary<string, string>();
        comparisons.Clear();
        string currentComparison = "";
        comparisons.Add("none", "");

        foreach (string line in lines)
        {
            if (line[0] == '#')
            {
                string[] headerFields = line.Split('|');
                headerFields[0] = headerFields[0].Substring(1);
                //displayObj.createHeader(int.Parse(data.getFieldFromItemID(id,headerFields[0])), data.getFieldFromItemID(id, headerFields[1]));
                continue;
            }
            if (line[0] == '=')
            {
                //displayObj.createSeperator();
                continue;
            }
            if (line[0] == '[')
            {
                currentComparison = line.Substring(1, line.Length - 2);
                comparisons.Add(currentComparison, "");
            }
            else if (currentComparison != "")
            {
                comparisons[currentComparison] += line + '\n';
            }
            else
            {
                comparisons["none"] += line + '\n';
            }
        }

        foreach (KeyValuePair<string, string> entry in comparisons)
        {
            if (entry.Value == "")
            {
                continue;
            }
            string value = entry.Value.Substring(0, entry.Value.Length - 1);
            if (entry.Key == data.getFieldFromItemID(id,comparisonVal) || entry.Key == "none")
            {
                string[] nlines = value.Split('\n');

                foreach (string line in nlines)
                {
                    string[] fields = line.Split('|');
                    if (fields.Length < 2)
                    {
                        //displayObj.createDescText(data.getFieldFromItemID(id,fields[0]));
                    }
                    else
                    {
                       // displayObj.createText(fields[0],data.getFieldFromItemID(id,fields[1]));
                    }

                }
            }
        }
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