using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    private const float DELAY_BACKGROUND = 0.01f;
    private const float DELAY_SKIP_RULE = 2.5f;

    enum OnStartEvent { None, Menu, Game };

    // ===================== VARIABLES =====================

    [Header("Properties")]
    [SerializeField] private OnStartEvent startEvent;
    [SerializeField] private Sprite[] rules;

    [Header("Objects To Drag")]
    [SerializeField] private RectTransform topBackground;
    [SerializeField] private RectTransform downBackground;
    [SerializeField] private GameObject infoSkip;
    [SerializeField] private GameObject ruleSlot;

    private bool canSkip = false;

    // =====================================================


    private void Start()
    {
        transform.SetAsLastSibling();

        loadStartEvent();
    }

    private void Update()
    {
        if(canSkip)
        {
            skipRule();
        }
    }

    private void skipRule()
    {
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            canSkip = false;

            infoSkip.SetActive(false);
            ruleSlot.SetActive(false);

            callUnshowBackground(0.1f);
        }
    }

    private void loadStartEvent()
    {
        if (startEvent == OnStartEvent.Menu)
        {
            setBackgroundOnMenuStart();
        }
        else if (startEvent == OnStartEvent.Game)
        {
            setBackgroundOnGameStart();
        }
    }

    public void callShowBackground(float _delay)
    {
        StartCoroutine(IShowBackground(_delay));
    }

    public void callUnshowBackground(float _delay)
    {
        StartCoroutine(IUnshowBackground(_delay));
    }

    private IEnumerator IShowBackground(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        int cpt = 0;

        while(cpt < 80)
        {
            yield return new WaitForSeconds(DELAY_BACKGROUND);

            topBackground.transform.localPosition = new Vector2(topBackground.transform.localPosition.x - 4, topBackground.transform.localPosition.y - 4);
            downBackground.transform.localPosition = new Vector2(downBackground.transform.localPosition.x + 4, downBackground.transform.localPosition.y + 4);

            cpt++;
        }
    }

    private IEnumerator IUnshowBackground(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        int cpt = 0;

        while (cpt < 80)
        {
            yield return new WaitForSeconds(DELAY_BACKGROUND);

            topBackground.transform.localPosition = new Vector2(topBackground.transform.localPosition.x + 4, topBackground.transform.localPosition.y + 4);
            downBackground.transform.localPosition = new Vector2(downBackground.transform.localPosition.x - 4, downBackground.transform.localPosition.y - 4);

            cpt++;
        }
    }

    public void setBackgroundOnMenuStart()
    {
        topBackground.transform.localPosition = new Vector2(0, 0);
        downBackground.transform.localPosition = new Vector2(0, 0);

        callUnshowBackground(0.5f);
    }

    public void setBackgroundOnGameStart()
    {
        topBackground.transform.localPosition = new Vector2(0, 0);
        downBackground.transform.localPosition = new Vector2(0, 0);

        StartCoroutine(IShowInfoSkip());

        ruleSlot.SetActive(true);
        StartCoroutine(FadeOnRule());
        placeRule();
    }

    private IEnumerator IShowInfoSkip()
    {
        yield return new WaitForSeconds(DELAY_SKIP_RULE);

        infoSkip.SetActive(true);
        Image imgInfoSkip = infoSkip.GetComponent<Image>();
        StartCoroutine(IAlphaColorInfoSkip(imgInfoSkip));
        canSkip = true;
    }

    private IEnumerator FadeOnRule()
    {
        Image imgRule = ruleSlot.GetComponent<Image>();

        Color color = new Color(1f, 1f, 1f, 0);
        imgRule.color = color;

        float a = 0f;

        while (a < 1f)
        {
            color = new Color(1f, 1f, 1f, a);
            imgRule.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator IAlphaColorInfoSkip(Image _img)
    {
        float a = 0f;

        while (a < 1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            _img.color = color;
            a += 0.02f;

            yield return new WaitForSeconds(0.01f);
        }

        while (a > 0.1f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            _img.color = color;
            a -= 0.01f;

            yield return new WaitForSeconds(0.01f);
        }

        if(infoSkip.activeSelf)
        {
            StartCoroutine(IAlphaColorInfoSkip(_img));
        }
    }

    private void placeRule()
    {
        int choice = Random.Range(0, rules.Length);
        Sprite choosedSprite = rules[choice];

        ruleSlot.GetComponent<Image>().sprite = choosedSprite;
    }
}
