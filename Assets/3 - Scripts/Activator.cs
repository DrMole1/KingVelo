using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public enum Role { Activate, Desactivate };

    // ====================== VARIABLES ======================

    [SerializeField] private Role role;
    [SerializeField] private GeneratorAuto[] generators;

    // =======================================================


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (role == Role.Activate) { activateGenerator(); }
            else if (role == Role.Desactivate) { desactivateGenerator(); }
        }
    }

    private void activateGenerator()
    {
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].setActivated(true);
        }
    }

    private void desactivateGenerator()
    {
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].setActivated(false);
        }
    }
}
