using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [Header("Components")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private MusicManager musicManager;
    private SoundManager soundManager;

    // =====================================================


    private void Start()
    {
        if (GameObject.Find("MusicManager") != null) { musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>(); }
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }

        setHandle(musicSlider, LoadSave.LoadSave.LoadMusicVolume());
        setHandle(soundSlider, LoadSave.LoadSave.LoadSoundVolume());
    }

    private void setHandle(Slider _slider, float _value)
    {
        _slider.value = _value;
    }

    public void getMusicValue()
    {
        setVolumeMusic(musicSlider.value);
    }

    public void getSoundValue()
    {
        setVolumeSound(soundSlider.value);
    }

    private void setVolumeMusic(float _value)
    {
        Save.Save.SaveMusicVolume(_value);
        if (musicManager != null) { musicManager.resetVolumeMusic(); }
    }

    private void setVolumeSound(float _value)
    {
        Save.Save.SaveSoundVolume(_value);
        if (soundManager != null) { soundManager.resetVolumeSound(); }
    }
}
