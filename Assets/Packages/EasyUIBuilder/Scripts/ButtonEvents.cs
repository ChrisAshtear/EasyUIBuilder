using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public ButtonProps props;
    void Start()
    {
        populateButtons.createButton(props, gameObject, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
