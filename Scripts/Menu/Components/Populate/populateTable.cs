using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[Serializable]
public class TableProps : DataProps
{
    public listFunction onSelect = listFunction.Form;//TODO: convert this to just an intcode that is populated by some script? so it can be used by popTable/popList
    public listObjProps displayObj;

    [Tooltip("Alternate color for odd and even rows")]
    public bool striping = true;
    public bool sortable = true;
    public bool showHeader = true;
}

public class populateTable : populateData, I_ItemMenu
{

    public new TableProps props;

    private Dictionary<string, object> preservedData = new Dictionary<string, object>();

    public override void Populate()
    {

        bool selected = false;

        Clear();

        DataSource d;
        string primaryKey = "";
        {
            primaryKey = props?.data?.primaryKey;
            d = props?.data;
        }
        bool selectedAnItem = false;
        List<string> keys = d.getFieldFromAllItems(primaryKey);

        IEnumerable<SourceFilter> allFilters = filters.Concat(permanentFilter);

        if (d.displayCode != null)
        {
            displayCode = d.displayCode;
        }
        if(props.displayObj!=null)
        {
            UIDisplayCodeController dc = props.displayObj.GetComponent<UIDisplayCodeController>();
            if (dc != null)
            {
                dc.displayCode = displayCode;
            }
        }
        

        foreach (string key in keys)
        {
            bool filtered = false;
            foreach (SourceFilter filter in allFilters)
            {
                if (key == "0" && showEmptyItem) { continue; }
                string value = d.getFieldFromItemID(key, filter.filterVar);
                if (!filter.MatchesFilter(value))
                {
                    filtered = true;
                }
            }
            if (filtered) { continue; }

            GameObject obj = Instantiate(prefab, layoutGroup.transform);
            obj.transform.parent = layoutGroup.transform;
            obj.name = key;
            UIRowListItem listItem = obj.GetComponent<UIRowListItem>();
            d.AddListener(key, listItem.SourceUpdate);
            DataLibrary dat = new DataLibrary(props.data.getObjFromItemID(key));
            foreach (KeyValuePair<string, object> de in preservedData)
            {
                dat.SetValue(de.Key, de.Value);
            }
            listItem.SetData(dat);
            listItem.Click = OnClick;
            listItem.source = props.data;
            if (!selectedAnItem)
            {
                if (selectDefaultItem && (defaultSelection == -1 || key == defaultSelection.ToString()))
                {
                    listItem.Click?.Invoke(listItem);
                    selectedAnItem = true;
                }
            }

        }

    }

    public void OnClick(UIButtonListItem listItem)
    {
        props.evData.Invoke(listItem.GetData());
    }

}