using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayGMval : MonoBehaviour
{
    Text textfield;
    public string valueToDisplay;
    public float updateTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        textfield = GetComponent<Text>();
        InvokeRepeating("updateField", updateTime, updateTime);
    }

    // Update is called once per frame
    void updateField()
    {
        textfield.text = GameManager.ins.getUIVal(valueToDisplay);
    }
}
