using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // =====================================================

    public void goToNavigationScene()
    {
        if (canUseButton) { StartCoroutine(IAnimButton()); }
    }

    private IEnumerator IAnimButton()
    {
        canUseButton = false;

        float minScaleX = button.sizeDelta.x;
        float minScaleY = button.sizeDelta.y;
        float maxScaleX = minScaleX * FACTOR_SCALE;
        float maxScaleY = minScaleY * FACTOR_SCALE;
        float addScaleX = (maxScaleX - minScaleX) / 25;
        float addScaleY = (maxScaleY - minScaleY) / 25;

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
