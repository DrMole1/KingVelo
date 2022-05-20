using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // ====================== VARIABLES ======================

    [SerializeField] GameManager gameManager;

    // =======================================================


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.callStopPlayer(true);
        }
    }
}
