using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.TextCore;
/// <summary>
// Display a projectValue in a gui element.
/// </summary>


public class displayProjVal : MonoBehaviour
{

    private Text txt;
    private TMPro.TextMeshProUGUI txtmesh;
    public string var;
    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txtmesh = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        //autosize text element for scrolling?
    }

    // Update is called once per frame
    void Update()
    {
        if(projectHandler.loaded)
        {
            if(txt != null)
            {
                txt.text = projectHandler.returnStr(var).ToString();
            }
            if(txtmesh != null)
            {
                txtmesh.text = projectHandler.returnStr(var).ToString();
            }
            
            this.enabled = false;
        }
    }

}
