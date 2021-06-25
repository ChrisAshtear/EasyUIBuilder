using UnityEngine;
using System.Collections;

public class UICheck : MonoBehaviour
{
    public UIDialogBox dialogObj;
    public DialogProps dialogProps;
    public int successIndex;
    public bool showDialog = false;

    public void DoCheck()
    {
        if(showDialog)
        {
            dialogObj.ShowDialog(dialogProps);
        }
        else
        {
            dialogProps.options[successIndex].OnChooseOption.Invoke();
        }

        
    }
}
