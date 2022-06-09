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

    public void callDecreasePitch(float _pitch)
    {
        StartCoroutine(IDecreasePitch(_pitch));
    }

    public void callIncreasePitch(float _pitch)
    {
        StartCoroutine(IIncreasePitch(_pitch));
    }

    private IEnumerator IDecreasePitch(float _pitch)
    {
        while(audioSource.pitch > _pitch)
        {
            audioSource.pitch -= 0.01f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator IIncreasePitch(float _pitch)
    {
        while (audioSource.pitch < _pitch)
        {
            audioSource.pitch += 0.01f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void callChangeMusic(int _music)
    {
        StartCoroutine(IChangeMusic(_music));
    }

    private IEnumerator IChangeMusic(int _music)
    {
        while(audioSource.volume > 0)
        {
            audioSource.volume -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        audioSource.volume = 0;
        audioSource.pitch = 1;
        audioSource.Stop();
        audioSource.clip = clips[_music];
        audioSource.Play();

        yield return new WaitForSeconds(1.5f);

        while (audioSource.volume < LoadSave.LoadSave.LoadSoundVolume())
        {
            audioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        audioSource.volume = LoadSave.LoadSave.LoadSoundVolume();
    }
}