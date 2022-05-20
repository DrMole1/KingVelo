using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    private const int CHANCE_TO_SPAWN_COIN = 60;

    // ====================== VARIABLES ======================

    [Header("Object to Spawn")]
    [SerializeField] private GameObject objPref;

    [Header("Read Only : Stats of Spawning")]
    [SerializeField] private int spawnedCoin;
    [SerializeField] private int maxSpawners;

    // =======================================================


    private void Start()
    {
        maxSpawners = transform.childCount;
        generateCoins();
    }

    private void generateCoins()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            int chanceToSpawn = Random.Range(1, 101);

            if(chanceToSpawn <= CHANCE_TO_SPAWN_COIN)
            {
                spawnCoin(transform.GetChild(i));
                spawnedCoin++;
            }
        }

        deleteEmptySpawners();
    }

    private void spawnCoin(Transform _tr)
    {
        GameObject coin;
        coin = Instantiate(objPref, _tr.position, Quaternion.identity, _tr);
    }

    private void deleteEmptySpawners()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).childCount == 0)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
