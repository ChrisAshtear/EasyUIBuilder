using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class projectHandler : MonoBehaviour
{
    static public projData pData;
    static protected bool m_Loaded = false;
    static public bool loaded { get { return m_Loaded; } }
    private string pName;
    public static projectHandler ins;

    public delegate void DataReady();
    public event DataReady onDataReady;

    private void Awake()
    {
        ins = this;
        projectHandler.pData = Resources.Load("ProjectData") as projData;
        Invoke("doInit", 0.5f);
        //init();
    }

    public void doInit()
    {
        projectHandler.init();
        onDataReady();
    }
    public static void init()
    {
        if (pData != null)
        {
            m_Loaded = true;
        }
        else
        {

#if UNITY_EDITOR
            //createData();
#endif
        }
    }

    public void loadData()
    {
        if(pData == null)
        {
#if UNITY_EDITOR
            //createData();
#endif
        }
        else
        {
            pName = pData.gameName;
            m_Loaded = true;
        }
    }

#if UNITY_EDITOR
    public static void createData()
    {
        projData pDat = (projData)ScriptableObject.CreateInstance(typeof(projData));
        pDat.defaultButton = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("ButtonPrefab")[0]),typeof(UnityEngine.GameObject));
        pDat.menuConfirm = (AudioClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:AudioClip")[0]),typeof(UnityEngine.AudioClip));
        pDat.menuCancel = (AudioClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:AudioClip")[0]),typeof(UnityEngine.AudioClip));
        pData = pDat;
        AssetDatabase.CreateAsset(pDat, "Assets/Resources/ProjectData.asset");
        
    }
#endif
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
