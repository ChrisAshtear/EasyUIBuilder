using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataSource))]
public class DS_Editor : Editor
{
    string[] _choices = new[] { "Player1", "Player2" };
    int _choiceIndex = 0;

    public List<string> fields;

    public void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
        DataSource myTarget = (DataSource)target;

        DrawDefaultInspector();

        _choiceIndex = EditorGUILayout.Popup("Player", _choiceIndex, _choices);
        // Update the selected choice in the underlying object

    }
}