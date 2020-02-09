using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/XMLDataSource", order = 1)]
[System.Serializable]
public class XMLDataSource : DataSource
{
    public string elementToLoad = "";

    public XMLDataSource()
    {
        //WorldFile w = new WorldFile(XDocument.Load(f.FullName).Element("WorldFile"), f.Directory.FullName);
        //condition = maxCond;
        
    }


    private void Awake()
    {
        //if(!dataReady)
        //{
            LoadData();
        //}
    }
    private void OnEnable()
    {
        //if (!dataReady)
        //{
            LoadData();
        //}
    }

    private void OnDisable()
    {
        
    }

    public override void LoadData()
    {
        dataReady = false;
        if(sourceName != "")
        {

            data = new Dictionary<string,Dictionary<string, string>>();
            XDocument doc = XDocument.Load(Application.dataPath + "/Resources/" + sourceName);

            foreach (XElement element in doc.Descendants(elementToLoad))
            {
                Dictionary<string,string> list = element.Attributes().ToDictionary(c => c.Name.LocalName,c=> c.Value);
                data.Add(list[primaryKey],list);
                Debug.Log(element);
            }
            if(data.Count > 0)
            {
                dataReady = true;
                doOnDataReady();
            }

        }
       
        
    }

    

    public override Dictionary<string,fieldType> getFields()
    {
        return new Dictionary<string, fieldType>();
    }

    public override List<string> getFieldSimple()
    {
        if(dataReady)
        {
            List<string> fields = new List<string>();
            foreach(Dictionary<string,string> dict in data.Values)
            {
                foreach (KeyValuePair<string, string> entry in dict)
                {
                    if(!fields.Contains(entry.Key))
                    {
                        fields.Add(entry.Key);
                    }
                    // do something with entry.Value or entry.Key
                }
            }
            return fields;
        }
        return new List<string>();
    }

    public override List<string> getFieldFromAllItems(string field)
    {
        if (dataReady)
        {
            List<string> allVals = new List<string>();
            foreach (Dictionary<string, string> dict in data.Values)
            {
                    string val = "none";
                    dict.TryGetValue(field, out val);

                    allVals.Add(val);
                    // do something with entry.Value or entry.Key
            }
            return allVals;
        }
        return new List<string>();
    }

    public override Dictionary<string,string> getFieldFromAllItemsKeyed(string field, bool uniqueOnly = false, bool sort = false)
    {
        if (dataReady)
        {
            Dictionary<string,string> allVals = new Dictionary<string,string>();

           

            foreach (Dictionary<string, string> dict in data.Values)
            {
                string val = "none";
                string key = "none";
                dict.TryGetValue(field, out val);
                dict.TryGetValue(primaryKey, out key);

                if(uniqueOnly && allVals.ContainsKey(val))
                {
                    continue;
                }
                allVals.Add(val,key);
                // do something with entry.Value or entry.Key
            }

            if(sort)
            {
                //this seems the easiest way to sort.
                var items = from pair in allVals
                            orderby pair.Key ascending
                            select pair;

                return items.ToDictionary(t => t.Key, t => t.Value);
            }
            else
            {
                return allVals;
            }

        }
        return new Dictionary<string, string>();
    }


    public override string getFieldFromItemID(string id, string field)
    {
        if (dataReady/* && id > 0 && id < data.Count*/)
        {

            Dictionary<string, string> dict = data[id];

            string val;
            dict.TryGetValue(field, out val);

            return val ?? "none";
        }
        return "";
    }

    public override string setFieldFromItemID(string id, string field, string value)
    {
        if (dataReady/* && id > 0 && id < data.Count*/)
        {

            Dictionary<string, string> dict = data[id];

            string val;
            if (dict.ContainsKey(field)) 
            {
                dict[field] = value;
                return "success";
            }
            else
            {
                return "field not found";
            }
        }
        return "Data Not Ready";
        //this will require a save operation, as any changed data will be lost on exit.
    }

    public override Dictionary<string,string> getFieldsFromItemID(string id)
    {
        if (dataReady/* && id > 0 && id < data.Count*/)
        {

            Dictionary<string, string> dict = data[id];
            if(dict != null)
            {
                return dict;
            }

        }
        return new Dictionary<string, string>();
    }


}