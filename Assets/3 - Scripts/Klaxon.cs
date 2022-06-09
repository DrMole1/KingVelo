using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klaxon : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [Header("Sound Clips")]
    public AudioClip[] clips;

    [Header("Klaxon Buttons")]
    public GameObject pushLeft;
    public GameObject pushRight;

    [Header("Player Transform")]
    [SerializeField] private Transform player;

    private int sound;
    private AudioSource audioSource;

    // =====================================================

    private void Awake()
    {
        sound = LoadSave.LoadSave.LoadEquipedKlaxon();

        audioSource = GetComponent<AudioSource>();
    }

    public void useKlaxon()
    {
        StartCoroutine(IAnimateKlaxon());
        checkDummies();

        if (clips[sound] != null)
        {
            audioSource.volume = LoadSave.LoadSave.LoadSoundVolume();
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(clips[sound]);
        }
        else
            Debug.Log("Attention, clip vide.");
    }

    private IEnumerator IAnimateKlaxon()
    {
        pushLeft.SetActive(false);
        pushRight.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        pushLeft.SetActive(true);
        pushRight.SetActive(false);
    }

    private void checkDummies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.position, 15f);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Dummy"))
            {
                hitCollider.gameObject.GetComponent<Dummy>().alertDummy();
            }
        }
    }
}
