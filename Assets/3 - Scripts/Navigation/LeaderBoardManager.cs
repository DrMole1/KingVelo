using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Save;
using LoadSave;

public class LeaderBoardManager : MonoBehaviour
{
    private Color32 skin01 = new Color(1f, 0.89f, 0.77f, 1f);
    private Color32 skin02 = new Color(0.99f, 0.9f, 0.67f, 1f);
    private Color32 skin03 = new Color(0.57f, 0.37f, 0.22f, 1f);
    private Color32 skin04 = new Color(0.37f, 0.2f, 0.06f, 1f);


    // ===================== VARIABLES =====================

    [Header("Components")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject placeHolder;
    [SerializeField] private Transform leftArrow;
    [SerializeField] private Transform rightArrow;
    [SerializeField] private TextMeshProUGUI[] pseudoBestPlayer;
    [SerializeField] private TextMeshProUGUI[] scoreBestPlayer;
    [SerializeField] private Transform[] avatarBestPlayer;

    [Header("Avatar Sprites")]
    [SerializeField] private Sprite[] shapes;
    [SerializeField] private Sprite[] mouths;
    [SerializeField] private Sprite[] noses;
    [SerializeField] private Sprite[] eyes;
    [SerializeField] private Sprite[] hairs;

    private Color32[] skinsColor = new Color32[4];

    [HideInInspector] public string currentPseudo = "";

    private bool arrowsAreMoving = false;

    // =====================================================

    private void Awake()
    {
        skinsColor[0] = skin01;
        skinsColor[1] = skin02;
        skinsColor[2] = skin03;
        skinsColor[3] = skin04;
    }

    private void Start()
    {
        sortingScore();

        showBestPlayers();
        showBestScores();
        showBestAvatar();
    }

    private void showBestPlayers()
    {
        for(int i = 0; i < pseudoBestPlayer.Length; i++)
        {
            string pseudo = LoadSave.LoadSave.LoadBestPlayerAtRank(i + 1);
            pseudoBestPlayer[i].text = pseudo;
        }
    }

    private void showBestScores()
    {
        for (int i = 0; i < scoreBestPlayer.Length; i++)
        {
            int score = LoadSave.LoadSave.LoadBestScoreAtRank(i + 1);
            scoreBestPlayer[i].text = "- " + score.ToString() + " -";
        }
    }

    private void showBestAvatar()
    {
        for (int i = 0; i < avatarBestPlayer.Length; i++)
        {
            avatarBestPlayer[i].gameObject.SetActive(false);
            string avatar = LoadSave.LoadSave.LoadCombinationAtRank(i + 1);
            if(avatar != "" && avatar != " ") 
            { 
                avatarBestPlayer[i].gameObject.SetActive(true);

                int faceValue = int.Parse(avatar.Substring(0, 1));
                int skinValue = int.Parse(avatar.Substring(1, 1));
                int mouthValue = int.Parse(avatar.Substring(2, 1));
                int noseValue = int.Parse(avatar.Substring(3, 1));
                int eyesValue = int.Parse(avatar.Substring(4, 1));
                int hairValue = int.Parse(avatar.Substring(5, 1));

                avatarBestPlayer[i].GetChild(0).GetComponent<Image>().sprite = shapes[faceValue];
                avatarBestPlayer[i].GetChild(1).GetComponent<Image>().sprite = mouths[mouthValue];
                avatarBestPlayer[i].GetChild(2).GetComponent<Image>().sprite = noses[noseValue];
                avatarBestPlayer[i].GetChild(3).GetComponent<Image>().sprite = eyes[eyesValue];
                avatarBestPlayer[i].GetChild(4).GetComponent<Image>().sprite = hairs[hairValue];
                avatarBestPlayer[i].GetChild(0).GetComponent<Image>().color = skinsColor[skinValue];
            }
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

        string avatar01 = LoadSave.LoadSave.LoadCombinationAtRank(1);
        string avatar02 = LoadSave.LoadSave.LoadCombinationAtRank(2);
        string avatar03 = LoadSave.LoadSave.LoadCombinationAtRank(3);
        string avatarPlayer = LoadSave.LoadSave.LoadCombinationString();
        string avatarTemp = "";

        if (scorePlayer > score03)
        {
            score03 = scorePlayer;
            pseudo03 = pseudoPlayer;
            avatar03 = avatarPlayer;
        }

        if (score03 > score02)
        {
            scoreTemp = score02;
            pseudoTemp = pseudo02;
            avatarTemp = avatar02;

            score02 = score03;
            pseudo02 = pseudo03;
            avatar02 = avatar03;

            score03 = scoreTemp;
            pseudo03 = pseudoTemp;
            avatar03 = avatarTemp;
        }

        if (score02 > score01)
        {
            scoreTemp = score01;
            pseudoTemp = pseudo01;
            avatarTemp = avatar01;

            score01 = score02;
            pseudo01 = pseudo02;
            avatar01 = avatar02;

            score02 = scoreTemp;
            pseudo02 = pseudoTemp;
            avatar02 = avatarTemp;
        }

        Save.Save.SaveBestScoreAtRank(score01, 1);
        Save.Save.SaveBestScoreAtRank(score02, 2);
        Save.Save.SaveBestScoreAtRank(score03, 3);
        Save.Save.SavePlayerScore(0);

        Save.Save.SaveBestPlayerAtRank(pseudo01, 1);
        Save.Save.SaveBestPlayerAtRank(pseudo02, 2);
        Save.Save.SaveBestPlayerAtRank(pseudo03, 3);
        Save.Save.SavePlayerPseudo("");

        Save.Save.SaveBestCombinationAtRank(avatar01, 1);
        Save.Save.SaveBestCombinationAtRank(avatar02, 2);
        Save.Save.SaveBestCombinationAtRank(avatar03, 3);
    }
}
