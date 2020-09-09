using UnityEngine;
using UnityEngine.Audio;
[System.Serializable]


/// <summary>
/// This is an asset which contains all the data for a theme.
/// As an asset it live in the project folder, and get built into an asset bundle.
/// </summary>
[CreateAssetMenu(fileName = "projData", menuName = "EasyUIBuilder/ProjectData")]
public class projData : ScriptableObject
{
    [Header("Project Data")]
    public string gameName = "EasyUI";
    //public MenuManager menu;
    [TextArea(6,20)]
    public string credits = "";

    [Header("SFX")]
    public GameObject defaultButton;
    public AudioClip menuConfirm;
    public AudioClip menuCancel;
    public AudioClip gameStart;

    public AudioMixer mixerSFX;
    public AudioMixer mixerAI;
    public AudioMixer mixerMusic;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Menus")]
    public string showInMenu;
    public string hideInMenu;
    public string showInGame;
    public string hideInGame;
}