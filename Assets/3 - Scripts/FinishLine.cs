using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // ====================== VARIABLES ======================

    [SerializeField] GameManager gameManager;
    [SerializeField] private GameObject ptcCheckPref;

    private ScoreManager scoreManager;
    private SoundManager soundManager;

    // =======================================================

    private void Awake()
    {
        scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>();
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.callStopPlayer(true);

            if (soundManager != null) { soundManager.playAudioClip(20); }

            scoreManager.addScore(50, false);

            Transform ptcRoot = transform.parent.GetChild(2);
            Quaternion quat = ptcRoot.rotation;
            Vector3 pos = ptcRoot.position;

            GameObject ptcCheck;
            ptcCheck = Instantiate(ptcCheckPref, pos, quat);
            Destroy(ptcCheck, 3f);

            Destroy(ptcRoot.gameObject);
        }
    }
}
