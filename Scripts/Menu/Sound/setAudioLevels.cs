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
        Init();

    }

    public void Init()
    {
        slider = GetComponent<Slider>();
        float val = PlayerPrefs.GetFloat(prefName,1);
        slider.value = val;
        slider.onValueChanged.AddListener(SetVolume);

        mixer.SetFloat("volume", Mathf.Log10(val) * 20);
    }

    public void SetVolume(float val)
    {
        mixer.SetFloat("volume", Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat(prefName, val);
        
    }
}
