using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ProjectSoundClip
{
    public int idx = 0;
    public string audioName;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ProjectSoundClip))]
public class SoundClipDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    int _choiceIndex = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        ProjectData dat = ProjectSettings.GetData();

        SerializedProperty soundStr = property.FindPropertyRelative("audioName");
        SerializedProperty soundIdx = property.FindPropertyRelative("idx");
        string[] audioList = dat.GetSoundList();

        if(soundIdx.intValue >= audioList.Length) { soundIdx.intValue = audioList.Length-1; }

        soundIdx.intValue = EditorGUI.Popup(position, soundIdx.intValue, audioList);

        soundStr.stringValue = dat.audioList[soundIdx.intValue].name;

        EditorGUI.EndProperty();
    }

}

#endif