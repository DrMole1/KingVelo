using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenObjectGenerator : MonoBehaviour
{
    // =============== VARIABLES ===============

    [Header("Canvas")]
    [SerializeField] private Image[] frames;
    [SerializeField] private Sprite[] blackObjectsGreen;
    [SerializeField] private Sprite[] blackObjectsBlue;
    [SerializeField] private Sprite[] blackObjectsViolet;
    [SerializeField] private Sprite[] hiddenObjectsGreen;
    [SerializeField] private Sprite[] hiddenObjectsBlue;
    [SerializeField] private Sprite[] hiddenObjectsViolet;

    [Header("Hidden Objects")]
    [SerializeField] private Transform greenRoot;
    [SerializeField] private Transform blueRoot;
    [SerializeField] private Transform violetRoot;

    private int[] greenObjects = {-1, -1, -1};
    private int[] blueObjects = { -1, -1};
    private int violetObject = -1;

    private GameObject[] greenGO = new GameObject[3];
    private GameObject[] blueGO = new GameObject[2];
    private GameObject[] violetGO = new GameObject[1];

    private ScoreManager scoreManager;
    private SoundManager soundManager;

    // =========================================

    private void Awake()
    {
        scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>();
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    private void Start()
    {
        chooseGreenObjects();
        chooseBlueObjects();
        chooseVioletObject();

        showFrame();

        activateObjectsGreen();
        activateObjectsBlue();
        activateObjectsViolet();
    }

    private void Update()
    {
        checkHiddenObjects();   
    }


    private void chooseGreenObjects()
    {
        bool check;

        for(int i = 0; i < 3; i++)
        {
            check = true;

            while(check)
            {
                int choice = Random.Range(0, 6);

                if(choice == greenObjects[0] || choice == greenObjects[1] || choice == greenObjects[2])
                {
                    check = true;
                }
                else
                {
                    check = false;
                    greenObjects[i] = choice;
                }
            }
        }
    }

    private void chooseBlueObjects()
    {
        bool check;

        for (int i = 0; i < 2; i++)
        {
            check = true;

            while (check)
            {
                int choice = Random.Range(0, 4);

                if (choice == blueObjects[0] || choice == blueObjects[1])
                {
                    check = true;
                }
                else
                {
                    check = false;
                    blueObjects[i] = choice;
                }
            }
        }
    }

    private void chooseVioletObject()
    {
        int choice = Random.Range(0, 3);
        violetObject = choice;
    }

    private void showFrame()
    {
        frames[0].sprite = blackObjectsGreen[greenObjects[0]];
        frames[1].sprite = blackObjectsGreen[greenObjects[1]];
        frames[2].sprite = blackObjectsGreen[greenObjects[2]];
        frames[3].sprite = blackObjectsBlue[blueObjects[0]];
        frames[4].sprite = blackObjectsBlue[blueObjects[1]];
        frames[5].sprite = blackObjectsViolet[violetObject];
    }

    private void activateObjectsGreen()
    {
        Transform objectRoot;

        for (int i = 0; i < 6; i++)
        {
            objectRoot = greenRoot.GetChild(i);

            if (i == greenObjects[0] || i == greenObjects[1] || i == greenObjects[2])
            {
                int choice = Random.Range(0, 3);

                for (int j = 0; j < objectRoot.childCount; j++)
                {
                    if (j != choice) { objectRoot.GetChild(j).gameObject.SetActive(false); }
                    if (j == choice) 
                    {
                        if (i == greenObjects[0]) { greenGO[0] = objectRoot.GetChild(j).gameObject; }
                        if (i == greenObjects[1]) { greenGO[1] = objectRoot.GetChild(j).gameObject; }
                        if (i == greenObjects[2]) { greenGO[2] = objectRoot.GetChild(j).gameObject; }
                    }
                }
            }
            else
            {
                objectRoot.gameObject.SetActive(false);
            }
        }
    }

    private void activateObjectsBlue()
    {
        Transform objectRoot;

        for (int i = 0; i < 4; i++)
        {
            objectRoot = blueRoot.GetChild(i);

            if (i == blueObjects[0] || i == blueObjects[1])
            {
                int choice = Random.Range(0, 3);

                for (int j = 0; j < objectRoot.childCount; j++)
                {
                    if (j != choice) { objectRoot.GetChild(j).gameObject.SetActive(false); }
                    if (j == choice) 
                    {
                        if (i == blueObjects[0]) { blueGO[0] = objectRoot.GetChild(j).gameObject; }
                        if (i == blueObjects[1]) { blueGO[1] = objectRoot.GetChild(j).gameObject; }
                    }
                }
            }
            else
            {
                objectRoot.gameObject.SetActive(false);
            }
        }
    }

    private void activateObjectsViolet()
    {
        Transform objectRoot;

        for (int i = 0; i < 3; i++)
        {
            objectRoot = violetRoot.GetChild(i);

            if (i == violetObject)
            {
                int choice = Random.Range(0, 3);

                for (int j = 0; j < objectRoot.childCount; j++)
                {
                    if (j != choice) { objectRoot.GetChild(j).gameObject.SetActive(false); }
                    if (j == choice) { violetGO[0] = objectRoot.GetChild(j).gameObject; }
                }
            }
            else
            {
                objectRoot.gameObject.SetActive(false);
            }
        }
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

                if (objectHit.name.Contains("Blue"))
                {
                    checkBlue(objectHit.gameObject);
                }

                if (objectHit.name.Contains("Violet"))
                {
                    checkViolet(objectHit.gameObject);
                }
            }
        }
    }

    private void checkGreen(GameObject _go)
    {
        if (_go == greenGO[0])
        {
            collectItem(_go, 0);
        }

        if (_go == greenGO[1])
        {
            collectItem(_go, 1);
        }

        if (_go == greenGO[2])
        {
            collectItem(_go, 2);
        }
    }

    private void checkBlue(GameObject _go)
    {
        if (_go == blueGO[0])
        {
            collectItem(_go, 3);
        }

        if (_go == blueGO[1])
        {
            collectItem(_go, 4);
        }
    }

    private void checkViolet(GameObject _go)
    {
        if (_go == violetGO[0])
        {
            collectItem(_go, 5);
        }
    }

    private void collectItem(GameObject _go, int _idFrame)
    {
        Destroy(_go);

        if (_idFrame == 0) { frames[0].sprite = hiddenObjectsGreen[greenObjects[0]]; scoreManager.addScore(300, false); }
        if (_idFrame == 1) { frames[1].sprite = hiddenObjectsGreen[greenObjects[1]]; scoreManager.addScore(300, false); }
        if (_idFrame == 2) { frames[2].sprite = hiddenObjectsGreen[greenObjects[2]]; scoreManager.addScore(300, false); }
        if (_idFrame == 3) { frames[3].sprite = hiddenObjectsBlue[blueObjects[0]]; scoreManager.addScore(400, false); }
        if (_idFrame == 4) { frames[4].sprite = hiddenObjectsBlue[blueObjects[1]]; scoreManager.addScore(400, false); }
        if (_idFrame == 5) { frames[5].sprite = hiddenObjectsViolet[violetObject]; scoreManager.addScore(500, false); }

        Color color = new Color(1f, 1f, 1f, 1f);
        frames[_idFrame].color = color;

        StartCoroutine(IAnimateFrame(_idFrame));

        if (soundManager != null) { soundManager.playAudioClip(24); }
    }

    private IEnumerator IAnimateFrame(int _id)
    {
        while(frames[_id].transform.localScale.x < 1.5f)
        {
            yield return new WaitForSeconds(0.01f);

            frames[_id].transform.localScale = new Vector3(frames[_id].transform.localScale.x + 0.02f, frames[_id].transform.localScale.y + 0.02f, 1f);
        }

        while (frames[_id].transform.localScale.x > 1)
        {
            yield return new WaitForSeconds(0.01f);

            frames[_id].transform.localScale = new Vector3(frames[_id].transform.localScale.x - 0.01f, frames[_id].transform.localScale.y - 0.01f, 1f);
        }
    }
}
