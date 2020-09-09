using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum AudioType { SFX,BGM };
public class setAudioLevels : MonoBehaviour
{
    public AudioMixer mixer;
    public string prefName;
    public AudioType audioType;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        float val = PlayerPrefs.GetFloat(prefName);
        slider.value = val;
        slider.onValueChanged.AddListener(SetVolume);
        
        mixer.SetFloat("volume", val);

    }

    public void SetVolume(float val)
    {
        mixer.SetFloat("volume", val);
        PlayerPrefs.SetFloat(prefName, val);
        
    }
}
