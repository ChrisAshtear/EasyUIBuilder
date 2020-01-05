﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
// Display a projectValue in a gui element.
/// </summary>


public class displayProjVal : MonoBehaviour
{

    private Text txt;
    public string var;
    // Use this for initialization
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(projectHandler.loaded)
        {
            txt.text = projectHandler.returnStr(var).ToString();
            this.enabled = false;
        }
    }

    private void Awake()
    {

    }
}
