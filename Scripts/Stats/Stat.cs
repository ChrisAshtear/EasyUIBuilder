using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Stat : IStat,IStatReadOnly
{
    public Action<Stat> StatChanged { get; set; }

    float currentVal;
    float baseVal;
    float mods;
    float maxVal;
    string name;

    float lastUpdate;

    public float LastChange;//last change of current value

    public float FinalValue { get { return currentVal + mods; } }
    public float Modifiers { get { return mods; } }

    public float BaseValue { get { return baseVal; } set { baseVal = value; /*StatChanged?.Invoke(this); */} }

    public float CurrentValue { get { return currentVal; } set { LastChange = Math.Abs(currentVal - value); currentVal = value; /*StatChanged?.Invoke(this); */} }

    public string Name { get { return name; } }

    public float MaxValue { get { return maxVal; } set { maxVal = value; /*StatChanged?.Invoke(this); */} }

    public Stat(string name,float baseVal)
    {
        this.name = name;
        maxVal = this.baseVal = currentVal = baseVal;
    }

    public void AddModifier(float value, bool dontNotify = false)
    {
        LastChange = value;
        mods += value;
        if (!dontNotify)
        {
            StatChanged?.Invoke(this);
        }
    }
}

