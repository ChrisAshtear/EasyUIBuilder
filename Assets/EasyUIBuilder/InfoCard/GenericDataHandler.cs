using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class GenericDataHandler : MonoBehaviour
{
    object dataSource;
    Dictionary<string, object> fields;
    
    // Start is called before the first frame update
    void Start()
    {
        fields = new Dictionary<string, object>();
    }

    public void setData(object dataSource)
    {
        this.dataSource = dataSource;
        if(dataSource is IDictionary)
        {
            Dictionary<string, string> dict = (Dictionary<string, string>)dataSource;
            fields = dict.ToDictionary(k => k.Key, k => (object)k.Value);
        }
        else
        {
            fields = dataSource.GetType().GetFields().ToDictionary(prop => prop.Name, prop => prop.GetValue(dataSource));
        }
    }

    public string getData(string field)
    {
        object outp = new object();
        Dictionary<string, object> subFieldVars;
        //if (fields.Count == 0)
        //{
            setData(dataSource); // need to refresh data
        //}
        string[] splitFields = field.Split('.');
        string fieldToSearch = splitFields[0];
        
        if(splitFields.Length > 1)
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
            return outp.ToString();
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
