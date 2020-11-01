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
    public XMLDatabaseSource()
    {
        //condition = maxCond;
    }

    private void Awake()
    {
        LoadData();
    }

    public override void LoadData()
    {
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


            foreach (XElement element in doc.Descendants())
            {
                Dictionary<string, object> list = element.Attributes().ToDictionary(c => c.Name.LocalName, c => (object)c.Value);
                data.Add(list[primaryKey].ToString(), list);
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
