using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MouseOver : MonoBehaviour
{
    enum Type { ButtonList, SimpleButton };

    // ===================== VARIABLES =====================

    [Header("Type for Interaction")]
    [SerializeField] private Type type;

    [Header("Components for ButtonList")]
    [SerializeField] private GameObject selector;

    private SoundManager soundManager;
    private bool canDoSound = true;
    private TransitionManager transition;

    // =====================================================


    private void Awake()
    {
        if(GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
        if (GameObject.Find("Transition") != null) { transition = GameObject.Find("Transition").GetComponent<TransitionManager>(); }
    }

    void OnMouseOver()
    {
        if(transition.isOnTransition) { return; }

        if (soundManager != null && canDoSound) { soundManager.playAudioClip(2); }

        switch (type)
        {
            case Type.ButtonList:
                selector.SetActive(true);
                break;
            default:
                break;
        }

        canDoSound = false;
    }

    void OnMouseExit()
    {
        canDoSound = true;

        switch (type)
        {
            case Type.ButtonList:
                selector.SetActive(false);
                break;
            default:
                break;
        }
    }
}
