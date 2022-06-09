using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private const float FACTOR_SCALE = 1.5f;

    // ====================== VARIABLES ======================

    [SerializeField] private Transform canvasRoot;
    [SerializeField] private Image[] backgrounds;
    [SerializeField] private GameObject[] btnNext;
    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject hiddenObjects;

    [Header("Name of the Navigation Scene")]
    [SerializeField] private string nameNavigationScene;

    [Header("Drag the Transition Manager")]
    [SerializeField] private TransitionManager transitionManager;

    [Header("Drag the Play Button")]
    [SerializeField] private RectTransform button;

    private bool canUseButton = true;

    private SoundManager soundManager;
    private MusicManager musicManager;

    private int state = 0;

    // =======================================================

    private void Awake()
    {
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
        if (GameObject.Find("MusicManager") != null) { musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>(); }
    }


    public void doState(int _id)
    {
        Camera.main.GetComponent<CameraShake>().StopShaking();
        GetComponent<PauseSystem>().canPause = false;

        canvasRoot.GetChild(state).gameObject.SetActive(true);

        if(state == 4) { minimap.SetActive(true); }
        if(state == 6) { hiddenObjects.SetActive(true); }

        showPannel();
    }


    public void goToNextState()
    {
        Time.timeScale = 1;

        Camera.main.GetComponent<CameraShake>().StopShaking();
        GetComponent<PauseSystem>().canPause = true;

        canvasRoot.GetChild(state).gameObject.SetActive(false);

        state++;
    }

    private void showPannel()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            Color color = new Color(0f, 0f, 0f, 0);
            backgrounds[i].color = color;

            StartCoroutine(IBlackBackground(i));
        }

        btnNext[state].SetActive(true);

        Time.timeScale = 0;
    }

    private IEnumerator IBlackBackground(int _id)
    {
        Color color = new Color(0f, 0f, 0f, 0);
        float a = 0f;

        while (a < 0.9f)
        {
            color = new Color(0f, 0f, 0f, a);
            backgrounds[_id].color = color;
            a += 0.02f;

            yield return null;
        }
    }

    public void goToNavigationScene()
    {
        Time.timeScale = 1;
        if (canUseButton) 
        { 
            StartCoroutine(IAnimButton()); 
            if (soundManager != null) { soundManager.playAudioClip(6); }
            if (musicManager != null) { musicManager.callChangeMusic(0); }
        }
    }

    private IEnumerator IAnimButton()
    {
        canUseButton = false;

        float minScaleX = button.sizeDelta.x;
        float minScaleY = button.sizeDelta.y;
        float maxScaleX = minScaleX * FACTOR_SCALE;
        float maxScaleY = minScaleY * FACTOR_SCALE;
        float addScaleX = (maxScaleX - minScaleX) / 15;
        float addScaleY = (maxScaleY - minScaleY) / 15;

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
}
