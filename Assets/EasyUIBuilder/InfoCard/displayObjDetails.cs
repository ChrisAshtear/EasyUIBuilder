using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayObjDetails : MonoBehaviour
{
    public listObjProps uiObject;
    public Dictionary<string, string> props; // new props type
    GenericDataHandler data;

    [TextArea(3, 10)]
    public string displayCode;
    //display code contained in DATA file
    //something should be done for an instance of a data type:
    //ie new stats would be in data file
    //changing item stats like condition would also have to be displayed

    public string comparisonVal;

    public Dictionary<string,string> comparisons;

    public bool alwaysVisible = false;
    public bool followCursor = true;

    public float updateTick = 0.5f;

    public GameObject selectedSpr;



    //USE STRING TO HANDLE WHAT TO DISPLAY!!!!! 
    //ex:
    //equippedItem.RateOfFire
    //equippedItem.ProjectileSpeed


    // Start is called before the first frame update
    private void Reset()
    {
        GenericDataHandler h = GetComponent<GenericDataHandler>();
        if(h == null)
        {
            gameObject.AddComponent<GenericDataHandler>();
        }
    }

    void Start()
    {
        comparisons = new Dictionary<string, string>();
        data = GetComponent<GenericDataHandler>();
        if(uiObject == null)
        {
            uiObject = MenuManager.ins.detailsCard.GetComponentInChildren<listObjProps>();
        }

        Invoke("Init", 0.1f);
        InvokeRepeating("uiUpdate", updateTick, updateTick);
    }

    void Init()
    {

        //gameObject.GetComponentInChildren<Text>().text = ingName;

    }

    public virtual void OnMouseDown()
    {
        
        MenuManager.ins.selectedDetails = this;
        uiObject.transform.parent.gameObject.SetActive(true);
        //uiObject.Name = ingName;
        uiObject.resetVals();
        //selectedSpr.SetActive(true);
        parseFields();

    }

    public void parseFields()
    {
        if(displayCode.Length <=1)
        {
            return;
        }
        string[] lines = displayCode.Split('\n');
        comparisons.Clear();
        string currentComparison = "";
        comparisons.Add("none", "");
        foreach (string line in lines)
        {
            if(line[0] == '#')
            {
                string[] headerFields = line.Split('|');
                headerFields[0] = headerFields[0].Substring(1);
                uiObject.createHeader(int.Parse(data.getData(headerFields[0])), data.getData(headerFields[1]));
                continue;
            }
            if(line[0] == '=')
            {
                uiObject.createSeperator();
                continue;
            }
            if (line[0] == '[')
            {
                currentComparison = line.Substring(1, line.Length - 2);
                comparisons.Add(currentComparison, "");
            }
            else if(currentComparison != "")
            {
                comparisons[currentComparison] += line + '\n';
            }
            else
            {
                comparisons["none"] += line + '\n';
            }
        }

        foreach (KeyValuePair<string, string> entry in comparisons)
        {
            if(entry.Value == "")
            {
                continue;
            }
            string value = entry.Value.Substring(0, entry.Value.Length - 1);
            if (entry.Key == data.getData(comparisonVal) || entry.Key == "none")
            {
                string[] nlines = value.Split('\n');

                foreach (string line in nlines)
                {
                    string[] fields = line.Split('|');
                    if(fields.Length < 2)
                    {
                        uiObject.createDescText(data.getData(fields[0]));
                    }
                    else
                    {
                        uiObject.createText(fields[0], data.getData(fields[1]));
                    }
                    
                }
            }
        }

    }

    public void OnMouseExit()
    {
        if(!alwaysVisible)
        {
            uiObject.transform.parent.gameObject.SetActive(false);
        }
        
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }


    void uiUpdate()
    {
        if (MenuManager.ins.selectedDetails == this)
        {
            uiObject.resetVals();
            parseFields();
        }
        else
        {
            //selectedSpr.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(followCursor)
        {
            Vector3 pos = Input.mousePosition;

            pos.y += 150;
            pos.x += 300;

            uiObject.transform.parent.position = pos;
        }
        
    }
}
