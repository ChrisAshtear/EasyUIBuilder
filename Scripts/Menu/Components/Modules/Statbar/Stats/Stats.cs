using System;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IStats,IStatsCallback
{
    Dictionary<string, Stat> statsList;

    public Dictionary<string,Stat> AllStats { get { return statsList; } }

    public Action<Stats> StatsChanged { get; set; }
    private void Awake()
    {
        statsList = new Dictionary<string, Stat>();
    }


    public void SetBaseValue(string valueName, float value)//notify within this function instead of stat.
    {
        Stat stat = GetStat(valueName);
        stat.BaseValue = value;
        stat.CurrentValue = value;
    }

    public void SetCurrentValue(string valueName, float value, bool notify = false)
    {
        Stat stat = GetStat(valueName);
        stat.CurrentValue = value;
        if (value> stat.MaxValue) { stat.CurrentValue = stat.MaxValue; }
        if(notify)
        {
            stat.StatChanged?.Invoke(stat);
        }
    }

    private Stat GetStat(string valueName)
    {
        if (!statsList.ContainsKey(valueName))
        {
            statsList.Add(valueName, new Stat(valueName,0));
        }

        return statsList[valueName];
    }

    public float GetBaseValue(string valueName)
    {
        Stat stat = GetStat(valueName);
        return stat.BaseValue;
    }

    public float GetEndValue(string valueName)
    {
        Stat stat = GetStat(valueName);
        return stat.FinalValue;
    }

    public void AddModifier(string modifierName, float value,bool dontNotify = false)
    {
        Stat stat = GetStat(modifierName);
        stat.AddModifier(value, dontNotify);
    }

    public float GetModifier(string modifierName)
    {
        Stat stat = GetStat(modifierName);
        return stat.Modifiers;
    }

    public void AddListener(string statName, Action<Stat> action)
    {
        Stat stat = GetStat(statName);

        stat.StatChanged += action;
    }

    public void SetMaxValue(string statName, float value)
    {
        Stat stat = GetStat(statName);
        stat.MaxValue = value;
    }

    public float GetMaxValue(string statName)
    {
        Stat stat = GetStat(statName);
        return stat.MaxValue;
    }

    public void RemoveListener(string statName, Action<Stat> action)
    {
        Stat stat = GetStat(statName);
        stat.StatChanged -= action;
    }

    public IStatReadOnly GetStatData(string statName)
    {
        return GetStat(statName);
    }
}
