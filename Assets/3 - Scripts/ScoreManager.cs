using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private const float DELAY_RESET_STREAK = 2f;
    private const float DELAY = 0.01f;

    // ====================== VARIABLES ======================

    [SerializeField] private int currentScore = 0;
    [SerializeField] private TextMeshProUGUI txtScore;

    private int streak = 0;
    private Coroutine streakCoroutine;
    private bool textIsGrowing = false;

    // =======================================================

    public void setStreak(int _streak) { streak = _streak; }
    public int getStreak() { return streak; }
    public void setCurrentScore(int _score) { currentScore = _score; }
    public int getScore() { return currentScore; }


    public void addScore(int _value, bool _withStreak)
    {
        if(_withStreak)
        {
            streak++;
            if (streakCoroutine != null) { StopCoroutine(streakCoroutine); }
            streakCoroutine = StartCoroutine(IResetStreak());
        }

        currentScore += _value;

        StartCoroutine(IUpdateScore());
        if(!textIsGrowing) { StartCoroutine(IGrowText()); }
    }

    private IEnumerator IResetStreak()
    {
        yield return new WaitForSeconds(DELAY_RESET_STREAK);

        streak = 0;
    }

    private IEnumerator IUpdateScore()
    {
        int scoreValue = int.Parse(txtScore.text);

        while(currentScore > scoreValue)
        {
            scoreValue++;
            txtScore.text = scoreValue.ToString();

            yield return new WaitForSeconds(DELAY);
        }
    }

    private IEnumerator IGrowText()
    {
        textIsGrowing = true;

        while(txtScore.fontSize < 40)
        {
            yield return new WaitForSeconds(DELAY);
            txtScore.fontSize += 1;
        }

        while (txtScore.fontSize > 30)
        {
            yield return new WaitForSeconds(DELAY);
            txtScore.fontSize -= 1;
        }

        textIsGrowing = false;
    }
}
