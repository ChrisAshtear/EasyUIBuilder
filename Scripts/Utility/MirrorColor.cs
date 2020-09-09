using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MirrorColor : MonoBehaviour
{
    public Button mirrorButton;
    Selectable targetSelectable;
    Image targetImage;
    // Start is called before the first frame update
    void Start()
    {
        targetImage = GetComponent<Image>();
        targetSelectable = GetComponent<Selectable>();
        if(targetSelectable != null)
        {
            targetSelectable.colors = mirrorButton.colors;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       /* ColorBlock colors = mirrorButton.colors;
        if(mirrorButton.)
        var targetColor =
            state == SelectionState.Disabled ? colors.disabledColor :
            state == SelectionState.Highlighted ? colors.highlightedColor :
            state == SelectionState.Normal ? colors.normalColor :
            state == SelectionState.Pressed ? colors.pressedColor :
            state == SelectionState.Selected ? colors.selectedColor : Color.white;

            targetImage.CrossFadeColor(targetColor, 0f,colors.fadeDuration, true, true);*/
    }
}
