using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Save;
using LoadSave;

public class AvatarManager : MonoBehaviour
{
    private const float FACTOR_SCALE = 1.5f;
    private const float DELAY_TO_USE = 0.5f;

    private Color32 skin01 = new Color(1f, 0.89f, 0.77f, 1f);
    private Color32 skin02 = new Color(0.99f, 0.9f, 0.67f, 1f);
    private Color32 skin03 = new Color(0.57f, 0.37f, 0.22f, 1f);
    private Color32 skin04 = new Color(0.37f, 0.2f, 0.06f, 1f);

    // ===================== VARIABLES =====================

    [Header("Drag Buttons")]
    [SerializeField] private RectTransform[] buttonsCat;
    // 0 = Visage / 1 = Peau / 2 = Bouche / 3 = Nez / 4 = Yeux / 5 = Cheveux_01 / 6 = Cheveux_02
    [SerializeField] private RectTransform[] buttonsChoice;

    [Header("Drag Image Buttons")]
    [SerializeField] private Image[] imgCat;
    // 0 = Visage / 1 = Peau / 2 = Bouche / 3 = Nez / 4 = Yeux / 5 = Cheveux_01 / 6 = Cheveux_02
    [SerializeField] private Image[] imgChoice;

    [Header("Drag Avatar Component")]
    [SerializeField] private Image[] imgAvatar;
    // 0 = Visage / 1 = Bouche / 2 = Nez / 3 = Yeux / 4 = Cheveux

    [Header("Avatar Sprites")]
    [SerializeField] private Sprite[] shapes;
    [SerializeField] private Sprite[] skins;
    [SerializeField] private Sprite[] mouths;
    [SerializeField] private Sprite[] noses;
    [SerializeField] private Sprite[] eyes;
    [SerializeField] private Sprite[] hairs01;
    [SerializeField] private Sprite[] hairs02;

    [Header("Objects in Scene")]
    [SerializeField] private GameObject choicePanel;

    private int currentCat;
    private Color32[] skinsColor = new Color32[4];

    private bool canUseButton = true;
    private SoundManager soundManager;

    // =====================================================

    private void Awake()
    {
        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }

        skinsColor[0] = skin01;
        skinsColor[1] = skin02;
        skinsColor[2] = skin03;
        skinsColor[3] = skin04;

        loadAvatar();
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

        while (_btn.sizeDelta.x < maxScaleX)
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

    private IEnumerator IResetCanUse(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        canUseButton = true;
    }

    public void showCategory(int _id)
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttonsCat[_id]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));

        choicePanel.SetActive(true);
        currentCat = _id;
        Sprite[] sprAvatar = selectSprites();

        for(int i = 0; i < imgChoice.Length; i++)
        {
            imgChoice[i].sprite = sprAvatar[i];
        }
    }

    private Sprite[] selectSprites()
    {
        Sprite[] tempSpr = new Sprite[9];

        switch (currentCat)
        {
            case 0:
                tempSpr = shapes;
                break;
            case 1:
                tempSpr = skins;
                break;
            case 2:
                tempSpr = mouths;
                break;
            case 3:
                tempSpr = noses;
                break;
            case 4:
                tempSpr = eyes;
                break;
            case 5:
                tempSpr = hairs01;
                break;
            case 6:
                tempSpr = hairs02;
                break;
        }

        return tempSpr;
    }

    public void selectItem(int _id)
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(1); }
        StartCoroutine(IAnimButton(buttonsChoice[_id]));
        StartCoroutine(IResetCanUse(DELAY_TO_USE));

        Sprite[] sprAvatar = selectSprites();

        imgCat[currentCat].sprite = sprAvatar[_id];

        int avatarElement = selectAvatarElement();

        if(avatarElement != -1) // Pour les sprites
        {
            imgAvatar[avatarElement].sprite = sprAvatar[_id];
        }
        else // Pour la couleur du visage
        {
            imgAvatar[0].color = skinsColor[_id];
        }

        saveAvatarElement(_id);
    }

    private int selectAvatarElement()
    {
        int tempElement = 0;

        switch (currentCat)
        {
            case 0:
                tempElement = 0;
                break;
            case 1:
                tempElement = -1;
                break;
            case 2:
                tempElement = 1;
                break;
            case 3:
                tempElement = 2;
                break;
            case 4:
                tempElement = 3;
                break;
            case 5:
                tempElement = 4;
                break;
            case 6:
                tempElement = 4;
                break;
        }

        return tempElement;
    }

    private void saveAvatarElement(int _id)
    {
        switch (currentCat)
        {
            case 0:
                Save.Save.SaveFace(_id);
                break;
            case 1:
                Save.Save.SaveSkin(_id);
                break;
            case 2:
                Save.Save.SaveMouth(_id);
                break;
            case 3:
                Save.Save.SaveNose(_id);
                break;
            case 4:
                Save.Save.SaveEyes(_id);
                break;
            case 5:
                Save.Save.SaveHair(_id);
                break;
            case 6:
                Save.Save.SaveHair(_id + 4);
                break;
        }
    }

    private void loadAvatar()
    {
        imgAvatar[0].sprite = shapes[LoadSave.LoadSave.LoadFace()];
        imgAvatar[1].sprite = mouths[LoadSave.LoadSave.LoadMouth()];
        imgAvatar[2].sprite = noses[LoadSave.LoadSave.LoadNose()];
        imgAvatar[3].sprite = eyes[LoadSave.LoadSave.LoadEyes()];

        if(LoadSave.LoadSave.LoadHair() < 4)
        {
            imgAvatar[4].sprite = hairs01[LoadSave.LoadSave.LoadHair()];
        }
        else
        {
            imgAvatar[4].sprite = hairs02[LoadSave.LoadSave.LoadHair() - 4];
        }


        imgAvatar[0].color = skinsColor[LoadSave.LoadSave.LoadSkin()];
    }
}
