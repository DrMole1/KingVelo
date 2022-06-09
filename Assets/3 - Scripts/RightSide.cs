using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSide : MonoBehaviour
{
    private const int SCORE_TO_ADD = 4;
    private const float DELAY_TO_CHECK = 1f;
    private const float MIN_SPEED_TO_GET_SCORE = 3.5f;

    // ====================== VARIABLES ======================

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject ptcPref;

    private bool isPlayerRightSide = false;
    private Coroutine coroutine;

    private SoundManager soundManager;
    private ScoreManager scoreManager;

    // =======================================================


    private void Awake()
    {
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
        if (GameObject.Find("SceneManager") != null) { scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerRightSide = true;
            coroutine = StartCoroutine(ICheckPlayerOnRightSide());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerRightSide = false;
            if(coroutine != null) { StopCoroutine(coroutine); }
        }
    }

    private IEnumerator ICheckPlayerOnRightSide()
    {
        yield return new WaitForSeconds(DELAY_TO_CHECK);

        if(isPlayerRightSide && playerMovement.getSpeed() > MIN_SPEED_TO_GET_SCORE)
        {
            if (soundManager != null) { soundManager.playAudioClip(13); }
            scoreManager.addScore(SCORE_TO_ADD, false);

            GameObject ptc;
            ptc = Instantiate(ptcPref, playerMovement.transform.position, Quaternion.identity, playerMovement.transform);
            Destroy(ptc, 2f);
        }

        coroutine = StartCoroutine(ICheckPlayerOnRightSide());
    }
}
