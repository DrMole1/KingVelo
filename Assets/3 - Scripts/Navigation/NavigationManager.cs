using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Save;
using LoadSave;

public class NavigationManager : MonoBehaviour
{
    private const float FACTOR_SCALE = 1.5f;
    private const float DELAY_TO_USE = 0.25f;

    // ===================== VARIABLES =====================

    [Header("Name of Scenes")]
    [SerializeField] private string nameShopScene;
    [SerializeField] private string nameTutoScene;
    [SerializeField] private string nameGameScene;

    [Header("Drag the Transition Manager")]
    [SerializeField] private TransitionManager transitionManager;

    [Header("Drag Buttons")]
    [SerializeField] private RectTransform[] buttons;
    // 0 = Tuto / 1 = Jouer / 2 = Boutique / 3 = Paramètres / 4 = Crédits / 5 = Kingersheim / 6 = Modifier / 7 = Quitter / 8 = LeaderBoard / 9 = Portfolio

    [Header("Panels")]
    [SerializeField] private GameObject[] panels;
    // 0 = LeaderBoard / 1 = Paramètres / 2 = Crédits / 3 = ModifierAvatar

    [Header("Components of the Scene")]
    [SerializeField] private TextMeshProUGUI currentCoins;

    private bool canUseButton = true;
    private LeaderBoardManager leaderBoardManager;
    private SoundManager soundManager;

    // =====================================================

    private void Awake()
    {
        leaderBoardManager = GetComponent<LeaderBoardManager>();
        if(GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    private void Start()
    {
        currentCoins.text = LoadSave.LoadSave.LoadCurrentCoins().ToString();
        showLeaderBoardOnStart();
    }

    public void goToTuto()
    {
        if(!canUseButton) { return; }

        Save.Save.SaveCombination();
        if (soundManager != null) { soundManager.playAudioClip(6); }
        StartCoroutine(IAnimButton(buttons[0]));
        StartCoroutine(IGoToScene(2.5f, nameTutoScene));
    }

    public void goToGame()
    {
        if (!canUseButton) { return; }

        if(leaderBoardManager.checkEmpty())
        {
            showPanel(0);
            leaderBoardManager.callArrowsAnim();
            leaderBoardManager.activePlaceHolder();
            if(soundManager != null) { soundManager.playAudioClip(0); }
            StartCoroutine(IResetCanUse(DELAY_TO_USE));
            return;
        }

        Save.Save.SaveCombination();
        if (soundManager != null) { soundManager.playAudioClip(6); }
        Save.Save.SavePlayerPseudo(leaderBoardManager.getPseudo());
        StartCoroutine(IAnimButton(buttons[1]));
        StartCoroutine(IGoToScene(2.5f, nameGameScene));
    }

    public void goToBoutique()
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(6); }
        Save.Save.SaveCombination();
        StartCoroutine(IAnimButton(buttons[2]));
        StartCoroutine(IGoToScene(2.5f, nameShopScene));
    }

    public void showLeaderBoardPanel()
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttons[8]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));

        showPanel(0);
    }

    public void showSettings()
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttons[3]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));

        showPanel(1);
    }

    public void showCredits()
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttons[4]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));

        showPanel(2);
    }

    public void showModifyPanel()
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttons[6]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));

        showPanel(3);
    }

    public void showKingersheim()
    {
        if (!canUseButton) { return; }

        Application.OpenURL("https://www.ville-kingersheim.fr/");

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttons[5]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));
    }

    public void quitGame()
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(5); }
        StartCoroutine(IAnimButton(buttons[7]));
        StartCoroutine(IQuitGame(DELAY_TO_USE));
    }

    private IEnumerator IAnimButton(RectTransform _btn)
    {
        canUseButton = false;

        float minScaleX = _btn.sizeDelta.x;
        float minScaleY = _btn.sizeDelta.y;
        float maxScaleX = minScaleX * FACTOR_SCALE;
        float maxScaleY = minScaleY * FACTOR_SCALE;
        float addScaleX = (maxScaleX - minScaleX) / 15;
        float addScaleY = (maxScaleY - minScaleY) / 15;

        while(_btn.sizeDelta.x < maxScaleX)
        {
            yield return new WaitForSeconds(0.001f);

            _btn.sizeDelta = new Vector2(_btn.sizeDelta.x + addScaleX, _btn.sizeDelta.y + addScaleY);
        }

        while (_btn.sizeDelta.x > minScaleX)
        {
            yield return new WaitForSeconds(0.001f);

            _btn.sizeDelta = new Vector2(_btn.sizeDelta.x - addScaleX, _btn.sizeDelta.y - addScaleY);
        }
    }

    private IEnumerator IGoToScene(float _delay, string _nameScene)
    {
        transitionManager.callShowBackground(0.3f);

        yield return new WaitForSeconds(_delay); // 2.5f for normal delay

        SceneManager.LoadScene(_nameScene);
    }

    private IEnumerator IQuitGame(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        Application.Quit();
    }

    private IEnumerator IResetCanUse(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        canUseButton = true;
    }

    private void showPanel(int _idPanel)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if(_idPanel == i) { panels[i].SetActive(true); }
            else { panels[i].SetActive(false); }

            if (_idPanel == 0) { StartCoroutine(IShowButtonOk(0, false)); }
            else { StartCoroutine(IShowButtonOk(DELAY_TO_USE, true)); }
        }
    }

    private void showLeaderBoardOnStart()
    {
        panels[0].SetActive(true);
    }

    private IEnumerator IShowButtonOk(float _delay, bool _mustShow)
    {
        yield return new WaitForSeconds(_delay);

        GameObject buttonOk = buttons[8].gameObject;
        buttonOk.SetActive(_mustShow);
    }

    public void showPortfolio()
    {
        if (!canUseButton) { return; }

        Application.OpenURL("https://drmole1.github.io/");

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttons[9]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));
    }
}
