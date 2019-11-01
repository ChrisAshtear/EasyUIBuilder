using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity;
using UnityEngine.Events;
using UnityEditor;

public enum buttonFunction { changeMenu, startGame, Quit, setPref,Custom,GoBack };

// Custom serializable class
[Serializable]
public class ButtonProps
{
    public string name;
    public buttonFunction onPress;
    public string argument;
    public Color32 color = new Color32(0,0,0,255);
    public AudioClip AC;
    public UnityEvent ev;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ButtonProps))]
public class IngredientDrawer : PropertyDrawer
{
    private float xOffset = 0;
    private float yHeight = 16;
    private float expandedHeight = 150;
    // Draw the property inside the given rect

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty p = property.FindPropertyRelative("onPress");
        if (p.intValue == 4)//custom prop
        {
            return expandedHeight;
        }
        else
        {
            return yHeight;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        xOffset = position.x;
        // Calculate rects
        var amountRect = getRect(100,position);
        var unitRect = getRect(100, position);
        var colorRect = getRect(50, position);
        var nameRect = getRect(200, position);
        var acRect = getRect(100, position);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        SerializedProperty p = property.FindPropertyRelative("onPress");
        if(p.intValue == 4)//custom prop
        {
            var eventRect = new Rect(position.x, position.y+yHeight, position.width, position.height-yHeight);
            position.height = 200;
            EditorGUI.PropertyField(eventRect, property.FindPropertyRelative("ev"), GUIContent.none);

        }
        else if(p.intValue == 0 || p.intValue == 3)
        {
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("argument"), GUIContent.none);
        }

        EditorGUI.PropertyField(acRect, property.FindPropertyRelative("AC"), GUIContent.none);
        

        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("color"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("onPress"), GUIContent.none);
        

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public Rect getRect(int size, Rect position)
    {
        Rect newR = new Rect(xOffset, position.y, size, yHeight);
        xOffset += size;
        return newR;
    }
}
#endif

public class populateButtons : MonoBehaviour
{
    //array string name of buttons
    //array event , func to call on press


    //dictionary, name->event to call
    //dictionary name-> color for button center

    //game object ref for layout group.

    public ButtonProps[] props;
    public GameObject layoutGroup; // where to place generated buttons
    public GameObject prefab;//button prefab

    // Start is called before the first frame update
    void Start()
    {
        bool selected = false;
        foreach(ButtonProps b in props)
        {

            GameObject obj = Instantiate(prefab, layoutGroup.transform);
            obj.transform.parent = layoutGroup.transform;
            obj.name = b.name;
            Button button = obj.GetComponent<Button>();
            if(!selected)
            {
                button.Select();
                selected = true;
                obj.AddComponent<UIselectOnEnable>();
            }
            
            obj.transform.Find("ResumeText").GetComponent<Text>().text = b.name;
            obj.transform.Find("ButtonCenter").GetComponent<Image>().color = b.color;
            switch(b.onPress)
            {
                case buttonFunction.changeMenu:
                    button.onClick.AddListener(() => MenuManager.returnInstance().changeMenu(b.argument));
                    break;

                case buttonFunction.GoBack:
                    button.onClick.AddListener(() => MenuManager.returnInstance().goBack());
                    break;

                case buttonFunction.startGame:
                    button.onClick.AddListener(() => MenuManager.returnInstance().startGame());
                    break;

                case buttonFunction.Quit:
                    button.onClick.AddListener(() => MenuManager.returnInstance().quitGame());
                    break;

                case buttonFunction.setPref:
                    string[] splitArg = b.argument.Split(':');
                    button.onClick.AddListener(() => PlayerPrefs.SetString(splitArg[0],splitArg[1]));
                    break;
                case buttonFunction.Custom:
                    button.onClick.AddListener(()=>b.ev.Invoke());
                    break;



            }

            if (b.AC != null)
            {
                obj.GetComponent<AudioSource>().clip = b.AC;
                button.onClick.AddListener(() => obj.GetComponent<AudioSource>().Play());

            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
