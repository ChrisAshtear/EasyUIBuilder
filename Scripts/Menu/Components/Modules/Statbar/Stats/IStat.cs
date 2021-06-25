using System;
using System.Collections.Generic;
using UnityEngine;

interface IStatsCallback
{
    Action<Stats> StatsChanged { get; set; }
}

public interface IStat
{
    float BaseValue { get; set; }
    float CurrentValue { get; set; }
    float MaxValue { get; set; }
    float FinalValue { get; }
    float Modifiers { get; }
    string Name { get; }
}

public interface IStatReadOnly
{
    float BaseValue { get;}
    float CurrentValue { get;}
    float MaxValue { get;}
    float FinalValue { get; }
    float Modifiers { get; }
    string Name { get; }
}

public interface IStats
{
    IStatReadOnly GetStatData(string statName); 
    void AddListener(string statName, Action<Stat> action);
    void RemoveListener(string statName, Action<Stat> action);

    void SetCurrentValue(string statName, float value,bool notify=false);

    void SetMaxValue(string statName, float value);
}

public interface ISelectable
{
    void OnSelect(IStats stats);
}