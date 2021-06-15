using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public enum listFunction { showDetails, Form,CustomPlusDetails,CustomDataLib };
public enum filterType { text,number,boolean}
public enum filterMatchType { matches,notmatching,greaterOrEqual,LessThanOrEqual}

[Serializable]
public class SourceFilter
{
    public string filterVar;//filter variable to be searched
    public string filterValue;//filter text - comma separated values.
    public filterType type;
    public filterMatchType match;

    public bool MatchesFilter(string value)
    {
        switch(type)
        {
            case filterType.number:

                int.TryParse(value, out int testingNum);
                int.TryParse(filterValue, out int filterNum);
                switch(match)
                {
                    case filterMatchType.greaterOrEqual:
                        if(testingNum >= filterNum){ return true; }
                        else { return false; }
                        break;
                    case filterMatchType.LessThanOrEqual:
                        if (testingNum <= filterNum) { return true; }
                        else { return false; }
                        break;
                }
                break;

            case filterType.text:
                bool doesMatch = filterValue.Contains(value);
                switch (match)
                {
                    case filterMatchType.matches:
                        return doesMatch;
                        break;
                    case filterMatchType.notmatching:
                        return !doesMatch;
                        break;
                }
                break;
            case filterType.boolean:
                bool matches = value.Contains("True");
                return matches;
        }
        return false;
    }
}
/*
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SourceFilter))]
public class FilterDrawer : PropertyDrawer
{
    private float xOffset = 0;
    private float yHeight = 20;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        xOffset = position.x - 100;

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var varRect = getRect(100, position);
        var valueRect = getRect(100, position);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(varRect, property.FindPropertyRelative("filterVar"),GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("filterValue"), GUIContent.none);

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
*/
public interface I_ItemMenu
{
    void SetSelected(GameObject obj);
    GameObject GetSelected();
}

[System.Serializable]
public class OnSelectListEvent : UnityEvent<selectListItem>
{
}


public class selectListItem
{
    public FillFromSource fill;//reference to selected obj
    public string index;//index of selected item
}

// Custom serializable class
[Serializable]
public class ListProps
{
    public string label = "";
    public listFunction onSelect = listFunction.Form;
    public Color32 color = Color.grey;
    public UnityEvent ev;
    public UnityEvent<IDataLibrary> evData;
    public OnSelectListEvent ev1;

    public string tableName;
    [HideInInspector]
    public DataSource data { get { return dataS.db.getTable(dataS.tableName); } }
    public SourceProps dataS;
    public string field;
    public listObjProps displayObj;
    public int fieldIdx;
    public int tableIdx;
}
[System.Serializable]
public class SourceProps
{
    public DatabaseSource db;
    public string tableName;
    public string ID;
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SourceProps))]
public class SourceDrawer : PropertyDrawer
{

    private float xOffset = 0;
    private float yHeight = 32;
    private float expandedHeight = 50;//extra space for event control +/- buttons
    // Draw the property inside the given rect
    int _choiceIndex;
    int _tableChoiceIndex;

    List<string> allFields = new List<string>();
    List<string> allTables = new List<string>();
    string currentData = "null";
    public int index = 0;
    public string[] options = new string[] { "Cube", "Sphere", "Plane" };

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
            return yHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        xOffset = position.x;

        SerializedProperty dataSource = property.FindPropertyRelative("db");
        DatabaseSource obj = (DatabaseSource)dataSource.objectReferenceValue;
        
        SerializedProperty table = property.FindPropertyRelative("tableName");
        // Calculate rects
        var labelRect = getRect(100, position);
        var tableRect = getRect(100, position);

        GUIutil.doPrefixLabel(ref labelRect, "Source");
        labelRect.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("db"));
        EditorGUI.PropertyField(labelRect, property.FindPropertyRelative("db"), GUIContent.none);
        GUIutil.doPrefixLabel(ref tableRect, "Table");
        
        if(obj!=null)
        {
            string[] tables = obj.getTables().ToArray();
            index = EditorGUI.Popup(tableRect, index, tables.ToArray());
            if (index >= tables.Length)
            {
                index = 0;
            }
            property.FindPropertyRelative("tableName").stringValue = tables[index];
            property.FindPropertyRelative("ID").stringValue = obj.primaryKey;
        }
        
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


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ListProps))]
public class ListDrawer : PropertyDrawer
{
    
    private float xOffset = 0;
    private float yHeight = 32;
    private float expandedHeight = 50;//extra space for event control +/- buttons
    // Draw the property inside the given rect
    int _choiceIndex;
    int _tableChoiceIndex;

    List<string> allFields = new List<string>();
    List<string> allTables = new List<string>();
    string currentData = "null";

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty p = property.FindPropertyRelative("onSelect");
        
        if (p.intValue == 2||p.intValue==3)//custom prop
        {
            string evName = "ev1";
            if (p.intValue == 3) { evName = "evData"; }
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(evName)) + expandedHeight ;
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
        var labelRect = getRect(150,position);
        var amountRect = getRect(100, position);
        var unitRect = getRect(100, position);
        var colorRect = getRect(100, position);
        var nameRect = getRect(100, position);
        var acRect = getRect(100, position);

        //DataSource dataProp = (DataSource)property.FindPropertyRelative("data").objectReferenceValue;

        SerializedProperty userIndexProperty = property.FindPropertyRelative("fieldIdx");
        SerializedProperty tableIndexProperty = property.FindPropertyRelative("tableIdx");

        SerializedProperty table = property.FindPropertyRelative("tableName");

        //GUIutil.doPrefixLabel(ref labelRect, "Source");
        labelRect.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("dataS"));
        EditorGUI.PropertyField(labelRect, property.FindPropertyRelative("dataS"), GUIContent.none);

        //when changing fields, field is first set to null.
        //keep a list of choices, when field is null, reset choices. if list is null, get choices.
        SerializedProperty p = property.FindPropertyRelative("onSelect");

        if (p.intValue == 2||p.intValue == 3)//custom prop
        {
            string evName = "ev1";
            if (p.intValue == 3) { evName = "evData"; }
            var eventRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            position.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(evName));

            var lblRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            EditorGUI.PrefixLabel(lblRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("OnPress : "));

            eventRect.y += 16;
            EditorGUI.PropertyField(eventRect, property.FindPropertyRelative(evName), GUIContent.none);

        }



        if (p.intValue == 0 || p.intValue == 2)
        {
            GUIutil.doPrefixLabel(ref nameRect, "Display Object");
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("displayObj"), GUIContent.none);
        }
        

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

public class populateList : MonoBehaviour, I_ItemMenu
{

    public ListProps props;
    public GameObject layoutGroup; // where to place generated buttons
    public GameObject prefab;//button prefab
    public bool populateOnStart = true;
    public bool alwaysVisible = false;//for individual item - should infocard pop up, or should data be placed onto an always visible ui element.

    public bool newPopulate = false;
    public bool showDefaultItem = false; //show 'empty' item.
    public bool selectDefaultItem = true;

    [TextArea(3, 10)]
    public string displayCode;
    public List<SourceFilter> filters;
    [Tooltip("Filter always present")]
    public List<SourceFilter> permanentFilter;

    private int defaultSelection = -1;
    private Dictionary<string,object> preservedData = new Dictionary<string, object>();

    [HideInInspector]
    public GameObject selectedItem;

    void Reset()
    {
        layoutGroup = gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetFilter(string newFilter)
    {
        if(filters.Count>0)
        {
            filters[0].filterValue = newFilter;
        }
        Populate();
    }

    private void DB_Loaded()
    {
        //if (populateOnStart && props.db.isDataReady())
        {
            //props.db.getTable(props.tableName).dataChanged += Refresh;
            Populate();
        }
    }

    private void OnEnable()
    {
        if (populateOnStart)
        {
            Populate();
        }
    }

    private void OnDisable()
    {
    }

    public void RefreshFilterByData(IDataLibrary data)
    {
        preservedData = data.GetPreserved();
        if(filters.Count>0)
        {
            if (filters.Count > 0)
            {
                filters[0].filterValue = data.GetValue(filters[0].filterVar).ToString();
            }
            IData id = data.GetValue("DefinitionID");
            if(id.Data != null)
            {
                defaultSelection = (int)(uint)id.Data;
            }
            Populate();
        }
    }

    public void Populate()
    {
        
        bool selected = false;

        Clear();

        DataSource d;
        string primaryKey = "";
        {
            primaryKey = props.data.primaryKey;
            d = props.data;
        }
        bool selectedAnItem = false;
        List<string> keys = d.getFieldFromAllItems(primaryKey);

        IEnumerable<SourceFilter> allFilters = filters.Concat(permanentFilter);

        foreach (string key in keys)
        {
            bool filtered = false;
            foreach (SourceFilter filter in allFilters)
            {
                if(key == "0" && showDefaultItem){ continue; }
                string value = d.getFieldFromItemID(key, filter.filterVar);
                if(!filter.MatchesFilter(value))
                {
                    filtered = true;
                }
            }
            if (filtered) { continue; }

            GameObject obj = Instantiate(prefab, layoutGroup.transform);
            obj.transform.parent = layoutGroup.transform;
            obj.name = key;
            if(!newPopulate)
            {
                FillFromSource fill = obj.GetComponent<FillFromSource>();
                fill.data = d;
                fill.index = key;
                displayObjDetails display = obj.GetComponent<displayObjDetails>();
                display.alwaysVisible = alwaysVisible;
                display.followCursor = !alwaysVisible;
                display.uiObject = props.displayObj;
                SelectableItem select = obj.GetComponent<SelectableItem>();


                if (select != null)
                {
                    select.parentList = this;
                }
                foreach (DataFieldMap m in fill.FieldMapping)
                {
                    m.data = d;
                    SelectableItem item = obj.GetComponent<SelectableItem>();
                    switch (props.onSelect)
                    {
                        case listFunction.showDetails:

                            item.e_pointerDown.AddListener(delegate {; });
                            break;

                        case listFunction.CustomPlusDetails:

                            item.e_pointerDown.AddListener(delegate {; });

                            selectListItem s = new selectListItem();
                            s.fill = fill;
                            s.index = key;

                            item = obj.GetComponent<SelectableItem>();
                            item.e_pointerDown.AddListener(delegate { props.ev1.Invoke(s); });


                            break;

                    }

                }
                fill.fill(displayCode);
            }
            else
            {
                UIButtonListItem listItem = obj.GetComponent<UIButtonListItem>();
                d.AddListener(key, listItem.SourceUpdate);
                DataLibrary dat = new DataLibrary(props.data.getObjFromItemID(key));
                foreach (KeyValuePair<string, object> de in preservedData)
                {
                    dat.SetValue(de.Key, de.Value);
                }
                listItem.SetData(dat);
                listItem.Click = OnClick;
                listItem.source = props.data;
                if(!selectedAnItem)
                {
                    if(selectDefaultItem &&(  defaultSelection == -1 || key == defaultSelection.ToString()))
                    {
                        listItem.Click?.Invoke(listItem);
                        selectedAnItem = true;
                    }
                }
                SelectableItem select = obj.GetComponent<SelectableItem>();
                if (select != null)
                {
                    select.parentList = this;
                }
            }
        }

    }

    public void OnClick(UIButtonListItem listItem)
    {
        props.evData.Invoke(listItem.GetData());
    }

    public void setData(DataSource d)
    {
        //props.data = d;
        Clear();
        Invoke("Populate", 0.2f);
    }

    public void setTable(string t)
    {
        props.tableName = t;
        Clear();
        Invoke("Populate", 0.2f);
    }

    public void Clear()
    {
        GUIutil.clearChildren(layoutGroup.transform);
        //props.Clear();
    }

    public void SetSelected(GameObject obj)
    {
        selectedItem = obj;
    }

    public GameObject GetSelected()
    {
        return selectedItem;
    }
}
