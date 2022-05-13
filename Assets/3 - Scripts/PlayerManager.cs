using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // ====================== VARIABLES ======================

    [Header("Customization Prefabs")]
    [SerializeField] private GameObject[] bikesPref;
    [SerializeField] private GameObject[] wheelsPref;
    [SerializeField] private GameObject[] helmetsPref;

    [Header("Pos to Spawn")]
    [SerializeField] private Transform bikePos;
    [SerializeField] private Transform wheelPos01;
    [SerializeField] private Transform wheelPos02;
    [SerializeField] private Transform helmetPos;

    [Header("Skin Materials")]
    [SerializeField] private Material[] m_skins;

    // =======================================================


    private void Awake()
    {
        setCustomizationPrefabs();
    }


    private void setCustomizationPrefabs()
    {
        spawnBike();
        spawnWheel(wheelPos01);
        spawnWheel(wheelPos02);
        spawnHelmet();
        setSkinColor();
    }

    private void spawnBike()
    {
        GameObject bike;
        bike = Instantiate(bikesPref[LoadSave.LoadSave.LoadEquipedBike()], bikePos.position, Quaternion.identity, bikePos);
    }

    private void spawnWheel(Transform _tr)
    {
        GameObject wheel;
        wheel = Instantiate(wheelsPref[LoadSave.LoadSave.LoadEquipedWheels()], _tr.position, Quaternion.identity, _tr);
    }

    private void spawnHelmet()
    {
        GameObject helmet;
        helmet = Instantiate(helmetsPref[LoadSave.LoadSave.LoadEquipedHelmet()], helmetPos.position, Quaternion.identity, helmetPos);

        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();

        currentRot = new Vector3(0f, 0f, 0f);
        currentQuaternionRot.eulerAngles = currentRot;
        helmet.transform.localRotation = currentQuaternionRot;
    }

    private void setSkinColor()
    {
        GameObject[] skinObjects;
        skinObjects = GameObject.FindGameObjectsWithTag("Skin");

        foreach (GameObject obj in skinObjects)
        {
            obj.GetComponent<MeshRenderer>().material = m_skins[LoadSave.LoadSave.LoadSkin()];
        }
    }
}
