using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class GenericDataHandler : MonoBehaviour
{
    object dataSource;
    Dictionary<string, object> fields;

    //for real time sources
    DataSource source = null;
    string index = "";
    
    // Start is called before the first frame update
    void Start()
    {
        fields = new Dictionary<string, object>();
    }


    public void setDataForUpdate(string index, DataSource source)
    {

    }

    public static Dictionary<string,object> objectToDict(object obj)
    {
        Dictionary<string, object> fields = new Dictionary<string, object>();
        if (obj is IDictionary)
        {
            Dictionary<string, object> dict = (Dictionary<string, object>)obj;
            fields = dict.ToDictionary(k => k.Key, k => (object)k.Value);
        }
        else
        {
            fields = obj.GetType().GetFields().ToDictionary(prop => prop.Name, prop => prop.GetValue(obj));
        }
        return fields;
    }
    public void setData(object dataSource,string index="",DataSource source = null)
    {
        this.dataSource = dataSource;
        if(this.source != null && this.index != null) // RT source has been set.
        {
            dataSource = this.source.getFieldObjsFromItemID(this.index);
        }
        fields = GenericDataHandler.objectToDict(dataSource);
        if(source != null && index != "")
        {
            this.source = source;
            this.index = index;
        }
    }


    public string getData(string field, string index="")//index is for looking up from a data source.
    {
        return getDataObj(field, index).ToString();
        
    }

    public object getDataObj(string field, string index="")
    {
        object outp = new object();
        Dictionary<string, object> subFieldVars;

        setData(dataSource); // need to refresh data
        //Make more efficient


        if (index != "")
        {
            DataSource d = (DataSource)fields["data"];
            return d.getFieldFromItemID(index, field);
        }

        string[] splitFields = field.Split('.');
        string fieldToSearch = splitFields[0];

        if (splitFields.Length > 1)
        {
            string subField = splitFields[1];
            field.Split('.');
            object result = new object();
            fields.TryGetValue(fieldToSearch, out result);
            if (result != null)
            {
                subFieldVars = result.GetType().GetFields().ToDictionary(prop => prop.Name, prop => prop.GetValue(fields[fieldToSearch]));
                return subFieldVars[subField].ToString();
            }
            else
            {
                return "None";
            }


        }

        fields.TryGetValue(fieldToSearch, out outp);
        if (outp != null)
        {
            return outp;
        }
        else
        {
            return "null";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
