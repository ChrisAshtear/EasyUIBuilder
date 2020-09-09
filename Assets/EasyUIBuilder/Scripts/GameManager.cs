using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newRunData
{
    public float newHeight;
    public float delay;
    public float speed;
}

public class GameManager : MonoBehaviour
{
    public static GameManager ins;

    public static bool isPaused = false;

    Dictionary<string, string> uiVals;

    public static bool draggingObject = false;
    public static GameObject draggedObject;

    public static int score = 0;
    public static int missesLeft = 3;

    public static int aiScore = 0;
    public static int aiMissesLeft = 0;

    public GameObject playerPrefab;
    public GameObject balloonPrefab;

    public static bool isGameOver = false;
    public static bool isAIGameOver = false;

    public static int minSpeed = 4;
    public static int maxSpeed = 8;

    public static int increaseSpeedByRun = 3;
    public static int runCounter = 0;

    void Awake()
    {
        ins = this;

        uiVals = new Dictionary<string, string>();

    }


    public void gameStart()
    {
        uiVals.Clear();

        GameManager.runCounter = 0;
        GameManager.isGameOver = false;
        GameManager.isAIGameOver = false;
        GameManager.score = 0;
        GameManager.aiScore = 0;
        GameManager.missesLeft = 3;
        GameManager.aiMissesLeft = 3;
        GameManager.minSpeed = 4;
        GameManager.maxSpeed = 8;
        setUIVal("score", score.ToString());
        setUIVal("missesLeft", missesLeft.ToString());
        setUIVal("aiScore", missesLeft.ToString());
        setUIVal("aiMissesLeft", missesLeft.ToString());
    }

    public static  newRunData makeNewRun(bool playerHasFired)
    {
        newRunData dat = new newRunData();
        runCounter++;
        if (runCounter == 0 || runCounter % 2 == 0)
        {
            minSpeed+=2;
            maxSpeed+=2;
        }
        dat.speed = Random.Range(minSpeed, maxSpeed);
        dat.newHeight = Random.Range(24, 49);
        dat.delay = 1f;
        
        if(!playerHasFired)
        {
            missed(false);
        }
        return dat;
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

    public static void addToScore(int amt,bool ai=false)
    {
        if(!ai)
        {
            score += amt;
            ins.setUIVal("score", score.ToString());
        }
        else
        {
            aiScore += amt;
            ins.setUIVal("aiScore", aiScore.ToString());
        }
    }

    public static void missed(bool ai=false)
    {
        if(!ai)
        {
            missesLeft -= 1;
           
            if (missesLeft <= 0)
            {
                missesLeft = 0;
                gameOver();
            }
            ins.setUIVal("missesLeft", missesLeft.ToString());
        }
        else
        {
            aiMissesLeft -= 1;
            
            if (aiMissesLeft <= 0)
            {
                aiMissesLeft = 0;
                gameOver(true);
            }
            ins.setUIVal("aiMissesLeft", aiMissesLeft.ToString());
        }
        
        
    }

    public static void gameOver(bool ai = false)
    {
        
        if(ai)
        {
            isAIGameOver = true;
        }
        else
        {
            MenuManager.ins.doGameOver();
            isGameOver = true;
        }
       
    }

    public string getUIVal(string key)
    {
        string output = "";

        uiVals.TryGetValue(key, out output);
        return output;
    }

}
