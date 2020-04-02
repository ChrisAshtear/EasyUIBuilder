using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDropEquipment : MonoBehaviour
{
    public Image highlight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onPointerEnter()
    {
        if(GameManager.draggingObject)
        {
            highlight.enabled = true;
        }
    }

    public void onPointerExit()
    {
        highlight.enabled = false;
    }

    public void DropToCargo()
    {
        if(GameManager.draggingObject)
        {
            GenericDataHandler data = GameManager.draggedObject.GetComponent<GenericDataHandler>();
            /*Entity.Ship ship = FLFlight.Ship.PlayerShip.ShipEntity;
            if(data != null)
            {
                int slotIdx = int.Parse(data.getData("slotIdx"));

                equipmentSlot slot = ship.getEquipmentSlot(slotIdx);
                ship.AddCargo(slot.equippedItem,1,slot.slotType);
                FLFlight.Ship.PlayerShip.unequipSlot(slotIdx);
            }*/
        }
    }

    public void DropToEquip()
    {
        if (GameManager.draggingObject)
        {
            GenericDataHandler data = GameManager.draggedObject.GetComponent<GenericDataHandler>();
            GenericDataHandler thisSlot = GetComponent<GenericDataHandler>();
            /*Entity.Ship ship = FLFlight.Ship.PlayerShip.ShipEntity;
            if (data != null)
            {
                int slotIdx = int.Parse(thisSlot.getData("slotIdx"));

                string thisSlotType = thisSlot.getData("slotType");
                string draggedType = data.getData("slotType");

               

                string uuid = data.getData("equippedItem.UUID");

                if(thisSlotType == draggedType)
                {
                    FLFlight.Ship.PlayerShip.equipSlot(uuid, slotIdx);
                    ship.RemoveCargo(uuid, 1);
                }

                
                
            }*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
