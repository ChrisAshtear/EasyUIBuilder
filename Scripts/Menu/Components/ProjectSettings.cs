using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ProjectSettings
{
    public static ProjectData data;

    public static string returnStr(string valName)
    {
        if (data == null) { return ""; }
        string val = (string)data.GetType().GetField(valName).GetValue(data);
        return val;
    }
}