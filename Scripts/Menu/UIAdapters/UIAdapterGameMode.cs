using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*
public class UIAdapterGameMode : MonoBehaviour,IUIAdapter
{
    protected DataLibrary data;
    protected GameMode mode;
    protected GameObject gamemodeObj;

    float lastTime;
    public Action<Dictionary<string, DataObject>> OnValueChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void AddListener(string valueName, Action<IData> callback)
    {
        data.AddListener(valueName, callback);
    }

    public DataObject GetValue(string valueName)
    {
        return (DataObject)data.GetValue(valueName);
    }

    public void RemoveListener(string valueName, Action<IData> callback)
    {
        data.RemoveListener(valueName, callback);
    }

    public void SetValue(string valueName, object value)
    {
        data.SetValue(valueName, value);
    }

    // Use this for initialization
    void Start()
    {
        data = new DataLibrary();
        this.mode = (GameMode)ServiceLocator.GetService<IGameModeService>();
        gamemodeObj = mode.gameObject;
        mode.GameStatsChange += UpdateStats;
        mode.StateChange += EndGameStats;
        PopulateData();
        IPlayerManagerService playerManager = ServiceLocator.GetService<IPlayerManagerService>();
        playerManager.PlayerObjectRemove += PlayerObjectKilled;
        Ready();
    }

    private void OnDestroy()
    {
        mode.GameStatsChange -= UpdateStats;
        mode.StateChange -= EndGameStats;
        IPlayerManagerService playerManager = ServiceLocator.GetService<IPlayerManagerService>();
        playerManager.PlayerObjectRemove -= PlayerObjectKilled;
    }

    protected virtual void Ready()
    {

    }

    public void PopulateData()
    {
        data.SetValue("time", FormatTime(mode.GameModeTime));
        data.SetValue("playerKills", 0);
    }

    private string FormatTime(float time)
    {
        var ts = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    public void EndGame()
    {
        Time.timeScale = 1f;
        mode.ChangeState(GameModeState.Finished);
    }

    public virtual void UpdateStats(GameMode mode)
    {
        Debug.Log("WTF");
    }

    public virtual void EndGameStats(GameModeState state)
    {

    }

    public virtual void PlayerObjectKilled(uint id, GameObject playerObj)
    {

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.DoReload();
    }

    private void Update()
    {
        if (mode?.GameModeTime != lastTime)
        {
            data.SetValue("time", FormatTime(mode.GameModeTime));
        }
    }
    public DataLibrary GetData()
    {
        return data;
    }
}
*/