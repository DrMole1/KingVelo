using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsGenerator : MonoBehaviour
{
    private const int CHANCE_TO_SPAWN = 75;
    private const int CHANCE_TO_HAVE_SIGN = 20;

    // ====================== VARIABLES ======================



    // =======================================================


    private void Awake()
    {
        desactiveAllTrafficLights();
    }

    private void Start()
    {
        activeTrafficLights();
    }

    private void desactiveAllTrafficLights()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void activeTrafficLights()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int chance = Random.Range(1, 101);

            if(chance <= CHANCE_TO_SPAWN)
            {
                transform.GetChild(i).gameObject.SetActive(true);

                activeSign(transform.GetChild(i));
            }         
        }
    }

    private void activeSign(Transform _tr)
    {
        int chance = Random.Range(1, 101);

        if (chance <= CHANCE_TO_HAVE_SIGN)
        {
            _tr.GetChild(1).gameObject.SetActive(true);
            _tr.GetComponent<TrafficLights>().hasSign = true;
        }
    }
}
