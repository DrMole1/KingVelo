using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Save;
using LoadSave;

public class GameManager : MonoBehaviour
{
    private float DELAY_TO_RUN_PLAYER = 3f;
    private float DELAY = 0.01f;
    private const float FACTOR_SCALE = 1.3f;

    // ====================== VARIABLES ======================

    [Header("Player Movement")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Canvas")]
    [SerializeField] private RectTransform endHeadband;
    [SerializeField] private Transform letters;
    [SerializeField] private Transform circles;
    [SerializeField] private Transform endMenu;
    [SerializeField] private Transform winLetters;
    [SerializeField] private Transform loseLetters;
    [SerializeField] private Transform scorePannel;
    [SerializeField] private GameObject panelAlpha;
    [SerializeField] private GameObject panelConverter;
    [SerializeField] private TextMeshProUGUI txtScoreConverter;
    [SerializeField] private TextMeshProUGUI txtCoinsConverter;
    [SerializeField] private GameObject btnNext;

    [Header("Name of the Navigation Scene")]
    [SerializeField] private string nameNavigationScene;

    [Header("Drag the Transition Manager")]
    [SerializeField] private TransitionManager transitionManager;

    [Header("Drag the Play Button")]
    [SerializeField] private RectTransform button;

    private bool canUseButton = true;

    private SoundManager soundManager;
    private Camera cam;
    [HideInInspector] public bool isGameFinished = false;
    private bool hasStarted = false;
    private bool canConvert = true;

    // =======================================================


    private void Awake()
    {
        cam = Camera.main;
        playerMovement.gameManager = this;
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }


    private void Update()
    {
        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && !hasStarted)
        {
            cam.GetComponent<CameraShake>().callIntroTranslation();

            StartCoroutine(IRunPlayer());

            hasStarted = true;
        }
    }

    private IEnumerator IRunPlayer()
    {
        yield return new WaitForSeconds(DELAY_TO_RUN_PLAYER);

        playerMovement.state = PlayerMovement.State.Run;
    }

    public void callStopPlayer(bool _win)
    {
        isGameFinished = true;

        StartCoroutine(IStopPlayer(_win));
    }

    private IEnumerator IStopPlayer(bool _win)
    {
        while (playerMovement.getSpeed() > 0)
        {
            playerMovement.setSpeed(playerMovement.getSpeed() - 0.1f);

            yield return new WaitForSeconds(0.02f);
        }

        playerMovement.state = PlayerMovement.State.Stop;

        StartCoroutine(IShowEndHeadband(_win));
    }

    private IEnumerator IShowEndHeadband(bool _win)
    {
        yield return new WaitForSeconds(1f);

        while(endHeadband.localPosition.y < 50f)
        {
            endHeadband.localPosition = new Vector2(endHeadband.localPosition.x, endHeadband.localPosition.y + 10f);
            yield return new WaitForSeconds(DELAY);
        }

        while (endHeadband.localPosition.y > 0f)
        {
            endHeadband.localPosition = new Vector2(endHeadband.localPosition.x, endHeadband.localPosition.y - 10f);
            yield return new WaitForSeconds(DELAY);
        }

        yield return new WaitForSeconds(0.25f);

        StartCoroutine(ICallGrowLetters());
        StartCoroutine(ICallGrowCircles());

        yield return new WaitForSeconds(2.5f);

        while (endHeadband.localPosition.y < 50f)
        {
            endHeadband.localPosition = new Vector2(endHeadband.localPosition.x, endHeadband.localPosition.y + 10f);
            yield return new WaitForSeconds(DELAY);
        }

        while (endHeadband.localPosition.y > -400f)
        {
            endHeadband.localPosition = new Vector2(endHeadband.localPosition.x, endHeadband.localPosition.y - 10f);
            yield return new WaitForSeconds(DELAY);
        }

        showEndPannel(_win);
    }

    private IEnumerator ICallGrowLetters()
    {
        for (int i = 0; i < letters.childCount; i++)
        {
            StartCoroutine(IGrowLetter(i, letters));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ICallGrowCircles()
    {
        for (int i = 0; i < circles.childCount; i++)
        {
            StartCoroutine(IGrowCircle(i));
            yield return new WaitForSeconds(0.4f);
        }
    }

    private IEnumerator IGrowLetter(int _i, Transform tr)
    {
        while (tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta.x < 70)
        {
            tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta = new Vector2(tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta.x + 1.2f, tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta.y + 1.2f);

            yield return new WaitForSeconds(DELAY);
        }

        while (tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta.x > 50)
        {
            tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta = new Vector2(tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta.x - 1f, tr.GetChild(_i).GetComponent<RectTransform>().sizeDelta.y - 1f);

            yield return new WaitForSeconds(DELAY);
        }
    }

    private IEnumerator IGrowCircle(int _i)
    {
        while (circles.GetChild(_i).GetComponent<RectTransform>().sizeDelta.x < 3000)
        {
            circles.GetChild(_i).GetComponent<RectTransform>().sizeDelta = new Vector2(circles.GetChild(_i).GetComponent<RectTransform>().sizeDelta.x + 50f, circles.GetChild(_i).GetComponent<RectTransform>().sizeDelta.y + 50f);

            yield return new WaitForSeconds(DELAY);
        }

        circles.GetChild(_i).gameObject.SetActive(false);
    }

    private void showEndPannel(bool _win)
    {
        panelAlpha.SetActive(true);
        endMenu.gameObject.SetActive(true);
        endMenu.localScale = new Vector2(0f, 0f);

        if (_win) { winLetters.gameObject.SetActive(true); }
        else { loseLetters.gameObject.SetActive(true); }

        StartCoroutine(IGrowEndPannel());

        if(winLetters.gameObject.activeSelf) { StartCoroutine(IAnimateWinLetters()); }
    }

    private IEnumerator IGrowEndPannel()
    {
        while (endMenu.localScale.x < 1)
        {
            endMenu.localScale = new Vector3(endMenu.localScale.x + 0.02f, endMenu.localScale.y + 0.02f, endMenu.localScale.z + 0.02f);

            yield return new WaitForSeconds(DELAY);
        }

        StartCoroutine(IShowScore());
    }

    private IEnumerator IAnimateWinLetters()
    {
        for (int i = 0; i < winLetters.childCount; i++)
        {
            StartCoroutine(IGrowLetter(i, winLetters));
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(IAnimateWinLetters());
    }

    private IEnumerator IShowScore()
    {
        scorePannel.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        TextMeshProUGUI text = scorePannel.GetChild(0).GetComponent<TextMeshProUGUI>();
        ScoreManager scoreManager = GetComponent<ScoreManager>();
        int scoreToShow = int.Parse(text.text);

        StartCoroutine(IGrowTextSize(text));

        while(scoreToShow <= scoreManager.getScore())
        {
            text.text = scoreToShow.ToString();
            scoreToShow += 2;

            yield return new WaitForSeconds(DELAY / calculateFactor(GetComponent<ScoreManager>().getScore()));
        }

        yield return new WaitForSeconds(1f);

        panelConverter.SetActive(true);
        txtScoreConverter.text = text.text;
        txtCoinsConverter.text = LoadSave.LoadSave.LoadCurrentCoins().ToString();
        Save.Save.SavePlayerScore(scoreToShow - 2);
    }

    private IEnumerator IGrowTextSize(TextMeshProUGUI _text)
    {
        while (_text.fontSize < 44)
        {
            _text.fontSize++;

            yield return new WaitForSeconds(DELAY);
        }

        while (_text.fontSize > 34)
        {
            _text.fontSize--;

            yield return new WaitForSeconds(DELAY);
        }
    }

    public void convert()
    {
        if(canConvert) { StartCoroutine(IConvertor()); }
    }

    private IEnumerator IConvertor()
    {
        if(soundManager != null) { soundManager.playAudioClip(12); }

        canConvert = false;
        int scoreToConvert = int.Parse(txtScoreConverter.text);
        int currentCoins = int.Parse(txtCoinsConverter.text);
        int cpt = 0;

        while (scoreToConvert > 0)
        {
            scoreToConvert -= 2;
            cpt += 2;

            if(cpt == 10)
            {
                currentCoins++;
                cpt = 0;
            }

            txtScoreConverter.text = scoreToConvert.ToString();
            txtCoinsConverter.text = currentCoins.ToString();

            yield return new WaitForSeconds(DELAY / calculateFactor(GetComponent<ScoreManager>().getScore()));
        }

        yield return new WaitForSeconds(1);

        Save.Save.SaveCurrentCoins(currentCoins);

        btnNext.SetActive(true);
    }

    public void goToNavigationScene()
    {
        if (canUseButton) { StartCoroutine(IAnimButton()); if (soundManager != null) { soundManager.playAudioClip(6); } }
    }

    private IEnumerator IAnimButton()
    {
        canUseButton = false;

        float minScaleX = button.sizeDelta.x;
        float minScaleY = button.sizeDelta.y;
        float maxScaleX = minScaleX * FACTOR_SCALE;
        float maxScaleY = minScaleY * FACTOR_SCALE;
        float addScaleX = (maxScaleX - minScaleX) / 20;
        float addScaleY = (maxScaleY - minScaleY) / 20;

        while (button.sizeDelta.x < maxScaleX)
        {
            yield return new WaitForSeconds(0.001f);

            button.sizeDelta = new Vector2(button.sizeDelta.x + addScaleX, button.sizeDelta.y + addScaleY);
        }

        while (button.sizeDelta.x > minScaleX)
        {
            yield return new WaitForSeconds(0.001f);

            button.sizeDelta = new Vector2(button.sizeDelta.x - addScaleX, button.sizeDelta.y - addScaleY);
        }

        transitionManager.callShowBackground(0.3f);

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(nameNavigationScene);
    }

    private float calculateFactor(int _score)
    {
        float factor = 1f;

        if(_score > 200)
        {
            factor = (float)_score / 100f;
        }

        return factor;
    }
}
