using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorAuto : MonoBehaviour
{
    private const float MIN_DELAY_A = 4f;
    private const float MAX_DELAY_A = 5f;
    private const float MIN_DELAY_B = 6f;
    private const float MAX_DELAY_B = 7f;

    private const int CHANCE_TO_SPAWN_CAR = 90;

    // ====================== VARIABLES ======================

    [SerializeField] private Transform posToLookAt;
    [SerializeField] private GameObject autoPref;
    [SerializeField] private GameObject truckPref;
    [SerializeField] private Material[] m_Auto;

    private bool isActivated = false;
    private float delayA;
    private float delayB;

    // =======================================================


    public void setActivated(bool _activate) { isActivated = _activate; }
    public bool getActivated() { return isActivated; }


    private void Awake()
    {
        delayA = Random.Range(MIN_DELAY_A, MAX_DELAY_A);
        delayB = Random.Range(MIN_DELAY_B, MAX_DELAY_B);

        transform.LookAt(posToLookAt);
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

        paintObject(obj);
    }

    private void paintObject(GameObject _obj)
    {
        Material mat = m_Auto[Random.Range(0, m_Auto.Length)];

        Transform mesh = _obj.transform.GetChild(0);
        for (int i = 0; i < mesh.childCount; i++)
        {
            if(mesh.GetChild(i).CompareTag("Color"))
            {
                mesh.GetChild(i).GetComponent<MeshRenderer>().material = mat;
            }
        }
    }
}
