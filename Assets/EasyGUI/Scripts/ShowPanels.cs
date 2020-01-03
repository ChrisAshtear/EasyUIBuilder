using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

    public GameObject DialogContainer;//container for dialog box windows that go over everything

    public string animInName;
    public string animOutName;
    //public GameObject parentPanel;

	//Call this function to activate and display the Options panel during the main menu

	public void setPanel(string panelName, bool enabled, bool dialog)
	{
		returnPanel (panelName,dialog).SetActive (enabled);
	}

	public GameObject returnPanel(string panelName, bool dialog = false)
	{
        if (dialog)
        {
            return DialogContainer.transform.Find(panelName).gameObject;
        }
        else
        {
            return transform.Find(panelName).gameObject;
        }
	}

	public void showPanel(string panelName, bool animate = false, bool dialog = false)
	{
		setPanel (panelName, true,dialog);
        if (animate && returnPanel(panelName, dialog).GetComponent<Animator>() != null)
        { returnPanel(panelName,dialog).GetComponent<Animator>().SetTrigger(animInName); }
    }

    public void showPanel(GameObject panel, bool animate = false)
    {
        panel.SetActive(true);
    }

    public void hidePanel(GameObject panel, bool animate = false)
    {
        panel.SetActive(false);
    }

    public void hideP(string panel)
    {
        GameObject obj = returnPanel(panel);
        hidePanel(obj, true);
    }
    public void showP(string panel)
    {
        GameObject obj = returnPanel(panel);
        showPanel(panel, true,false);
    }

    public void hidePanel(string panelName, bool animate = false,bool dialog=false)
	{
        if (animate)
        {
            doAnimAndSleep anim = returnPanel(panelName, dialog).GetComponent<doAnimAndSleep>();
            if(anim != null)
            {
                anim.doAnim();
            }
            else
            {
                setPanel(panelName, false, false);
            }
            
        }
        else
        {
            setPanel(panelName, false,false);
        }
        
	}
		
}
