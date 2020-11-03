using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Xml.Linq;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DatabaseSource/XMLDatabaseSource", order = 1)]
public class XMLDatabaseSource : DatabaseSource
{

    //Props
    public XMLDatabaseSource()
    {
        //condition = maxCond;
    }

    private void Awake()
    {
        type = DataType.XML;
        LoadData();
    }

    private void OnEnable()
    {

        LoadData();
    }

    public override void LoadData()
    {
        tables = new Dictionary<string, DataSource>();
        displayCodes = new Dictionary<string, string>();
        dataReady = false;
        if (sourceName != "")
        {

            Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();

#if UNITY_WEBGL
            TextAsset xml = Resources.Load(sourceName.Split('.')[0]) as TextAsset;
            XDocument doc = XDocument.Parse(xml.text);
#endif
            //if x86
#if !UNITY_WEBGL
            XDocument doc = XDocument.Load(Application.streamingAssetsPath + "\\" + sourceName);
#endif
            bool firstElement = true;
            DataSource currentTable = null;

            foreach (XElement element in doc.Descendants())
            {
                if(firstElement)//Root
                {
                    firstElement = false;
                    continue;
                }
                if(element.HasElements && element.Attributes().Count() <=1)//Table
                {
                    DataSource table = new DataSource();
                    table.name = element.Name.ToString();
                    table.parentData = this;
                    addTable(element.Name.ToString(), table);
                    currentTable = table;
                    table.data = new Dictionary<string, Dictionary<string, object>>();
                    if (element.Attribute("displayCode") != null)
                    {
                        displayCodes.Add(element.Name.ToString(), element.Attribute("displayCode").Value);
                    }
                }
                else if(element.Attributes().Count() > 1 && element.Parent.Name.ToString() == currentTable.name)//Entry
                {                  
                    Dictionary<string, object> list = element.Attributes().ToDictionary(c => c.Name.LocalName, c => (object)c.Value);
                    currentTable.data.Add(list[primaryKey].ToString(), list);
                }
                else if(element.HasAttributes)
                {       
                    DataSource table = new DataSource();
                    table.name = element.Name.ToString();
                    table.parentData = this;
                    addTable(element.Name.ToString(), table);
                    currentTable = table;
                    table.data = new Dictionary<string, Dictionary<string, object>>();
                    Dictionary<string, object> list = element.Attributes().ToDictionary(c => c.Name.LocalName, c => (object)c.Value);
                    currentTable.data.Add(table.name, list);
                    table.setReady();
                }
                //Dictionary<string, object> list = element.Attributes().ToDictionary(c => c.Name.LocalName, c => (object)c.Value);
                //data.Add(list[primaryKey].ToString(), list);
                Debug.Log(element);
            }
            if (data.Count > 0)
            {
                dataReady = true;
                doOnDataReady();
            }

        }

    }

}
