using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity;
using UnityEngine.Events;
using UnityEditor;

public enum buttonFunction { changeMenu, startGame, Quit, setPref,Custom,GoBack , OpenWeb, Continue };

// Custom serializable class
[Serializable]
public class ButtonProps
{
    public string name = "Name";
    public buttonFunction onPress = buttonFunction.startGame;
    public string argument;
    public Color32 color = Color.grey;
    public Color32 txtcolor = Color.black;
    public string AC;
    public UnityEvent ev;
}

public class populateButtons : MonoBehaviour
{

    public List<ButtonProps> props;
    public GameObject layoutGroup; // where to place generated buttons
    public GameObject prefab;//button prefab

    void Reset()
    {
        props = new List<ButtonProps>();
        props.Add(new ButtonProps());
        props[0].name = "Button";
        props[0].color = Color.grey;
        layoutGroup = gameObject;
        prefab = ProjectSettings.data.defaultButton;
    }
    // Start is called before the first frame update

    public static Button createButton(ButtonProps props, GameObject buttonObj, bool isPrefab = true, GameObject layoutGroup=null)
    {
        GameObject obj = buttonObj;
        if(isPrefab)
        {
            obj = Instantiate(buttonObj, layoutGroup.transform);
            obj.transform.parent = layoutGroup.transform;
            obj.name = props.name;
           
        }
        Button button = obj.GetComponent<Button>();
        //button.Select();
        obj.AddComponent<UIselectOnEnable>();

        Transform label = obj.transform.Find("Label");
        if(label != null)
        {
            Text t = label.GetComponent<Text>();
            if (t == null)
            {
                TMPro.TextMeshProUGUI txtmesh = label.GetComponent<TMPro.TextMeshProUGUI>();
                txtmesh.text = props.name;
                txtmesh.color = props.txtcolor;
            }
            else
            {
                t.text = props.name;
            }


            Transform center = obj.transform.Find("ButtonCenter");
            if (center != null)
            {
                center.GetComponent<Image>().color = props.color;
            }
        }
        
        switch (props.onPress)
        {
            case buttonFunction.changeMenu:
                button.onClick.AddListener(() => MenuManager.ins.changeMenu(props.argument, props.AC));
                break;

            case buttonFunction.GoBack:
                button.onClick.AddListener(() => MenuManager.ins.goBack());
                break;

            case buttonFunction.startGame:
                button.onClick.AddListener(() => MenuManager.ins.startGame());
                break;

            case buttonFunction.Continue:
                button.onClick.AddListener(() => MenuManager.ins.loadGame());
                break;

            case buttonFunction.Quit:
                button.onClick.AddListener(() => MenuManager.ins.quitGame());
                break;

            case buttonFunction.setPref:
                string[] splitArg = props.argument.Split(':');
                button.onClick.AddListener(() => PlayerPrefs.SetString(splitArg[0], splitArg[1]));
                button.onClick.AddListener(() => MenuManager.ins.processPrefs());
                break;
            case buttonFunction.Custom:
                button.onClick.AddListener(() => props.ev.Invoke());
                if (props.AC == null)
                {
                    button.onClick.AddListener(() => ProjectSettings.data.PlaySound(ProjectSettings.data.menuConfirm));
                }
                break;

            case buttonFunction.OpenWeb:
                button.onClick.AddListener(() => MenuManager.returnInstance().openWeb(props.argument));
                break;


        }

        if (props.AC != "")
        {
            button.onClick.AddListener(() => ProjectSettings.data.PlaySound(props.AC));

        }

        return button;
    }
    void Start()
    {
        GUIutil.clearChildren(layoutGroup.transform, "none", true);
        bool selected = false;
        foreach(ButtonProps b in props)
        {
            Button btn = createButton(b, prefab, true, layoutGroup);
            if (!selected)
            {
                btn.Select();
                selected = true;
                btn.gameObject.AddComponent<UIselectOnEnable>();
            }
        }
    }

    private void OnValidate()
    {
        foreach (Transform child in transform)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if(child!=null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }
            };
        }
        generateButtons();
    }

    void generateButtons()
    {
        bool selected = false;
        foreach (ButtonProps b in props)
        {
            Button btn = createButton(b, prefab, true, layoutGroup);
            if (!selected)
            {
                btn.Select();
                selected = true;
                btn.gameObject.AddComponent<UIselectOnEnable>();
            }
        }
    }
}
