using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    // ====================== VARIABLES ======================

    public int id = 0;

    // =======================================================


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<TutorialManager>().doState(id);
        }
    }
}
