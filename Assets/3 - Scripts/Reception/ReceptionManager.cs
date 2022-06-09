using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Save;

public class ReceptionManager : MonoBehaviour
{
    private const float FACTOR_SCALE = 1.5f;

    // ===================== VARIABLES =====================

    [Header("Name of the Navigation Scene")]
    [SerializeField] private string nameNavigationScene;

    [Header("Drag the Transition Manager")]
    [SerializeField] private TransitionManager transitionManager;

    [Header("Drag the Play Button")]
    [SerializeField] private RectTransform button;

    private bool canUseButton = true;

    private SoundManager soundManager;

    // =====================================================

    private void Awake()
    {
        Save.Save.SaveStartElements();

        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    private void Update()
    {
        if(Input.GetKeyDown("a"))
        {
            PlayerPrefs.DeleteAll();
        }
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
        float addScaleX = (maxScaleX - minScaleX) / 15;
        float addScaleY = (maxScaleY - minScaleY) / 15;

        while(button.sizeDelta.x < maxScaleX)
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
