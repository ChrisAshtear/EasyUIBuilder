using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class projectHandler : MonoBehaviour
{
    static public projData pData;
    static protected bool m_Loaded = false;
    static public bool loaded { get { return m_Loaded; } }
    private string pName;
    public static projectHandler ins;

    private void Start()
    {
        ins = this;
        loadData();
    }

    public static void init()
    {
        projectHandler.pData = Resources.Load("ProjectData") as projData;
        if (pData != null)
        {
            m_Loaded = true;
        }
    }

    public void loadData()
    {
        projectHandler.pData = Resources.Load("ProjectData") as projData;
        if(pData == null)
        {
            loadData();
        }
        else
        {
            pName = pData.gameName;
            m_Loaded = true;
        }
    }

    public static int returnInt(string valName)
    {
        int val = (int)pData.GetType().GetField(valName).GetValue(pData);
        return val;
    }

    public static string returnStr(string valName)
    {
        string val = (string)pData.GetType().GetField(valName).GetValue(pData);
        return val;
    }

}
