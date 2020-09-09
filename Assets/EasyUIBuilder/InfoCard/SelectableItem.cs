using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableItem : MonoBehaviour
{
    public Image highlight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onPointerEnter()
    {
         highlight.enabled = true;
    }

    public void onPointerExit()
    {
        if(MenuManager.ins.selectedItem != gameObject)
        {
            highlight.enabled = false;
        }
    }

    public void selectItem()
    {
        if(MenuManager.ins.selectedItem != null)
        {
            SelectableItem old = MenuManager.ins.selectedItem.GetComponent<SelectableItem>();
            MenuManager.ins.selectedItem = gameObject;
            old.Invoke("onPointerExit", 0.1f);
        }
        
        MenuManager.ins.selectedItem = gameObject;
        highlight.enabled = true;
    }

}
