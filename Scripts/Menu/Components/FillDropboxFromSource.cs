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
    public string labelText = "";

    Text label;

    public string comparisonVal;

    GenericDataHandler dataHandler;
    displayObjDetails display;

    public listObjProps displayObj;
    // Start is called before the first frame update
    public List<string> optionKeys;
    //SHOW # items. adjust how big viewport is.
    //Font to use. Maybe not for this particular script but in the prefab.

    public void populateButtons()
    {

        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        
        //need to pair id to value when selecting.
        Dictionary<string,string> opts= data.getFieldFromAllItemsKeyed(chosenField,true,true);
        //options.Add("");//add extra at bottom because dropdown hides it. 
        optionKeys = opts.Values.ToList();
        dropdown.AddOptions(opts.Keys.ToList());
    }

    private void Start()
    {
        dataHandler = GetComponent<GenericDataHandler>();
        display = GetComponent<displayObjDetails>();

        initData();
    }

    public void initData()
    {
        label = gameObject.transform.Find("Label").GetComponent<Text>();
        label.enabled = showLabel;

        if (showLabel && labelText == "")
        {
            label.text = chosenField;
        }
        else
        {
            label.text = labelText;
        }

        if (data.isDataReady())
        {
            populateButtons();
        }

        display = GetComponent<displayObjDetails>();
    }

    private void OnLevelWasLoaded(int level)
    {
       // data.onDataReady += populateButtons;
       if(data.isDataReady())
        {
            populateButtons();
        }
    }
    


    public void selectedVal(int chosen)
    {
        //should store all keys instead of doing a search like this
        Dictionary<string, string> opts = data.getFieldFromAllItemsKeyed(chosenField);
        string chosenVal = dropdown.options[chosen].text;
        string key = opts[chosenVal];
        Debug.Log("Chose " + chosenVal + ": " + key);

        Dictionary<string, object> dat = data.getFieldsFromItemID(key);

        string output = "";
        foreach (KeyValuePair<string, object> entry in dat)
        {

            output += entry.Key + " : " + entry.Value + "\n";


        }
        detailsPane.text = output;

    }

    public void displayValFields(int chosen)
    {
        GenericDataHandler datah = GetComponent<GenericDataHandler>();
        listObjProps uiObject = displayObj;

        string key = this.optionKeys[chosen];


        Dictionary<string, object> dat = data.getFieldsFromItemID(key);

        uiObject.resetVals();


        dataHandler.setData(data.getFieldsFromItemID(key));
        display.uiObject = displayObj;
        display.displayCode = data.displayCode;
        display.parseFields();

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