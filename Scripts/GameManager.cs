using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Timers;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;

    public static bool isPaused = false;

    protected Dictionary<string, string> uiVals;

    public static bool draggingObject = false;
    public static GameObject draggedObject;

    protected bool gameLoaded = false;

    private string savePath;

    void Awake()
    {
        ins = this;

        uiVals = new Dictionary<string, string>();

    }

    public void gameStart()
    {
        CancelInvoke();

        ins = this;
        finishGameStart();
    }

    public virtual void finishGameStart()
    {

    }

    public void OnLevelWasLoaded(int level)
    {
        if (MenuManager.gameRunning)
        {
            gameStart();
        }
    }

    public void setUIVal(string key, object val)
    {
        if (uiVals.ContainsKey(key))
        {
            uiVals[key] = val.ToString();
        }
        else
        {
            uiVals.Add(key, val.ToString());
        }
    }

    public string getUIVal(string key)
    {
        string output = "";

        uiVals.TryGetValue(key, out output);
        return output;
    }


    private void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string finalpath = path + "/" + Application.companyName + "/" + Application.productName;
        savePath = finalpath.Replace("\\", "/");
#endif
#if !UNITY_STANDALONE_WIN && !UNITY_EDITOR
            savePath = Application.persistentDataPath;
#endif
    }

    public virtual object prepareSave()
    {
        return new object();
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(savePath + "/gamesave.save"));
        object save = prepareSave();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }

    public virtual void finishLoad(object save)
    {

    }

    public void Load()
    {
        if (File.Exists(savePath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath + "\\gamesave.save", FileMode.Open);
            object save = bf.Deserialize(file);
            file.Close();
            finishLoad(save);
            gameLoaded = true;
        }


    }
}
