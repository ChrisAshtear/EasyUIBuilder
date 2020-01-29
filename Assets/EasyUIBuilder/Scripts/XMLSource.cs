using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/XMLDataSource", order = 1)]
public class XMLDataSource : DataSource
{
    public string elementToLoad = "";
    public XMLDataSource()
    {
        //WorldFile w = new WorldFile(XDocument.Load(f.FullName).Element("WorldFile"), f.Directory.FullName);
        //condition = maxCond;
        
    }

    private void OnEnable()
    {
        LoadData();
    }

    public override void LoadData()
    {
        dataReady = false;
        if(sourceName != "")
        {

            data = new List<Dictionary<string, string>>();
            XDocument doc = XDocument.Load(Application.dataPath + "/Resources/" + sourceName);

            foreach (XElement element in doc.Descendants(elementToLoad))
            {
                Dictionary<string,string> list = element.Attributes().ToDictionary(c => c.Name.LocalName,c=> c.Value);
                data.Add(list);
                Debug.Log(element);
            }
            if(data.Count > 0)
            {
                dataReady = true;
                doOnDataReady();
            }
            getFieldFromAllItems("Description");
            getFieldFromItemID(2, "Hulld");//move names to lowercase.
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
            foreach(Dictionary<string,string> dict in data)
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
            foreach (Dictionary<string, string> dict in data)
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

    public override string getFieldFromItemID(int id, string field)
    {
        if (dataReady && id > 0 && id < data.Count)
        {

            Dictionary<string, string> dict = data[id];

            string val;
            dict.TryGetValue(field, out val);

            return val ?? "none";
        }
        return "";
    }
}