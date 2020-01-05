using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class scoreEntry : IComparable<scoreEntry>
{
    public int score;
    public string initials;

    public scoreEntry(int score, string initials)
    {
        this.score = score;
        this.initials = initials;
    }

    public int CompareTo(scoreEntry that)
    {
        //this sorts into descending order - so [last] would be the lowest score.
        if (that == null) return 1;
        if (this.score > that.score) return -1;
        if (this.score < that.score) return 1;
        return 0;
    }
}


public class LocalLeaderboard : MonoBehaviour
{
    protected static List<scoreEntry> _scores;
    public int numScores = 5;
    // Use this for initialization
    void Start()
    {
        _scores = new List<scoreEntry>();
        loadLeaderboard();
    }

    private void Awake()
    {
        
    }

    public void addToLeaderboard(scoreEntry entry)
    {
        LocalLeaderboard._scores.Add(entry);
        LocalLeaderboard._scores.Sort();
        if(LocalLeaderboard._scores.Count > numScores)
        {
            LocalLeaderboard._scores.RemoveAt(_scores.Count - 1);
        }
    }

    public void saveLeaderboard()
    {
 
        for(int i=0;i< LocalLeaderboard._scores.Count;i++)
        {
            PlayerPrefs.SetString("LBini" + i.ToString(), LocalLeaderboard._scores[i].initials);
            PlayerPrefs.SetInt("LBscore" + i.ToString(), LocalLeaderboard._scores[i].score);
        }
        PlayerPrefs.SetInt("LBsize", LocalLeaderboard._scores.Count);
        PlayerPrefs.Save();
    }
    
    public int getCount()
    {
        return LocalLeaderboard._scores.Count;
    }

    public int getScore(int index)
    {
        return LocalLeaderboard._scores[index].score;
    }

    public string getInitials(int index)
    {
        return LocalLeaderboard._scores[index].initials;
    }

    public void loadLeaderboard()
    {
        int size = PlayerPrefs.GetInt("LBsize");
        for(int i=0;i<size;i++)
        {
            string initials = PlayerPrefs.GetString("LBini" + i.ToString());
            int score = PlayerPrefs.GetInt("LBscore" + i.ToString());
            addToLeaderboard(new scoreEntry(score, initials));
        }
        LocalLeaderboard._scores.Sort();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
