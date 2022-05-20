using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private const float MAX_SCALE_HEART = 35f;
    private const float MIN_SCALE_HEART = 0f;
    private const float ADD_SCALE_HEART = 1.5f;
    private const float DELAY = 0.01f;
    private const float DELAY_BLINK = 0.1f;
    private const float ADD_ALPHA = 0.1f;

    // ====================== VARIABLES ======================

    [Header("Life")]
    [SerializeField] private int lifePoint = 3;
    [SerializeField] private RectTransform[] lifeHeart;
    [SerializeField] private GameObject[] objToBlink;
    [SerializeField] private Image[] borders;

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

    [SerializeField] GameManager gameManager;


    private bool isImmune = false;
    private SoundManager soundManager;

    // =======================================================


    private void Awake()
    {
        setCustomizationPrefabs();

        if(GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
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

    public void takeDamage()
    {
        if(isImmune || lifePoint <= 0) { return; }

        lifePoint--;

        StartCoroutine(IAnimateHeart());

        if (lifePoint == 0) { die(); }
        else
        {
            StartCoroutine(IBlinkPlayer());
            StartCoroutine(IFadeBorders());

            if (soundManager != null) { soundManager.playAudioClip(9); }
        }
    }

    private IEnumerator IAnimateHeart()
    {
        while(lifeHeart[lifePoint].sizeDelta.x < MAX_SCALE_HEART)
        {
            yield return new WaitForSeconds(DELAY);

            lifeHeart[lifePoint].sizeDelta = new Vector2(lifeHeart[lifePoint].sizeDelta.x + ADD_SCALE_HEART, lifeHeart[lifePoint].sizeDelta.y + ADD_SCALE_HEART);
        }

        while (lifeHeart[lifePoint].sizeDelta.x > MIN_SCALE_HEART)
        {
            yield return new WaitForSeconds(DELAY);

            lifeHeart[lifePoint].sizeDelta = new Vector2(lifeHeart[lifePoint].sizeDelta.x - ADD_SCALE_HEART, lifeHeart[lifePoint].sizeDelta.y - ADD_SCALE_HEART);
        }

        lifeHeart[lifePoint].GetComponent<Image>().enabled = false;
        lifeHeart[lifePoint].GetChild(0).gameObject.SetActive(true);
        Color color = new Color(1f, 1f, 1f, 1f);
        float a = 1f;

        while (lifeHeart[lifePoint].sizeDelta.x < MAX_SCALE_HEART)
        {
            yield return new WaitForSeconds(DELAY);

            lifeHeart[lifePoint].sizeDelta = new Vector2(lifeHeart[lifePoint].sizeDelta.x + ADD_SCALE_HEART, lifeHeart[lifePoint].sizeDelta.y + ADD_SCALE_HEART);
            color = new Color(1f, 1f, 1f, a);
            lifeHeart[lifePoint].GetChild(0).GetComponent<Image>().color = color;
            a -= 0.02f;
        }

        lifeHeart[lifePoint].gameObject.SetActive(false);
    }

    private IEnumerator IBlinkPlayer()
    {
        isImmune = true;

        for(int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(false);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(true);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(false);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(true);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(false);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(true);
        }

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(false);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(true);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(false);
        }

        yield return new WaitForSeconds(DELAY_BLINK);

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        isImmune = false;
    }

    private IEnumerator IFadeBorders()
    {
        Color color = new Color(1f, 0f, 0f, 0f);
        float a = 0f;

        while(a < 1f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(0.89f, 0.65f, 0.75f, a);
            for(int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a += ADD_ALPHA;
        }

        while (a > 0f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(0.89f, 0.65f, 0.75f, a);
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a -= ADD_ALPHA;
        }

        while (a < 1f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(0.89f, 0.65f, 0.75f, a);
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a += ADD_ALPHA;
        }

        while (a > 0f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(0.89f, 0.65f, 0.75f, a);
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a -= ADD_ALPHA;
        }

        while (a < 1f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(0.89f, 0.65f, 0.75f, a);
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a += ADD_ALPHA;
        }

        while (a > 0f)
        {
            yield return new WaitForSeconds(DELAY);

            color = new Color(0.89f, 0.65f, 0.75f, a);
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].color = color;
            }
            a -= ADD_ALPHA;
        }
    }

    private void die()
    {
        if (soundManager != null) { soundManager.playAudioClip(10); }

        for (int i = 0; i < objToBlink.Length; i++)
        {
            objToBlink[i].SetActive(false);
        }

        GetComponent<PlayerMovement>().state = PlayerMovement.State.Stop;

        gameManager.callStopPlayer(false);
    }
}
