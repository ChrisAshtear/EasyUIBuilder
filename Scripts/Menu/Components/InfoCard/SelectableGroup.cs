using UnityEngine;
using System.Collections;

public class SelectableGroup : MonoBehaviour, I_ItemMenu
{
    public SelectableItem defaultSelection;
    private GameObject selectedItem;

    private void Start()
    {
        defaultSelection.Init();
        defaultSelection.selectItem();
    }

    public void SetSelected(GameObject obj)
    {
        selectedItem = obj;
    }

    public GameObject GetSelected()
    {
        return selectedItem;
    }
}
