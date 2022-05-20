using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private const float DELAY = 0.01f;

    // ====================== VARIABLES ======================

    [SerializeField] private int value;

    private SoundManager soundManager;
    private ScoreManager scoreManager;

    // =======================================================


    private void Awake()
    {
        if(GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
        if(GameObject.Find("SceneManager") != null) { scoreManager = GameObject.Find("SceneManager").GetComponent<ScoreManager>(); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(IAnimateCoin());

            float pitch = 1 + (scoreManager.getStreak() * 0.1f);
            if (soundManager != null) { soundManager.playAudioClipWithPitch(11, pitch); }

            scoreManager.addScore(value, true);
        }
    }

    private IEnumerator IAnimateCoin()
    {
        GetComponent<Rotate>().yAngle = 1000;

        int cpt = 0;

        while(cpt < 15)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.1f, transform.localPosition.z);

            yield return new WaitForSeconds(DELAY);
            cpt++;
        }

        while (cpt > 12)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.08f, transform.localPosition.z);

            yield return new WaitForSeconds(DELAY);
            cpt--;
        }

        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
}
