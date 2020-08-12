using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class listObjProps : MonoBehaviour
{
    public string[] valsList; // level vals
    public string[] saveValsList;
    public GameObject prefabName;
    public GameObject prefabDesc;
    public GameObject prefabHeader;
    public GameObject prefabSep;
    public Texture2D iconSheet;
    public string Name;

    void Start()
    {
        Invoke("list", 2);
    }

    public void list()
    {
        Animator anim = GetComponent<Animator>();
        //createText("Name", Name);

        
        //also display best times.
    }

    public void resetVals()
    {
        Transform t = transform;
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform child in t)
        {
            objs.Add(child.gameObject);
        }
        foreach (GameObject c in objs)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(c);
            }
            else
            {
                Destroy(c);
            }
        }
        list();
    }

    public void createText(string text, string value="")
    {
        GameObject obj = (GameObject)Instantiate(prefabName,transform.position, Quaternion.identity,transform);


        Text txt = obj.transform.Find("Name").GetComponent<Text>();
        txt.text = text + ": ";
        Text val = obj.transform.Find("Value").GetComponent<Text>();
        val.text = value;
    }

    public void createDescText(string text)
    {
        GameObject obj = (GameObject)Instantiate(prefabDesc, transform.position, Quaternion.identity,transform);
        obj.transform.parent = transform;

        Text txt = obj.GetComponentInChildren<Text>();
        txt.text = text;
    }

    public void createHeader(int iconIdx,string text)
    {
        GameObject obj = (GameObject)Instantiate(prefabHeader, transform.position, Quaternion.identity, transform);
        Sprite[] sprites = Resources.LoadAll<Sprite>("GUIicons/" + iconSheet.name); 
        Text txt = obj.GetComponentInChildren<Text>();
        txt.text = text;
        Image img = obj.GetComponentInChildren<Image>();
        img.sprite = sprites[iconIdx];
    }

    public void createSeperator()
    {
        GameObject obj = (GameObject)Instantiate(prefabSep, transform.position, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
