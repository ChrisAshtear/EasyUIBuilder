using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
/*
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TabProps))]
public class TabDrawer : PropertyDrawer
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

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        xOffset = position.x;
        position.xMax += 100;
        var fieldsRect = new Rect(position.x, position.y, position.width, yHeight);

        EditorRowProps fields = new EditorRowProps(fieldsRect);
        fields.AddProp(property, "name", "Button Name");
        fields.AddProp(property, "color", "Button Color");
        fields.AddProp(property, "txtcolor", "Label Color");
        fields.AddProp(property, "onPress", "Button Type");
        fields.AddProp(property, "AC", "Custom Sound");
        SerializedProperty p = property.FindPropertyRelative("onPress");

        if (p.intValue == 0 || p.intValue == 3 || p.intValue == 6)
        {
            string optName = "Option Field";
            switch (p.intValue)
            {
                case 0:
                    optName = "Panel Name";
                    break;
                case 3:
                    optName = "Pref Name";
                    break;
                case 6:
                    optName = "URL";
                    break;
            }
            fields.AddProp(property, "argument", optName);
        }
        else { fields.AddProp(property, "none"); }



        fields.Draw();
        if (p.intValue == 4)//custom prop
        {
            EditorGUI.indentLevel = 0;
            var eventRect = new Rect(position.x, position.y + yHeight, position.width, position.height - yHeight);
            position.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ev"));

            eventRect.x -= 100;
            var labelRect = new Rect(position.x - 100, position.y + yHeight, position.width, position.height - yHeight);
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("OnPress : "));

            eventRect.y += 16;
            EditorGUI.PropertyField(eventRect, property.FindPropertyRelative("ev"), GUIContent.none);

        }
        EditorGUI.EndProperty();
    }
}
#endif
*/