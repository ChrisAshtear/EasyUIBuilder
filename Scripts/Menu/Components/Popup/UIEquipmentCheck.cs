using UnityEngine;
using System.Collections;

public class UIEquipmentCheck : MonoBehaviour
{
   // public PlayersCarDisplay car;
    public UIDialogBox dialogObj;
    public DialogProps dialogProps;
    public int successIndex;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoCheck()
    {
      //  EquipmentController controller = car.SpawnedCar.GetComponent<EquipmentController>();
        if(false)
        {
            dialogObj.ShowDialog(dialogProps);
        }
        else
        {
            dialogProps.options[successIndex].OnChooseOption.Invoke();
        }

        
    }
}
