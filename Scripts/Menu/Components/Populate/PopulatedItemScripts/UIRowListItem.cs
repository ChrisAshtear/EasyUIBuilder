using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
[RequireComponent(typeof(UIDataController))]
public class UIRowListItem : UIButtonListItem
{

    [Tooltip("Container Object for created Text objects")]
    public GameObject containerObject;
    public GameObject RowCellPrefab;
    //TODO: need to change bgcolor + txtcolor automatically 
    public override void PreDataUpdate(IDataLibrary data)
    {
        foreach(IData d in data)
        {
            IData dat = (IData)d;
            switch (dat.Type)
            {//TODO: maybe also add a datacontrol tag here
                case DataLibSupportedTypes.text:
                    GameObject g = Instantiate(RowCellPrefab, containerObject.transform, false);
                    TextMeshProUGUI text = g.GetComponent<TextMeshProUGUI>();
                    text.text = dat.DisplayValue;
                    break;
            }
        }
        //create components.
    }

}
