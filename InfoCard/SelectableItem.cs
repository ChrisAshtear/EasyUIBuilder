using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SelectableItem : MonoBehaviour
{
    public Image highlight;
    public SpriteRenderer highlightSpr;
    public UnityEvent e_pointerDown = new UnityEvent();

    public I_ItemMenu parentList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseEnter()
    {
        Debug.Log("WTF");
        //onPointerEnter();
    }

    public void onPointerEnter()
    {
        setHighlight(true);
    }

    public void setHighlight(bool enable)
    {
        if (highlight != null)
        {
            highlight.enabled = enable;
        }
        if (highlightSpr != null)
        {

            highlightSpr.enabled = enable;
        }
    }

    public void onPointerExit()
    {
        if(getSelected() != gameObject)
        {
            setHighlight(false);
            displayObjDetails obj = gameObject.GetComponent<displayObjDetails>();
            if(obj!=null)
            {
                obj.OnMouseExit();
            }
        }
    }

    public void setSelected()
    {
        if (parentList == null)
        {
            MenuManager.ins.selectedItem = gameObject;
        }
        else if (parentList != null)
        {
            parentList.SetSelected(gameObject);
        }
    }

    public GameObject getSelected()
    {
        if(parentList== null && MenuManager.ins.selectedItem != null)
        {
            return MenuManager.ins.selectedItem;
        }
        else if(parentList!= null && parentList.GetSelected() != null)
        {
            return parentList.GetSelected();
        }
        else
        {
            return null;
        }
    }

    public void selectItem()
    {
        GameObject curSelected = getSelected();

        if(curSelected != null)
        {
            SelectableItem old = curSelected.GetComponent<SelectableItem>();
            setSelected();
            old.Invoke("onPointerExit", 0.1f);
        }

        setSelected();
        setHighlight(true);


        e_pointerDown.Invoke();
    }

}
