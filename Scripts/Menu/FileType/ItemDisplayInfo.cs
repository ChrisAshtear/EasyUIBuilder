
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

public struct ItemTypeInfo
{
    public string typeName;
    public string statBarFields;
    public string statBarLabels;
    public string subType;//??
    public Sprite typeImage;
}

public struct TypeFile
{
    // Attributes

    public Dictionary<string,ItemTypeInfo> types;

    public TypeFile(XElement element)
    {
        // Attributes
        // Elements
        types = element
            .Elements("typeList")
            ?.Elements("type")
            .Select(p => new ItemTypeInfo
            {
                typeName = (string)p.Attribute("name"),
                statBarFields = (string)p.Attribute("statBarFields"),
                statBarLabels = (string)p.Attribute("statBarLabels"),
                subType = (string)p.Attribute("subType"),
            })
            .ToDictionary(y => y.typeName, y => y);
    }


}