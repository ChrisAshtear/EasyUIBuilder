using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using System;
/// <summary>
/// This populates the contents of a dropbox with a number of elements in a data collection.
/// </summary>

public class FillDropboxFromSource : MonoBehaviour
{

    private Dropdown dropdown;
    public Text detailsPane;
    public DataSource data;
    public string chosenField;
    public bool showLabel = true;

    Text label;
    // Start is called before the first frame update


    public void populateButtons()
    {

        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();

        List<string> options = data.getFieldFromAllItems(chosenField);
        //need to pair id to value when selecting.

        options.Add("");//add extra at bottom because dropdown hides it. 

        dropdown.AddOptions(options);
    }

    private void Start()
    {
        label = gameObject.transform.Find("Label").GetComponent<Text>();
        label.enabled = showLabel;

        if (showLabel)
        {
            label.text = chosenField;
        }

        if (data.isDataReady())
        {
            populateButtons();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
       // data.onDataReady += populateButtons;
       if(data.isDataReady())
        {
            populateButtons();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateUI()
    {
        //getDataForUI.ins.dropdownSelected(this.name, dropdown.value.ToString());
    }

}