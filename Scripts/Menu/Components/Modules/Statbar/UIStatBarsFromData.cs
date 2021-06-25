using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-100)]
class UIStatBarsFromData : MonoBehaviour
{
    public GameObject statBarPrefab;
    [Tooltip("Display these stats- use comma to seperate multiple stats(ie-cash,armor)")]
    public string statsToUse;
    public string statLabelList;
    public string keyName = "DefinitionID";
    private string oldKey = "";
    public SourceProps source;

    //might need to pass data source too
    public void RefreshData(IDataLibrary data)
    {
        bool sameKey = (oldKey == data.GetValue(keyName).ToString());
        if (data == null || sameKey) { return; }
        oldKey = data.GetValue(keyName).ToString();
        Clear();
        //ItemTypeInfo info = ItemDatabase.GetItemInfoForUI(data.GetValue("itemType").ToString());
        string[] statLabels = statLabelList.Split(',');
        string[] statList = statsToUse.Split(',');

        DataSource ds = source.db.getTable(source.tableName);
        if (ds == null) { return; }

        for(int i = 0; i<statLabels.Length;i++)
        {
            IData d = data.GetValue(statList[i]);
            GameObject obj = Instantiate(statBarPrefab, transform);
            UIStatBar bar = obj.GetComponent<UIStatBar>();
            bar.fieldName = statLabels[i];
            bar.name = statLabels[i];
            Stat virtualStat = new Stat(d.Name, Convert.ToSingle(d.Data));
            virtualStat.MaxValue = ds.GetMaxValueFromField(statList[i]);
            bar.trackingObject = null;
            bar.enabled = true;
            bar.UpdateData(virtualStat);//initialization
        }
    }

    public void Clear()
    {
        GUIutil.clearChildren(transform);
    }
}

