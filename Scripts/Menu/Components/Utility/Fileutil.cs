using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Fileutil
{
    public static Color32 getColorFromString(string colData)
    {

        string[] hcolors = colData.Split(',');
        Color32 color = new Color32((byte)int.Parse(hcolors[0]), (byte)int.Parse(hcolors[1]), (byte)int.Parse(hcolors[2]), 255);
        return color;
    }
}
