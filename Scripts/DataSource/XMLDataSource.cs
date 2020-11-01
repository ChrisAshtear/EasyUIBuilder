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

            data = new Dictionary<string,Dictionary<string, object>>();

#if UNITY_WEBGL
            TextAsset xml = Resources.Load(sourceName.Split('.')[0]) as TextAsset;
            XDocument doc = XDocument.Parse(xml.text);
#endif
//if x86
#if !UNITY_WEBGL
            XDocument doc = XDocument.Load(Application.streamingAssetsPath + "\\" + sourceName);
#endif


            foreach (XElement element in doc.Descendants(elementToLoad))
            {
                Dictionary<string,object> list = element.Attributes().ToDictionary(c => c.Name.LocalName,c=> (object)c.Value);
                data.Add(list[primaryKey].ToString(),list);
                Debug.Log(element);
            }
            if(data.Count > 0)
            {
                dataReady = true;
                doOnDataReady();
            }

        }
       
        
    }

    

}