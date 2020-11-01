using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[System.Serializable]
public class DatabaseSource : ScriptableObject
{
    public string sourceName;
    public string addressOfData;
    protected bool dataReady = false;
    public DataType type;

    public delegate void DataReadyHandler();
    public event DataReadyHandler onDataReady;

    public Dictionary<string, DataSource> tables;

    public string primaryKey = "";

    protected string selectedKey = "NA";

    public delegate void SelectionChangedHandler();
    public event SelectionChangedHandler selectionChanged;

    public delegate void DataChangedHandler();
    public event DataChangedHandler dataChanged;

    //Props
    public DatabaseSource()
    {
        //condition = maxCond;
    }

    public void UI_SelectEntry(selectListItem e)
    {
        selectItem(e.index);
        Debug.Log(e.ToString());
    }

    private void OnEnable()
    {

    }

    public bool isDataReady()
    {
        if (tables == null)
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

    }

   /* public virtual string getSelectedVal(string fieldName)
    {
        DataSource entry = getSelected();
        if (entry != null)
        {
            if (entry.ContainsKey(fieldName))
            {
                return entry[fieldName].ToString();
            }
        }
        return "Not Found";
    }*/

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

    public virtual void changeSelection(int amt)
    {

        List<string> keys = tables.Keys.AsEnumerable().ToList();
        if (selectedKey == "NA")
        {
            selectedKey = keys[0];
            return;
        }
        int curIdx = keys.IndexOf(selectedKey);
        curIdx += amt;
        if (curIdx >= keys.Count)
        {
            curIdx = 0;
        }
        if (curIdx < 0)
        {
            curIdx = keys.Count - 1;
        }
        selectedKey = keys[curIdx];
        if (selectionChanged != null)
        {
            selectionChanged();
        }
    }

    public void changedData()
    {
        if (dataChanged != null && selectedKey != "NA")
        {
            dataChanged();
        }
    }

    public virtual void selectPrev()
    {
        changeSelection(-1);
    }
    public virtual void selectNext()
    {
        changeSelection(1);
    }

    public virtual string getSelectedKey()
    {
        return selectedKey;
    }

    public virtual void selectItem(string key)
    {
        selectedKey = key;
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

    public virtual bool containsTable(string id)
    {
        return tables.ContainsKey(id);
    }


}
