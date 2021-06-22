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

    private ShowPanels panels;

    public static MenuManager ins;

    [HideInInspector] public string currentPanel = "MenuPanel";
    private string prevPanel;

    public GameObject pausePanel;
    [HideInInspector] public string pausePanelName = "PausePanel";

    protected Dictionary<string, AudioClip> sounds;


    private AudioSource audio;
    private AudioSource music;

    public static bool gameRunning = false;
    public static bool dataLoaded { get; } = false;

    public displayObjDetails selectedDetails;
    public GameObject detailsCard;
    public GameObject selectedItem;

    public bool changeScene = true;
     
    private GameObject msgPanel;
    public GameObject returnCurrentPanel()
    {
        return panels.returnPanel(currentPanel);
    }
	public void quitGame()
	{
        Invoke("exit", 1);
        ProjectSettings.data.PlaySound(ProjectSettings.data.menuCancel);
        panels.hidePanel(currentPanel, true);
    }

    public void exit()
    {
        Application.Quit();
    }

    private void OnLevelWasLoaded(int level)
    {
        audio = GetComponent<AudioSource>();
        panels = gameObject.GetComponent<ShowPanels>();

        if(MenuManager.ins != null && MenuManager.ins != this)
        {
            Destroy(this);
        }
        else
        {
            MenuManager.ins = this;
        }

        string[] hideInMenu = { "GamePanel", "GameOver" };
        string[] hideInGame = { "GameOver" };
        string[] showInGame = { "GamePanel" };
        string[] showInMenu = { "MenuBG"};

        if (ProjectSettings.data.showInMenu != "")
        {
            showInMenu = ProjectSettings.data.showInMenu.Split(',');
        }
        if (ProjectSettings.data.hideInMenu != "")
        {
            hideInMenu = ProjectSettings.data.hideInMenu.Split(',');
        }
        if (ProjectSettings.data.showInGame != "")
        {
            showInGame = ProjectSettings.data.showInGame.Split(',');
        }
        if (ProjectSettings.data.hideInGame != "")
        {
            hideInGame = ProjectSettings.data.hideInGame.Split(',');
        }

        switch (level)
        {
            case 0:
                foreach(string s in hideInMenu)
                {
                    panels.hidePanel(s);
                }
                foreach (string s in showInMenu)
                {
                    panels.showPanel(s);
                }
                break;

            case 1:
                foreach (string s in hideInGame)
                {
                    panels.hidePanel(s);
                }
                foreach (string s in showInGame)
                {
                    panels.showPanel(s);
                }
                break;
        }

    }

    public void changeMenu(string panel,string pressSound = "")
    {
        if(pressSound == "")
        {
            pressSound = ProjectSettings.data.menuConfirm;
        }
        ProjectSettings.data.PlaySound(pressSound);
        prevPanel = currentPanel;
        
        panels.hidePanel(currentPanel,true);
        panels.showPanel(panel, true);
        currentPanel = panel;
    }

    public void setPanel(string panel, bool show)
    {
        panels.setPanel(panel, show,false);
    }

    public void goBack()
    {
        GameObject panel = returnCurrentPanel();

        PanelInfo pInfo = panel.GetComponent<PanelInfo>();

        string targetPanel = prevPanel;//just returns last menu game was on

        if(pInfo != null && pInfo.parentPanel != null) // check PanelInfo property, this allows hierachical menus.
        {
            targetPanel = pInfo.parentPanel.name;
        }

        if(targetPanel != null)
        {
            changeMenu(targetPanel, ProjectSettings.data.menuCancel);
        }
        
    }

    public void changeMusic(AudioClip m)
    {
        music.Stop();
        if(m == null)
        {
            return;
        }
        music.clip = m;
        music.loop = true;
        music.Play();
    }
    
    public void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();

        audio = audios[0];
        music = audios[1];

        audio.outputAudioMixerGroup.audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("sfxVol"));
        music.outputAudioMixerGroup.audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("musicVol"));
        

        //use an event to change music after pdata is loaded?
        MenuManager.ins = returnInstance();
        panels = gameObject.GetComponent<ShowPanels>();

        #if UNITY_ANDROID
        //googlePlay = GameObject.Find("UI").GetComponent<gplayInteractor>();
#endif
        setVolume();

        currentPanel = "MenuPanel";
        if(pausePanel != null)
        {
            pausePanelName = pausePanel.name;
        }
        ProjectSettings.data.player = GetComponent<AudioSource>();
    }


    public void finishInit()
    {
        //changeMusic(ProjectSettings.data.menuMusic);
    }

    
    public void setVolume()
    {
        if (PlayerPrefs.GetString("Muted", "no") != "yes")
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }

    public void processPrefs()
    {
        playSound(ProjectSettings.data.menuConfirm);
        setVolume();
    }

    public void playSound(AudioClip clip)
    {
        audio.Stop();
        if(clip == null)
        { return; }
        audio.PlayOneShot(clip);
    }

    public static void playSound(string sfxName)
    {
        ProjectSettings.data.PlaySound(sfxName);
    }

    public static void statusMessage(string message,float time = 1)
    {
        GameManager.ins.setUIVal("message", message);
        ins.panels.showPanel("MessagePanel",false);
        ins.msgPanel = ins.panels.returnPanel("MessagePanel");
        ins.Invoke("endStatusMessage", time);
    }

    public void endStatusMessage()
    {
        msgPanel.GetComponent<doAnimAndSleep>().doAnim();
    }

    public void toggleUIelement(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public static void startGame(string x)
    {
        ins.startGame();
    }

    public void startGame()
    {
        ProjectSettings.data.PlaySound(ProjectSettings.data.gameStart);
        panels.hidePanel(currentPanel, true);
        Animator fader = panels.returnPanel("Fade").GetComponent<Animator>();
        fader.SetTrigger("fade");
        Invoke("doStart", 1.5f);
        currentPanel = "GamePanel";
    }

    public void loadGame()
    {
        startGame();
        Invoke("doLoad", 2f);
    }

    public void doLoad()
    {
        GameManager.ins.Load();
        GameManager.ins.gameStart();
        
    }

    public void doStart()
    {
        if(changeScene)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else
        {
            OnLevelWasLoaded(1);
        }
        
        changeMusic(ProjectSettings.data.gameMusic);
        MenuManager.gameRunning = true;
        GameManager.ins.gameStart();
    }

    public void StopGameplay()
    {
        gameRunning = false;
        Animator fader = panels.returnPanel("Fade").GetComponent<Animator>();
        fader.SetTrigger("fade");
        Invoke("doStop", 1.5f);
    }

    public void doStop()
    {
        if(changeScene)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            OnLevelWasLoaded(0);
        }
        
        changeMusic(ProjectSettings.data.menuMusic);
        panels.showPanel("MenuPanel", true);
        currentPanel = "MenuPanel";
    }

    public void openWeb(string address)
    {
        ProjectSettings.data.PlaySound(ProjectSettings.data.menuConfirm);
#if !UNITY_WEBGL
        Application.OpenURL(address);
#endif
#if UNITY_WEBGL
        Application.ExternalEval("window.open(\"" + address + "\")");
#endif
    }

    public void Restart()
    {
        UnPause();
        panels.hidePanel("GameOver", true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        MenuManager.ins.changeMusic(ProjectSettings.data.gameMusic);
        startGame();
    }


    public static MenuManager returnInstance()
    {
        MenuManager instance = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>(); // i know this is an extra line but unity freaked out when removed.
        ins = instance;
        return instance;
    }

    public void DoPause(bool showPauseMenu = false)
    {
        GameManager.isPaused = true;
        //Set time.timescale to 0, this will cause animations and physics to stop updating
        Time.timeScale = 0;
        if (showPauseMenu)
        {
            panels.showPanel(pausePanelName);
        }
    }

    public void doGameOver()
    {
        panels.showPanel("GameOver");
    }

    public void UnPause()
    {
        GameManager.isPaused = false;
        //Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
        Time.timeScale = 1;
        panels.hidePanel(pausePanelName);
    }

}