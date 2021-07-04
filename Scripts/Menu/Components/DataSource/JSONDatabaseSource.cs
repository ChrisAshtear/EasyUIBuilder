using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Xml.Linq;
using System;
using UnityEngine.Networking;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.IO;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DatabaseSource/JSONDatabaseSource", order = 1)]
public class JSONDatabaseSource : DatabaseSource
{
    //if we can use a text asset here & also support an assetbundle/web request that would be ideal.
    public TextAsset JSON_file; // load from here if loadFromURL false
    public string URL; // load from here if loadFromURL true
    public bool loadFromURL = false;
    HttpWebRequest webRequest;
    //Props

    private void Awake()
    {
        type = DataType.JSON;
    }

    public void LoadFromString(string data)
    {
        JObject dat = JObject.Parse(data);

        bool firstElement = true;

        /*DataSource currentTable = new DataSource();
        currentTable.name = "RootProperties";
        tables.Add(currentTable.name, currentTable);*/
        foreach (JProperty obj in dat.Properties())
        {
            if (obj.Value.Type == JTokenType.Array)
            {
                DataSource table = new DataSource(obj.Name, primaryKey);
                JArray arr = (JArray)obj.Value;
                tables.Add(obj.Name, table);
                foreach (JToken t in arr.Children())
                {
                    Dictionary<string, object> row = JsonConvert.DeserializeObject<Dictionary<string, object>>(t.ToString());
                    table.data.Add(row[primaryKey].ToString(), row);
                }
                table.setReady();
            }

        }

        if (tables.Count > 0)
        {
            dataReady = true;
        }
        doOnDataReady();
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public override void LoadData()
    {
        tables = new Dictionary<string, DataSource>();
        displayCodes = new Dictionary<string, string>();
        dataReady = false;

        Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();

        try
        {
            if (loadFromURL)
            {
                string getDataUrl = "https://us-central1-warscrap-c63.cloudfunctions.net/api/" + URL;
                NetUtil.DoWebRequest(getDataUrl, LoadFromString);
            }
            else
            {
                LoadFromString(JSON_file.text);
            }
        }
        catch (Exception e)
        {
            if (e.Message.Contains("dictionary"))
            {
                loadStatus = "key not found";
            }
            else
            {
                loadStatus = e.Message;
            }

        }
    }

}
