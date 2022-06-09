using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyGenerator : MonoBehaviour
{
    private const int CHANCE_TO_SPAWN = 50;

    // ====================== VARIABLES ======================


    // =======================================================


    private void Awake()
    {
        desactiveAllDummies();
    }

    private void Start()
    {
        activeDummies();
    }

    private void desactiveAllDummies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void activeDummies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int chance = Random.Range(1, 101);

            if (chance <= CHANCE_TO_SPAWN)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<Dummy>().paintObject();
            }
        }
    }
}
