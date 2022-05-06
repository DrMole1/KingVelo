using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTitle : MonoBehaviour
{
    private const float DELAY_NEXT_ANIM = 4f;
    private const float DELAY_NEXT_LETTER = 0.2f;
    private const float DELAY_GROW = 0.005f;
    private float MINSCALE_TOP = 1.5f;
    private float MAXSCALE_TOP = 1.8f;
    private float MINSCALE_DOWN = 1.2f;
    private float MAXSCALE_DOWN = 1.44f;

    // ===================== VARIABLES =====================

    [SerializeField] private Transform[] letters;

    // =====================================================


    private void Start()
    {
        StartCoroutine(IAnimateLetters());
    }

    private IEnumerator IAnimateLetters()
    {
        int cpt = 0;

        while (cpt < letters.Length)
        {
            StartCoroutine(IGrowLetter(letters[cpt], cpt));

            yield return new WaitForSeconds(DELAY_NEXT_LETTER);

            cpt++;
        }

        yield return new WaitForSeconds(DELAY_NEXT_ANIM);

        StartCoroutine(IAnimateLetters());
    }

    private IEnumerator IGrowLetter(Transform _letter, int _cpt)
    {
        float maxScale = 0;
        float minScale = 0;

        if(_cpt < 4)
        {
            maxScale = MAXSCALE_TOP;
            minScale = MINSCALE_TOP;
        }
        else
        {
            maxScale = MAXSCALE_DOWN;
            minScale = MINSCALE_DOWN;
        }

        while (_letter.localScale.x < maxScale)
        {
            _letter.localScale = new Vector2(_letter.localScale.x + 0.02f, _letter.localScale.y + 0.02f);

            yield return new WaitForSeconds(DELAY_GROW);
        }

        while (_letter.localScale.x > minScale)
        {
            _letter.localScale = new Vector2(_letter.localScale.x - 0.015f, _letter.localScale.y - 0.015f);

            yield return new WaitForSeconds(DELAY_GROW);
        }
    }
}
