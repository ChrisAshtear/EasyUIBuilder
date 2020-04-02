using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;

    public static bool isPaused = false;

    Dictionary<string, string> uiVals;

    public static bool draggingObject = false;
    public static GameObject draggedObject;

    void Awake()
    {
        ins = this;

        uiVals = new Dictionary<string, string>();

    }


    public void gameStart()
    {
        uiVals.Clear();
    }

    public void setUIVal(string key, string val)
    {
        if (uiVals.ContainsKey(key))
        {
            uiVals[key] = val;
        }
        else
        {
            uiVals.Add(key, val);
        }
    }

    public string getUIVal(string key)
    {
        string output = "";

        uiVals.TryGetValue(key, out output);
        return output;
    }

}
