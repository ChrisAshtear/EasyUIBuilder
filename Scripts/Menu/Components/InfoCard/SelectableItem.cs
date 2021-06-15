using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectableItem : MonoBehaviour
{
    public GameObject highlight;
    public UnityEvent e_pointerDown = new UnityEvent();
    public I_ItemMenu parentList;

    private TMPro.TextMeshProUGUI text;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        text = highlight.GetComponent<TMPro.TextMeshProUGUI>();
        image = highlight.GetComponent<Image>();

        if (parentList == null) { findParent(); }
    }

    public void onPointerEnter()
    {
        setHighlight(true);
    }

    public void setHighlight(bool enable)
    {
        if(text != null)
        {
            text.enabled = enable;
        }
        if(image != null)
        {
            image.enabled = enable;
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

    public void findParent()
    {
        I_ItemMenu menu = GetComponentInParent<I_ItemMenu>();
        if (menu != null) { parentList = menu; }
    }

    public void setSelected()
    {
        if (parentList == null)
        {
            findParent();
            parentList?.SetSelected(gameObject);
        }
        else if (parentList != null)
        {
            parentList.SetSelected(gameObject);
        }
    }

    public GameObject getSelected()
    {
        if(parentList!= null && parentList.GetSelected() != null)
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
