using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

public class MenuManager : MonoBehaviour
{

    //private adMobHandler adMob_Handler;
    private ShowPanels panels;

    private static bool firstRun = true;

    public static MenuManager ins;

    public string currentPanel = "Main";
    public string prevPanel;
    public AudioClip menuSound;
    private AudioSource audio;

    MenuManager()
    {
        //attemptedLogin = false;
        //initAdBag();
        
    }

    private void OnEnable()
    {
        
        #if UNITY_ANDROID
        googlePlay = GameObject.Find("UI").GetComponent<gplayInteractor>();
        #endif
        if(MenuManager.firstRun)
        {
            MenuManager.firstRun = false;
        }
    }

    public GameObject returnCurrentPanel()
    {
        return panels.returnPanel(currentPanel);
    }

	public void quitGame()
	{
        Invoke("exit", 1);
        audio.PlayOneShot(menuSound);
        panels.hidePanel(currentPanel, true);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void changeMenu(string panel)
    {
        audio.Stop();
        audio.PlayOneShot(menuSound);
        prevPanel = currentPanel;
        
        panels.hidePanel(currentPanel,false);
        panels.showPanel(panel, false);
        currentPanel = panel;
    }

    public void goBack()
    {
        //hierarchical menu needs to be set for this to work right
        if(prevPanel != null)
        {
            changeMenu(prevPanel);
        }
        
    }
    
    public void Start()
    {
        audio = GetComponent<AudioSource>();
        ins = this;
        panels = gameObject.GetComponent<ShowPanels>();

        #if UNITY_ANDROID
        googlePlay = GameObject.Find("UI").GetComponent<gplayInteractor>();
#endif
        setVolume();

        currentPanel = "MenuPanel";
    }

    

    public void setVolume()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }

    public void toggleUIelement(GameObject obj)
    {
        obj.SetActive(!obj.active);
    }

    public static void startGame(string x)
    {
        ins.startGame();
    }

    public void startGame()
    {

    }

    public static MenuManager returnInstance()
    {
        MenuManager instance = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>(); // i know this is an extra line but unity freaked out when removed.
        return instance;
    }

}