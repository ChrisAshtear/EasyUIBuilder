using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UISound : MonoBehaviour
{
    [HideInInspector]
    public string sound;
    [HideInInspector]
    public List<string> choices;
    [HideInInspector]
    public int listIdx = 0;

    // Use this for initialization
    void Start()
    {
        Toggle t = GetComponent<Toggle>();
        if (t != null)
        {
            t.onValueChanged.AddListener(delegate { PlaySound(); });
        }
        
        
        Button b = GetComponent<Button>();
        if (b != null)
        {
            b.onClick.AddListener(() => PlaySound());
        }
    }

    public void PlaySound()
    {
        ProjectSettings.data.PlaySound(sound);
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(UISound))]
public class UISoundDrawer : Editor
{
    private List<string> _choices = new List<string>();
    int _choiceIndex = 0;

    private void OnValidate()
    {
        _choices = new List<string>();
    }

    public override void OnInspectorGUI()
    {
        var uisound = target as UISound;
        _choiceIndex = uisound.listIdx;
        if(_choices.Count == 0) {
            ProjectSettings.data = Resources.LoadAll<ProjectData>("")[0];
            foreach (AudioClip a in ProjectSettings.data.audioList)
            {
                _choices.Add(a.name);
            }
        }
        // Draw the default inspector
        DrawDefaultInspector();
        
        _choices = uisound.choices;
        uisound.listIdx = _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices.ToArray());

        // Update the selected choice in the underlying object
        uisound.sound = ProjectSettings.data.audioList[_choiceIndex].name;
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }
}
#endif