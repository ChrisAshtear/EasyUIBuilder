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

public class populateList : MonoBehaviour, I_ItemMenu
{

    public ListProps props;
    public GameObject layoutGroup; // where to place generated buttons
    public GameObject prefab;//button prefab
    public bool populateOnStart = true;
    public bool alwaysVisible = false;//for individual item - should infocard pop up, or should data be placed onto an always visible ui element.

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

        if (d.displayCode != null)
        {
            displayCode = d.displayCode;
        }
        UIDisplayCodeController dc = props.displayObj.GetComponent<UIDisplayCodeController>();
        if (dc != null)
        {
            dc.displayCode = displayCode;
        }

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
                switch (props.onSelect)
                {
                    case listFunction.CustomPlusDetails:
                        select.e_pointerDown.AddListener(delegate {; });
                        select.e_pointerDown.AddListener(delegate { props.evData.Invoke(dat); });
                        break;
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
