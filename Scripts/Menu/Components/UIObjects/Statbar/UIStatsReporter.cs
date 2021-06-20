using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-100)]
class UIStatsReporter : MonoBehaviour
{
    public Action<Stats> StatChanged { get; set; }
    //if a stat is changed the bar itself should update.
    public List<UIStatBar> statsList;
    public GameObject statBarPrefab;
    public GameObject trackingObject;

    [Tooltip("Display these stats- use comma to seperate multiple stats(ie-cash,armor)")]
    public string statsToUse;
    [Tooltip("If you wish you can place properties manually instead of listing them. Check this first, then create child UIStatBars.")]
    public bool placePropertiesManually = false;
    public GameObject target;
    private Stats objStats;

    private void Awake()
    {
        if (target != null)
        {
        }
    }

    private void OnPlayerObjectAdded(GameObject playerObject)
    {
        trackingObject = playerObject;
        RefreshCar(trackingObject);
    }

    private void OnDestroy()
    {
    }

    public void RefreshC(Stats stats)
    {
        RefreshCar(stats.gameObject);
    }

    public void RefreshCar(GameObject newcar)
    {
        if (newcar == null) { return; }
        trackingObject = newcar;
        objStats = trackingObject.GetComponentInChildren<Stats>();
        Dictionary<string, Stat> stats = objStats.AllStats;
        if (placePropertiesManually)
        {
            UIStatBar[] Bars = GetComponentsInChildren<UIStatBar>();
            foreach(UIStatBar bar in Bars)
            {
                stats.TryGetValue(bar.fieldName, out Stat stat);
                bar.InitBar(trackingObject);
                bar.UpdateData(stat);
            }
        }
        else
        {
            Clear();
            string[] statList = statsToUse.Split(',');
            
            foreach (string s in statList)
            {
                stats.TryGetValue(s, out Stat stat);
                GameObject obj = Instantiate(statBarPrefab, transform);
                UIStatBar statBar = obj.GetComponent<UIStatBar>();
                statBar.trackingObject = trackingObject;
                statBar.fieldName = s;
                statBar.name = s;

                statBar.UpdateData(stat);
            }
        }
    }

    public void Clear()
    {
        ObjHelper.ClearChildren(transform);

    }

}

