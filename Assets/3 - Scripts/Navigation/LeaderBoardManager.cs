using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Save;
using LoadSave;

public class LeaderBoardManager : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [Header("Components")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject placeHolder;
    [SerializeField] private Transform leftArrow;
    [SerializeField] private Transform rightArrow;
    [SerializeField] private TextMeshProUGUI[] pseudoBestPlayer;
    [SerializeField] private TextMeshProUGUI[] scoreBestPlayer;

    [HideInInspector] public string currentPseudo = "";

    private bool arrowsAreMoving = false;

    // =====================================================

    private void Start()
    {
        sortingScore();

        showBestPlayers();
        showBestScores();
    }

    private void showBestPlayers()
    {
        for(int i = 0; i < pseudoBestPlayer.Length; i++)
        {
            string pseudo = LoadSave.LoadSave.LoadBestPlayerAtRank(i);
            pseudoBestPlayer[i].text = pseudo;
        }
    }

    private void showBestScores()
    {
        for (int i = 0; i < scoreBestPlayer.Length; i++)
        {
            int score = LoadSave.LoadSave.LoadBestScoreAtRank(i);
            scoreBestPlayer[i].text = "- " + score.ToString() + " -";
        }
    }

    public bool checkEmpty()
    {
        return (inputField.text == "" || inputField.text == " ");
    }

    public void activePlaceHolder()
    {
        placeHolder.SetActive(true);
    }

    public string getPseudo()
    {
        return inputField.text;
    }

    public void callArrowsAnim()
    {
        if (!arrowsAreMoving) { StartCoroutine(IArrowsAnim()); }
    }

    private IEnumerator IArrowsAnim()
    {
        arrowsAreMoving = true;
        int cpt = 0;

        while(cpt < 20)
        {
            leftArrow.localPosition = new Vector2(leftArrow.localPosition.x - 1, leftArrow.localPosition.y);
            rightArrow.localPosition = new Vector2(rightArrow.localPosition.x + 1, rightArrow.localPosition.y);

            yield return new WaitForSeconds(0.01f);

            cpt++;
        }

        while (cpt > 0)
        {
            leftArrow.localPosition = new Vector2(leftArrow.localPosition.x + 1, leftArrow.localPosition.y);
            rightArrow.localPosition = new Vector2(rightArrow.localPosition.x - 1, rightArrow.localPosition.y);

            yield return new WaitForSeconds(0.01f);

            cpt--;
        }

        while (cpt < 20)
        {
            leftArrow.localPosition = new Vector2(leftArrow.localPosition.x - 1, leftArrow.localPosition.y);
            rightArrow.localPosition = new Vector2(rightArrow.localPosition.x + 1, rightArrow.localPosition.y);

            yield return new WaitForSeconds(0.01f);

            cpt++;
        }

        while (cpt > 0)
        {
            leftArrow.localPosition = new Vector2(leftArrow.localPosition.x + 1, leftArrow.localPosition.y);
            rightArrow.localPosition = new Vector2(rightArrow.localPosition.x - 1, rightArrow.localPosition.y);

            yield return new WaitForSeconds(0.01f);

            cpt--;
        }

        while (cpt < 20)
        {
            leftArrow.localPosition = new Vector2(leftArrow.localPosition.x - 1, leftArrow.localPosition.y);
            rightArrow.localPosition = new Vector2(rightArrow.localPosition.x + 1, rightArrow.localPosition.y);

            yield return new WaitForSeconds(0.01f);

            cpt++;
        }

        while (cpt > 0)
        {
            leftArrow.localPosition = new Vector2(leftArrow.localPosition.x + 1, leftArrow.localPosition.y);
            rightArrow.localPosition = new Vector2(rightArrow.localPosition.x - 1, rightArrow.localPosition.y);

            yield return new WaitForSeconds(0.01f);

            cpt--;
        }

        arrowsAreMoving = false;
    }

    private void sortingScore()
    {
        int score01 = LoadSave.LoadSave.LoadBestScoreAtRank(1);
        int score02 = LoadSave.LoadSave.LoadBestScoreAtRank(2);
        int score03 = LoadSave.LoadSave.LoadBestScoreAtRank(3);
        int scorePlayer = LoadSave.LoadSave.LoadPlayerScore();
        int scoreTemp = 0;

        string pseudo01 = LoadSave.LoadSave.LoadBestPlayerAtRank(1);
        string pseudo02 = LoadSave.LoadSave.LoadBestPlayerAtRank(2);
        string pseudo03 = LoadSave.LoadSave.LoadBestPlayerAtRank(3);
        string pseudoPlayer = LoadSave.LoadSave.LoadPlayerPseudo();
        string pseudoTemp = "";

        if (scorePlayer > score03)
        {
            score03 = scorePlayer;
            pseudo03 = pseudoPlayer;
        }

        if (score03 > score02)
        {
            scoreTemp = score02;
            pseudoTemp = pseudo02;

            score02 = score03;
            pseudo02 = pseudo03;

            score03 = scoreTemp;
            pseudo03 = pseudoTemp;
        }

        if (score02 > score01)
        {
            scoreTemp = score01;
            pseudoTemp = pseudo01;

            score01 = score02;
            pseudo01 = pseudo02;

            score02 = scoreTemp;
            pseudo02 = pseudoTemp;
        }

        Save.Save.SaveBestScoreAtRank(score01, 1);
        Save.Save.SaveBestScoreAtRank(score02, 2);
        Save.Save.SaveBestScoreAtRank(score03, 3);
        Save.Save.SavePlayerScore(0);

        Save.Save.SaveBestPlayerAtRank(pseudo01, 1);
        Save.Save.SaveBestPlayerAtRank(pseudo02, 2);
        Save.Save.SaveBestPlayerAtRank(pseudo03, 3);
        Save.Save.SavePlayerPseudo("");
    }
}
