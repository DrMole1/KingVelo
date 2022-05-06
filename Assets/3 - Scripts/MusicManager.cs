using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const float FACTOR_MITIGATE = 2f;

    // ============================ VARIABLES ============================

    static GameObject instance;

    [Header("Music Clips")]
    public AudioClip[] clips;

    private AudioSource audioSource;

    // ===================================================================


    void Awake()
    {
        if (instance)
        { // if bgAudio already references some object...
            Destroy(gameObject); // suicide
        }
        else
        { // if bgAudio is null, this is the first TronBG object:
            instance = gameObject; // assign this one
        }

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void playMusic(int index)
    {
        if (clips[index] != null)
        {
            audioSource.volume = LoadSave.LoadSave.LoadSoundVolume();
            audioSource.pitch = 1;
            audioSource.clip = clips[index];
            audioSource.Play();
        }
        else
            Debug.Log("Attention, clip vide.");
    }

    public void mitigateVolumeMusic()
    {
        audioSource.volume = LoadSave.LoadSave.LoadMusicVolume() / FACTOR_MITIGATE;
    }

    public void resetVolumeMusic()
    {
        audioSource.volume = LoadSave.LoadSave.LoadMusicVolume();
    }
}