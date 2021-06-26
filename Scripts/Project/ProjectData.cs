using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;
[System.Serializable]


/// <summary>
/// This is an asset which contains all the data for a theme.
/// As an asset it live in the project folder, and get built into an asset bundle.
/// </summary>
[CreateAssetMenu(fileName = "projData", menuName = "UI/ProjectData")]
public class ProjectData : ScriptableObject
{
    [Header("Project Data")]
    public string gameName = "CarDerby";
    public string versionNum;
    //public MenuManager menu;
    [TextArea(6, 20)]
    public string credits = "";
    public GameObject defaultButton;
    [Header("Sound")]
    public List<AudioClip> audioList;
    private Dictionary<string, AudioClip> audio;
    public List<AudioMixer> audioMixers;
    private Dictionary<string, AudioClip> mixers;
    [HideInInspector]
    public AudioSource player;
    public AudioClip menuMusic;
    public string menuConfirm;
    public string menuCancel;
    public string gameStart;
    [Header("Music")]
    public AudioClip gameMusic;

    [Header("Menus")]
    public string showInMenu;
    public string hideInMenu;
    public string showInGame;
    public string hideInGame;
    //each map should have its own music property.

    private void OnValidate()
    {
        audio = new Dictionary<string, AudioClip>();
        foreach(AudioClip a in audioList)
        {
            if (a == null || audio.ContainsKey(a.name)){ continue; }
            audio.Add(a.name, a);
        }
    }

    private void OnEnable()
    {
        ProjectSettings.data = this;
    }

    public void PlaySound(string sound)
    {
        audio.TryGetValue(sound, out AudioClip clip);
        if(clip!=null)
        {
            player.PlayOneShot(clip);
        }
    }

    public void PlaySound(ProjectSoundClip clip)
    {
        PlaySound(clip.audioName);
    }
    
    public string[] GetSoundList()
    {
        return audio.Keys.ToArray();
    }
}