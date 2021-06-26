using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity;
using UnityEngine.Events;
using UnityEditor;

//public enum buttonFunction { changeMenu, startGame, Quit, setPref, Custom, GoBack, OpenWeb, Continue };

// Custom serializable class
[Serializable]
public class TabProps
{
    public string name = "Name";
    //public buttonFunction onPress = buttonFunction.startGame;
    //public string argument;
    public Sprite tabIcon;//support text or image tab icons
    public Color32 color = Color.grey;
    public Color32 txtColor = Color.grey;
    public Color32 selectedColor = Color.black;
    public ProjectSoundClip AC;
    public GameObject panelPrefab;
    public bool startSelected = false;
    //public UnityEvent ev;
}

public class populateTabs : MonoBehaviour
{
    //support seperators
    public List<TabProps> tProps;
    public GameObject tabButtonPrefab;
    public GameObject tabContainer; // where to place generated buttons
    public ToggleGroup contentContainer;//button prefab

    void Reset()
    {
        /*props = new List<ButtonProps>();
        props.Add(new ButtonProps());
        props[0].name = "Button";
        props[0].color = Color.grey;*/
    }
    // Start is called before the first frame update

    public static GameObject createTabPanel(TabProps props,GameObject contentContainer)
    {
        GameObject tabObj = Instantiate(props.panelPrefab, contentContainer.transform);
        tabObj.transform.parent = contentContainer.transform;
        tabObj.name = props.name;
        return tabObj;

    }

    public static Toggle createTabButton(TabProps props, GameObject buttonObj, GameObject tabContainer, GameObject tabPanel,ToggleGroup group)//associate tab button with tab panel.
    {
        GameObject obj = buttonObj;
        obj = Instantiate(buttonObj, tabContainer.transform);
        obj.transform.parent = tabContainer.transform;
        obj.name = props.name;

        Toggle tog = obj.GetComponent<Toggle>();
        tog.group = group;
        //maybe use datalib + datacontroller to auto fill this in the prefab
        Transform label = obj.transform.Find("Label");
        if (label != null)
        {
            Text t = label.GetComponent<Text>();
            if (t == null)
            {
                TMPro.TextMeshProUGUI txtmesh = label.GetComponent<TMPro.TextMeshProUGUI>();
                txtmesh.text = props.name;
                txtmesh.color = props.txtColor;
            }
            else
            {
                t.text = props.name;
            }
        }
        Transform img = obj.transform.Find("Image");
        if (img != null)
        {
            Image image = img.GetComponent<Image>();
            image.sprite = props.tabIcon;
            image.color = props.color;
            Text t = label.GetComponent<Text>();
        }
        /*
        switch (props.onPress)
        {
            case buttonFunction.changeMenu:
                button.onClick.AddListener(() => MenuManager.ins.changeMenu(props.argument));
                button.onClick.AddListener(() => ProjectSettings.data.PlaySound(props.AC.audioName));
                break;

        }
        */
        //tog.onValueChanged.AddListener(delegate { tabPanel.; });
        tog.onValueChanged.AddListener(delegate { tabPanel.SetActive(tog.isOn); });
        //add 2nd event for animation
        if (props.AC.audioName != "")
        {
           // tog.onValueChanged.AddListener(delegate { ProjectSettings.data.PlaySound(props.AC.audioName); });
        }

        return tog;
    }
    void Start()
    {
        GUIutil.clearChildren(tabContainer.transform, "none", true);
        GUIutil.clearChildren(contentContainer.transform, "none", true);
        generateContent();
    }

    private void OnValidate()
    {
        if (tabContainer == null || contentContainer == null) { return; }
        foreach (Transform child in tabContainer.transform)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (child != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }
            };
        }
        foreach (Transform child in contentContainer.transform)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (child != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }
            };
        }
        generateContent();
    }

    void generateContent()
    {
        if(contentContainer == null || tabContainer == null || tabButtonPrefab == null) { return; }
        bool selected = false;
        foreach (TabProps b in tProps)
        {
            if(b.panelPrefab == null) { continue; }
            GameObject tabPanel = createTabPanel(b, contentContainer.gameObject);
            if (!b.startSelected) { tabPanel.SetActive(false); }
            Toggle tog = createTabButton(b, tabButtonPrefab, tabContainer, tabPanel, contentContainer);
            if (!selected)
            {
                tog.Select();
                selected = true;
                tog.gameObject.AddComponent<UIselectOnEnable>();
            }
        }
    }
}
