using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateDDFromData : MonoBehaviour
{
    // Start is called before the first frame update
    public DataSource inputSource;
    public string inputFieldName;

    int id;

    public DataSource targetSource;
    public string targetFieldName;
    public listObjProps detailsObj;
    public populateDropDowns targetPopulate;
    public string labelName;
    

    void Start()
    {
        
    }


    public void Generate(selectData s)
    {

        //get number of items to generate

        Dictionary<string, string> opts = inputSource.getFieldFromAllItemsKeyed("Name");
        string chosenVal = s.drop.options[s.drop.value].text;
        string key = opts[chosenVal];


        string result = inputSource.getFieldFromItemID(key,inputFieldName);

        int numSlots = 0;
        bool wasANum = int.TryParse(result, out numSlots);

        if(wasANum)
        {
            targetPopulate.Clear();
            for (int i = 0; i< numSlots; i++)
            {
                DropDownProps prop = new DropDownProps();
                prop.data = targetSource;
                prop.field = targetFieldName;
                prop.label = labelName;
                if (numSlots > 1)
                {
                    prop.label += " " + (i + 1);
                }
                
                prop.displayObj = detailsObj;
                prop.onSelect = dropdownFunction.showDetails;
                targetPopulate.props.Add(prop);

            }
            
            targetPopulate.Populate();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
