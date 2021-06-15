using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

class Pause : MonoBehaviour
{
    //private ShowPanels showPanels;                      //Reference to the ShowPanels script used to hide and show UI panels
    private bool isPaused;								//Boolean to check if the game is paused or not
    public bool stopTime = true;//Stop timescale when paused.
    public bool canPause = true;

    private float origVolume;

    public GameObject PausePanel;

    public bool PauseInput { get; set; }
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PauseInput = Input.GetKey(KeyCode.Escape);
        if (PauseInput && !isPaused && canPause) 
        {
            //Call the DoPause function to pause the game
            DoPause();
            return;
        }
        if (PauseInput && isPaused)
        {
            //Call the UnPause function to unpause the game
            UnPause();
        }

    }


    public void DoPause()
    {
        IUIAdapter ui = GetComponent<IUIAdapter>();
        DataLibrary data = ui.GetData();
        isPaused = true;
        if(stopTime)
        {
            Time.timeScale = 0f;
        };
        PausePanel.SetActive(true);
        UIDataController controller = PausePanel.GetComponent<UIDataController>();
        data.SetValue("MenuTitle", "Pause");
        data.SetValue("ShowRestart", true);
        controller.RefreshData(data);
    }

    public bool isGamePaused()
    {
        if (isPaused)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnPause()
    {
        //Set isPaused to false
        isPaused = false;
        //Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }


}
