using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorAuto : MonoBehaviour
{
    private const float MIN_DELAY_A = 4f;
    private const float MAX_DELAY_A = 7f;
    private const float MIN_DELAY_B = 9f;
    private const float MAX_DELAY_B = 12f;

    private const int CHANCE_TO_SPAWN_CAR = 80;

    // ====================== VARIABLES ======================

    [SerializeField] private Transform destructorToLookAt;
    [SerializeField] private GameObject autoPref;
    [SerializeField] private GameObject truckPref;

    private bool isActivated = true;
    private float delayA;
    private float delayB;

    // =======================================================


    public void setActivated(bool _activate) { isActivated = _activate; }
    public bool getActivated() { return isActivated; }


    private void Awake()
    {
        delayA = Random.Range(MIN_DELAY_A, MAX_DELAY_A);
        delayB = Random.Range(MIN_DELAY_B, MAX_DELAY_B);

        transform.LookAt(destructorToLookAt);
    }

    private void Start()
    {
        StartCoroutine(IGenerateObject(chooseObjectToGenerate()));
    }

    private GameObject chooseObjectToGenerate()
    {
        int choice = Random.Range(1, 101);
        GameObject tempObj;

        if(choice <= CHANCE_TO_SPAWN_CAR) { tempObj = autoPref; }
        else { tempObj = truckPref; }

        return tempObj;
    }

    private IEnumerator IGenerateObject(GameObject _objPref)
    {
        float delay = Random.Range(delayA, delayB);

        yield return new WaitForSeconds(delay);

        if (isActivated) { spawnObject(_objPref); }

        StartCoroutine(IGenerateObject(chooseObjectToGenerate()));
    }

    private void spawnObject(GameObject _objPref)
    {
        GameObject obj;
        obj = Instantiate(_objPref, transform.position, transform.rotation, transform);
    }
}
