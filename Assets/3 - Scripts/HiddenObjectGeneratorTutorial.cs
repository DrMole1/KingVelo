using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenObjectGeneratorTutorial : MonoBehaviour
{
    // =============== VARIABLES ===============

    [SerializeField] private Image frames;
    [SerializeField] private Sprite hiddenObjectsGreen;

    [Header("Hidden Objects")]
    [SerializeField] private GameObject greenGO;


    private ScoreManager scoreManager;
    private SoundManager soundManager;

    // =========================================

    private void Awake()
    {
        scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>();
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }


    private void Update()
    {
        checkHiddenObjects();   
    }



    private void checkHiddenObjects()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layer = 14;
        int layerMask = 1 << layer;
        //layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hit, 30, layerMask))
        {
            Transform objectHit = hit.transform;

            if(objectHit.CompareTag("HiddenObjects"))
            {
                if(objectHit.name.Contains("Green"))
                {
                    checkGreen(objectHit.gameObject);
                }
            }
        }
    }

    private void checkGreen(GameObject _go)
    {
        if (_go == greenGO)
        {
            collectItem(_go);
        }
    }

    private void collectItem(GameObject _go)
    {
        Destroy(_go);

        frames.sprite = hiddenObjectsGreen; scoreManager.addScore(300, false);

        Color color = new Color(1f, 1f, 1f, 1f);
        frames.color = color;

        StartCoroutine(IAnimateFrame());

        if (soundManager != null) { soundManager.playAudioClip(24); }
    }

    private IEnumerator IAnimateFrame()
    {
        while(frames.transform.localScale.x < 1.5f)
        {
            yield return new WaitForSeconds(0.01f);

            frames.transform.localScale = new Vector3(frames.transform.localScale.x + 0.02f, frames.transform.localScale.y + 0.02f, 1f);
        }

        while (frames.transform.localScale.x > 1)
        {
            yield return new WaitForSeconds(0.01f);

            frames.transform.localScale = new Vector3(frames.transform.localScale.x - 0.01f, frames.transform.localScale.y - 0.01f, 1f);
        }
    }
}
