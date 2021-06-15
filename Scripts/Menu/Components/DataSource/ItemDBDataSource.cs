using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using UnityEditor;
/*
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemDBDataSource", order = 1)]
[System.Serializable]
public class ItemDBDataSource : DataSource
{
    private object sourceData;
    //public bool useListIndexAsKey = true;

    public override void LoadData()
    {
        IReadOnlyDictionary<uint, ItemDefinition> itemData = ItemDatabase.GetAllOrdered();
        object newdata = itemData;
        data = new Dictionary<string, Dictionary<string, object>>();
        if (newdata is IDictionary)
        {
            foreach (DictionaryEntry entry in (IDictionary)newdata)
            {
                Dictionary<string, object> fields;
                fields = entry.Value.GetType().GetFields().ToDictionary(prop => prop.Name, prop => prop.GetValue(entry.Value));
                data.Add(entry.Key.ToString(), fields);
            }
        }
        dataReady = true;
        fieldChanged = new Dictionary<string, Action<string,object>>();

    }

}*/