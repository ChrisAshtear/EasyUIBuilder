using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataType {  XML, SQL,JSON,Choice, Web}; // we need custom handlers for all these.

public enum fieldType { str, integer, flt };
public class DataSource : ScriptableObject
{
    public string sourceName;
    public string addressOfData;
    protected bool dataReady = false;
    public DataType type;

    protected List<Dictionary<string, string>> data;
    public DataSource()
    {
        //condition = maxCond;
    }

    private void OnEnable()
    {

    }

    public virtual void LoadData()
    {

    }

    public virtual Dictionary<string,fieldType> getFields()
    {
        return new Dictionary<string, fieldType>();
    }


    public virtual List<string> getFieldSimple()
    {
        return new List<string>();
    }

    public virtual List<string> getFieldFromAllItems(string field)
    {
        return new List<string>();
    }

    public virtual string getFieldFromItemID(int id, string field)
    {
        return "";
    }


}
