using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ButtonProps))]
public class ButtonDrawer : PropertyDrawer
{
    private float xOffset = 0;
    private float yHeight = 32;
    private float expandedHeight = 50;//extra space for event control +/- buttons
    // Draw the property inside the given rect

    // Have we loaded the prefs yet
    private static bool prefsLoaded = false;

    // The Preferences
    public static bool boolPreference = false;

    // Add preferences section named "My Preferences" to the Preferences window
    [PreferenceItem("My Preferences")]
    public static void PreferencesGUI()
    {
        // Load the preferences
        if (!prefsLoaded)
        {
            boolPreference = EditorPrefs.GetBool("BoolPreferenceKey", false);
            prefsLoaded = true;
        }

        // Preferences GUI
        boolPreference = EditorGUILayout.Toggle("Bool Preference", boolPreference);

        // Save the preferences
        if (GUI.changed)
            EditorPrefs.SetBool("BoolPreferenceKey", boolPreference);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty p = property.FindPropertyRelative("onPress");
        if (p.intValue == 4)//custom prop
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ev")) + expandedHeight;
        }
        else
        {
            return yHeight;
        }
    }

    [MenuItem("GameObject/EasyUIBuilder/MenuButtons-Horizontal", false, 10)]
    static void CreateHorizontalButtons(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("MenuButtons");
        go.AddComponent<populateButtons>();
        HorizontalLayoutGroup h = go.AddComponent<HorizontalLayoutGroup>();

        h.spacing = 64;
        h.childControlWidth = true;
        h.childControlHeight = true;
        h.childForceExpandWidth = true;
        h.childForceExpandHeight = true;
        h.childAlignment = TextAnchor.UpperLeft;

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/EasyUIBuilder/MenuButtons-Vertical", false, 10)]
    static void CreateVerticalButtons(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("MenuButtons");
        go.AddComponent<populateButtons>();
        VerticalLayoutGroup v = go.AddComponent<VerticalLayoutGroup>();

        v.spacing = 80;
        v.childControlWidth = true;
        v.childForceExpandWidth = true;
        v.childForceExpandHeight = false;
        v.childAlignment = TextAnchor.UpperLeft;

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/EasyUIBuilder/UI Root", false, 10)]
    static void CreateUI(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("MenuUI");

        Canvas c = go.AddComponent<Canvas>();
        CanvasScaler cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        go.AddComponent<CanvasGroup>();
        go.AddComponent<GraphicRaycaster>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        go.AddComponent<AudioSource>();
        MenuManager m = go.AddComponent<MenuManager>();
        go.AddComponent<DontDestroy>();
        go.AddComponent<GameManager>();

        m.currentPanel = "";

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("GameObject/EasyUIBuilder/Menu Panel", false, 10)]
    static void CreatePanel(MenuCommand menuCommand)
    {

        GameObject m = new GameObject("MenuPanel");
        GameObjectUtility.SetParentAndAlign(m, menuCommand.context as GameObject);
        m.AddComponent<Image>();
        GUIutil.matchObjSizeWithParent(m, menuCommand.context as GameObject);

        Animator anim = m.AddComponent<Animator>();
        doAnimAndSleep doanim = m.AddComponent<doAnimAndSleep>();

        m.AddComponent<PanelInfo>();

        doanim.anim = anim;
        doanim.animTrigger = "menuOut";
        doanim.delay = 0.7f;

    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);


        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        // EditorGUIUtility.labelWidth = 14f;
        // position.width /= 2f;
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        xOffset = position.x;
        // Calculate rects
        var amountRect = getRect(100, position);
        var unitRect = getRect(100, position);
        var colorRect = getRect(50, position);
        var nameRect = getRect(200, position);
        var acRect = getRect(100, position);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        SerializedProperty p = property.FindPropertyRelative("onPress");
        if (p.intValue == 4)//custom prop
        {
            var eventRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            position.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ev"));


            var labelRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("OnPress : "));

            eventRect.y += 16;
            EditorGUI.PropertyField(eventRect, property.FindPropertyRelative("ev"), GUIContent.none);

        }
        else if (p.intValue == 0 || p.intValue == 3 || p.intValue == 6)
        {
            EditorGUI.PrefixLabel(nameRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Option Field"));
            nameRect.y += 16;
            nameRect.height -= 16;
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("argument"), GUIContent.none);
        }

        EditorGUI.PrefixLabel(acRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Custom Sound"));
        acRect.y += 16;
        acRect.height -= 16;
        EditorGUI.PropertyField(acRect, property.FindPropertyRelative("AC"), GUIContent.none);


        EditorGUI.PrefixLabel(amountRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Button Name"));
        amountRect.y += 16;
        amountRect.height -= 16;
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("name"), GUIContent.none);

        EditorGUI.PrefixLabel(colorRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Color"));
        colorRect.y += 16;
        colorRect.height -= 16;
        EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("color"), GUIContent.none);
        EditorGUI.PrefixLabel(unitRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Button Type"));
        unitRect.y += 16;
        unitRect.height -= 16;
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
