using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupUIObj : MonoBehaviour
{
    public Vector2 offset;

    private Vector3 originalPosition;

    public GameObject uiObject;

    private bool grabbed = false;

    private GenericDataHandler data;

    public string comparisonData;
    public string comparisonResult;

    public bool noComparison = false;

    Transform previousParent;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = uiObject.transform.position;
        data = GetComponent<GenericDataHandler>();
    }

    public void OnMouseDown()
    {
        if (noComparison || data.getData(comparisonData) != comparisonResult || data.getData("equippedItem.Name") != "None")
        {
            previousParent = transform.parent;
            transform.parent = GameObject.Find("UI").transform;
            grabbed = true;
            GameManager.draggingObject = true;
            GameManager.draggedObject= this.gameObject;
        }
        
    }

    public void OnMouseUp()
    {
        grabbed = false;
        uiObject.transform.position = originalPosition;
        transform.parent = previousParent;

        Invoke("resetDrag", 0.15f);
    }

    public void resetDrag()
    {
        GameManager.draggingObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(grabbed)
        {
            Vector3 pos = Input.mousePosition;

            pos.y += offset.y;
            pos.x += offset.x;

            uiObject.transform.position = pos;
        }
        

    }
}
