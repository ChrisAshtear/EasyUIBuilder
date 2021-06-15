using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[CustomEditor(typeof(DatabaseSource),true)]
public class DatabaseSourceEditor : Editor
{

    DatabaseSource dbsource;
    static bool showTileEditor = false;

    public void OnEnable()
    {
        dbsource = (DatabaseSource)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //MAP DEFAULT INFORMATION
        //dbsource.name = EditorGUILayout.TextField("Test", dbsource.name);
        List<string> tables = dbsource.getTables();
        string tableList = "";
        EditorGUILayout.IntField("Tables", tables.Count);
        foreach(string t in tables)
        {
            tableList += t + ",";
        }
        EditorGUILayout.LabelField(tableList);
        //WIDTH - HEIGHT
        //int width = EditorGUILayout.IntField("Map Sprite Width", comp.mapSprites.GetLength(0));
        //int height = EditorGUILayout.IntField("Map Sprite Height", comp.mapSprites.GetLength(1));

        /*if (width != comp.mapSprites.GetLength(0) || height != comp.mapSprites.GetLength(1))
        {
            comp.mapSprites = new Sprite[width, height];
        }*/

        /*showTileEditor = EditorGUILayout.Foldout(showTileEditor, "Tile Editor");

        if (showTileEditor)
        {
            for (int h = 0; h < height; h++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int w = 0; w < width; w++)
                {
                    comp.mapSprites[w, h] = (Sprite)EditorGUILayout.ObjectField(comp.mapSprites[w, h], typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));
                }
                EditorGUILayout.EndHorizontal();
            }
        }*/
        if(GUILayout.Button("Load"))
        {
            dbsource.LoadData();
        }
        EditorUtility.SetDirty(dbsource);
    }

}


[System.Serializable]
public class DatabaseSource : ScriptableObject
{
    [HideInInspector]
    public string sourceName;
    [HideInInspector]
    public string addressOfData;
    protected bool dataReady = false;
    [HideInInspector]
    public DataType type;

    public delegate void DataReadyHandler();
    public event DataReadyHandler onDataReady;

    public Dictionary<string, DataSource> tables;

    public Dictionary<string, string> displayCodes;

    public string primaryKey = "UUID";

    protected string selectedKey = "NA";

    public delegate void SelectionChangedHandler();
    public event SelectionChangedHandler selectionChanged;

    public delegate void DataChangedHandler();
    public event DataChangedHandler dataChanged;

    private List<string> tableList;

    //Props
    public DatabaseSource()
    {

    }

    public void UI_SelectEntry(selectListItem e)
    {
        selectItem(e.index,e.fill.data);
        Debug.Log(e.ToString());
    }

    private void Awake()
    {

    }

    private void OnEnable()
    {

    }

    public void addSelectCallback(string tableName,Action<DataSource> action)
    {
        List<string> tableNames = new List<string>(tableName.Split(','));
        foreach (string table in tableNames)
        {
            DataSource d = getTable(table);
            if(d != null)
            {
                d.selectChanged += action;
            }
        }
    }

    public void addChangedCallback(string tableName, Action<DataSource> action)
    {
        List<string> tableNames = new List<string>(tableName.Split(','));
        foreach (string table in tableNames)
        {
            DataSource d = getTable(table);
            if (d != null)
            {
                d.datChanged += action;
            }
        }
    }

    public void dropTable(string tableName)
    {
        tableName = tableName.ToLower();
        if (tables.ContainsKey(tableName))
        {
            tables.Remove(tableName);
        }
    }

    public virtual List<string> getTables()
    {
        return tableList;
    }

    public virtual void addTable(string tableName, DataSource table)
    {
        tableName = tableName.ToLower();
        if (!tables.ContainsKey(tableName))
        {
            tables.Add(tableName, table);
        }
    }

    public virtual DataSource newTable(string tableName)
    {
        tableName = tableName.ToLower();
        if (!tables.ContainsKey(tableName))
        {
            DataSource table = new DataSource();
            table.name = tableName;
            tables.Add(tableName, table);
            //table.parentData = this;
            return table;
        }
        else
        {
            return getTable(tableName);
        }
    }

    public virtual bool containsTable(string id)
    {
        return tables.ContainsKey(id);
    }

    public virtual DataSource getTable(string tableName)
    {
        tableName = tableName.ToLower();
        DataSource table = new DataSource();
        tables?.TryGetValue(tableName, out table);
        return table;
    }

    public bool isDataReady()
    {
        if (tables == null || tables.Count == 0)
        {
            return false;
        }
        return dataReady;
    }

    public virtual void LoadData()
    {
    }

    protected void doOnDataReady()
    {
        if (onDataReady != null)
        {
            onDataReady();
        }
        tableList.Clear();
        foreach (DataSource source in tables.Values)
        {
            if (!tableList.Contains(source.name))
            {
                tableList.Add(source.name);
            }
        }
    }

    protected void doOnDataChanged()
    {
        if (dataChanged != null) { dataChanged(); }
    }

    public virtual DataSource getSelected()
    {
        if (dataReady && tables != null && tables.ContainsKey(selectedKey))
        {
            DataSource entry = tables[selectedKey];
            return entry;
        }
        else
        {
            return null;
        }
    }

    public void changedData()
    {
        if (dataChanged != null && selectedKey != "NA")
        {
            dataChanged();
        }
    }

    public virtual void selectNextItemInTable(string table)
    {
        getTable(table)?.selectNext();
    }

    public virtual void selectPrevItemInTable(string table)
    {
        getTable(table)?.selectPrev();
    }

    public virtual string getSelectedKey()
    {
        return selectedKey;
    }

    public virtual void selectItem(string key,DataSource table)
    {
        selectedKey = key;
        table?.selectItem(key);
        if (selectionChanged != null)
        {
            selectionChanged();
        }
    }

    //TODO:get all fields.
    public virtual Dictionary<string, fieldType> getFields()
    {
        return new Dictionary<string, fieldType>();
    }
}
