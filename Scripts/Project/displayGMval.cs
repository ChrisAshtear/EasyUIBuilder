using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayGMval : MonoBehaviour
{
    Text textfield;
    private TMPro.TextMeshProUGUI txtmesh;
    public string valueToDisplay;
    public float updateTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        textfield = GetComponent<Text>();
        txtmesh = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        InvokeRepeating("updateField", updateTime, updateTime);
    }

    // Update is called once per frame
    public void updateField()
    {
        if (textfield != null)
        {
            textfield.text = GameManager.ins.getUIVal(valueToDisplay);
        }
        if (txtmesh != null)
        {
            txtmesh.text = GameManager.ins.getUIVal(valueToDisplay);
        }
        
    }

    private void OnEnable()
    {
        updateField();
    }

    private void Awake()
    {
        updateField();
    }
}
