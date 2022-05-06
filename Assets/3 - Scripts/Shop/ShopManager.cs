using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Save;
using LoadSave;

public class ShopManager : MonoBehaviour
{
    private const float FACTOR_SCALE = 1.4f;
    private const float DELAY_TO_USE = 0.6f;
    private const int BASE_HEIGHT = 10;
    private const int SPACE_ITEM = 50;
    private const int NB_CHARACTERS_TO_DECREASE = 14;
    private const float DELAY_BLINK = 0.12f;

    private Color32 colorRare = new Color(0.5f, 0.75f, 1f, 1f);
    private Color32 colorEpic = new Color(0.75f, 0.5f, 1f, 1f);
    private Color32 colorLegendary = new Color(1f, 1f, 0.5f, 1f);

    // ===================== VARIABLES =====================

    [Header("Name of Scenes")]
    [SerializeField] private string nameNavigationScene;

    [Header("Drag the Transition Manager")]
    [SerializeField] private TransitionManager transitionManager;

    [Header("Drag Buttons")]
    [SerializeField] private RectTransform[] buttons;
    // 0 = Casque / 1 = Armature / 2 = Sonnette / 3 = Pneus / 4 = Acheter / 5 = Retour / 6 = Equiper

    [Header("Panels")]
    [SerializeField] private TextMeshProUGUI currentCoins;
    [SerializeField] private TextMeshProUGUI descriptionArticle;
    [SerializeField] private Image imageArticle;
    [SerializeField] private Image themaArticle;
    [SerializeField] private TextMeshProUGUI priceArticle;

    [Header("ShopItems - Helmets")]
    public ItemShop[] helmets;

    [Header("ShopItems - Bikes")]
    public ItemShop[] bikes;

    [Header("ShopItems - Klaxons")]
    public ItemShop[] klaxons;

    [Header("ShopItems - Wheels")]
    public ItemShop[] wheels;

    [Header("Shop Item Prefab")]
    public GameObject shopItemPref;

    [Header("Components of the Scene")]
    public RectTransform content;
    public GameObject equiped;

    private bool canUseButton = true;
    private int currentIdItem = 0;
    private int currentCatItem = 0;
    private int currentCoinsPlayer = 0;
    private GameObject[] currentValidateSign = new GameObject[99];
    private GameObject[] currentEquipSign = new GameObject[99];

    private SoundManager soundManager;

    // =====================================================


    private void Start()
    {
        //Save.Save.SaveCurrentCoins(1000); // To test Shop
        currentCoinsPlayer = LoadSave.LoadSave.LoadCurrentCoins();
        currentCoins.text = currentCoinsPlayer.ToString();

        if (GameObject.Find("SoundManager") != null) { soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
    }

    public void goToNavigation()
    {
        if(!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(5); }
        StartCoroutine(IAnimButton(buttons[5]));
        StartCoroutine(IGoToScene(2.5f, nameNavigationScene));
    }


    private IEnumerator IAnimButton(RectTransform _btn)
    {
        canUseButton = false;

        float minScaleX = _btn.sizeDelta.x;
        float minScaleY = _btn.sizeDelta.y;
        float maxScaleX = minScaleX * FACTOR_SCALE;
        float maxScaleY = minScaleY * FACTOR_SCALE;
        float addScaleX = (maxScaleX - minScaleX) / 18;
        float addScaleY = (maxScaleY - minScaleY) / 18;

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

    private IEnumerator IResetCanUse(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        canUseButton = true;
    }

    public void createContent(int _id)
    {
        if (!canUseButton) { return; }

        if (soundManager != null) { soundManager.playAudioClip(1); }
        canUseButton = false;
        StartCoroutine(IAnimButton(buttons[_id]));

        ItemShop[] itemShopCategory = selectCategory(_id);

        clearContent(); // Clear all the items in the content
        calcultateContentHeight(itemShopCategory.Length);

        for(int i = 0; i < itemShopCategory.Length; i++)
        {
            GameObject shopItem;
            shopItem = Instantiate(shopItemPref, this.transform.position, Quaternion.identity, content.transform);
            shopItem.name = "[ItemShop]" + "_" + i;

            placeItemInContent(shopItem.transform, i);

            customizeShopItem(shopItem.transform, i, itemShopCategory);

            setIdButton(shopItem, i, itemShopCategory[i].type);
        }

        StartCoroutine(IResetCanUse(DELAY_TO_USE));
    }

    private ItemShop[] selectCategory(int _id)
    {
        ItemShop[] catTemp = helmets;

        switch (_id)
        {
            case 0:
                catTemp = helmets;
                break;
            case 1:
                catTemp = bikes;
                break;
            case 2:
                catTemp = klaxons;
                break;
            case 3:
                catTemp = wheels;
                break;
            default:
                print("category unfoundable : ShopManager.cs");
                break;
        }

        return catTemp;
    }

    private void calcultateContentHeight(int _length)
    {
        int height = BASE_HEIGHT + (_length * SPACE_ITEM);

        content.sizeDelta = new Vector2(0, height);
    }

    private void placeItemInContent(Transform _shopItem, int _i)
    {
       int height = (40 + (_i * SPACE_ITEM)) * -1;

        _shopItem.localPosition = new Vector2(150f, height);
    }

    private void customizeShopItem(Transform _shopItem, int _i, ItemShop[] _cat)
    {
        // Change the color of the background
        if(_cat[_i].rarity == ItemShop.Rarity.Rare)
        {
            _shopItem.GetChild(0).GetComponent<Image>().color = colorRare;
        }
        else if (_cat[_i].rarity == ItemShop.Rarity.Epique)
        {
            _shopItem.GetChild(0).GetComponent<Image>().color = colorEpic;
        }
        else if (_cat[_i].rarity == ItemShop.Rarity.Legendaire)
        {
            _shopItem.GetChild(0).GetComponent<Image>().color = colorLegendary;
        }

        // Change the name of the item
        string nameItem = _cat[_i].nameItem;
        _shopItem.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = nameItem;

        // Set the size of the font if the text is too long
        if(nameItem.Length > NB_CHARACTERS_TO_DECREASE)
        {
            _shopItem.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 20;
        }

        // Set active the validate sign of the shop item
        currentValidateSign[_i] = _shopItem.GetChild(1).gameObject;
        currentEquipSign[_i] = _shopItem.GetChild(2).gameObject;

        if ((_cat[_i].type == ItemShop.Type.Casque && LoadSave.LoadSave.LoadBoughtHelmet(_i) == 1) ||
            (_cat[_i].type == ItemShop.Type.Armature && LoadSave.LoadSave.LoadBoughtBike(_i) == 1) ||
            (_cat[_i].type == ItemShop.Type.Sonnette && LoadSave.LoadSave.LoadBoughtKlaxon(_i) == 1) ||
            (_cat[_i].type == ItemShop.Type.Pneus && LoadSave.LoadSave.LoadBoughtWheels(_i) == 1))
        {
            _shopItem.GetChild(1).gameObject.SetActive(true);
        }

        if ((_cat[_i].type == ItemShop.Type.Casque && LoadSave.LoadSave.LoadEquipedHelmet() == _i) ||
            (_cat[_i].type == ItemShop.Type.Armature && LoadSave.LoadSave.LoadEquipedBike() == _i) ||
            (_cat[_i].type == ItemShop.Type.Sonnette && LoadSave.LoadSave.LoadEquipedKlaxon() == _i) ||
            (_cat[_i].type == ItemShop.Type.Pneus && LoadSave.LoadSave.LoadEquipedWheels() == _i))
        {
            _shopItem.GetChild(2).gameObject.SetActive(true);
        }
    }

    private void clearContent()
    {
        for(int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }

    private void setIdButton(GameObject _go, int _id, ItemShop.Type _type)
    {
        ButtonItemShop buttonItemShop = _go.GetComponent<ButtonItemShop>();

        buttonItemShop.setId(_id);
        buttonItemShop.setType(_type);
    }

    public void showItem(int _idItem, int _idCategory)
    {
        if (soundManager != null) { soundManager.playAudioClip(1); }
        currentIdItem = _idItem;
        currentCatItem = _idCategory;

        ItemShop[] itemShopCategory = selectCategory(_idCategory);

        descriptionArticle.text = itemShopCategory[_idItem].description;
        imageArticle.sprite = itemShopCategory[_idItem].imgItem;
        themaArticle.sprite = itemShopCategory[_idItem].imgThema;
        priceArticle.text = itemShopCategory[_idItem].price.ToString();

        showRights(_idItem, _idCategory);
    }

    private void showRights(int _idItem, int _idCategory)
    {
        if ((_idCategory == (int)ItemShop.Type.Casque && LoadSave.LoadSave.LoadBoughtHelmet(_idItem) == 1) ||
            (_idCategory == (int)ItemShop.Type.Armature && LoadSave.LoadSave.LoadBoughtBike(_idItem) == 1) ||
            (_idCategory == (int)ItemShop.Type.Sonnette && LoadSave.LoadSave.LoadBoughtKlaxon(_idItem) == 1) ||
            (_idCategory == (int)ItemShop.Type.Pneus && LoadSave.LoadSave.LoadBoughtWheels(_idItem) == 1))
        {
            buttons[4].gameObject.SetActive(false);
            buttons[6].gameObject.SetActive(true);
            equiped.SetActive(false);

            if ((_idCategory == (int)ItemShop.Type.Casque && LoadSave.LoadSave.LoadEquipedHelmet() == _idItem) ||
            (_idCategory == (int)ItemShop.Type.Armature && LoadSave.LoadSave.LoadEquipedBike() == _idItem) ||
            (_idCategory == (int)ItemShop.Type.Sonnette && LoadSave.LoadSave.LoadEquipedKlaxon() == _idItem) ||
            (_idCategory == (int)ItemShop.Type.Pneus && LoadSave.LoadSave.LoadEquipedWheels() == _idItem))
            {
                buttons[6].gameObject.SetActive(false);
                equiped.SetActive(true);
            }
            else
            {
                buttons[6].gameObject.SetActive(true);
                equiped.SetActive(false);
            }
        }
        else
        {
            buttons[4].gameObject.SetActive(true);
            buttons[6].gameObject.SetActive(false);
            equiped.SetActive(false);
        }
    }

    public void buyItem()
    {
        if(!checkCoins()) { if (soundManager != null) { soundManager.playAudioClip(0); } StartCoroutine(IBlinkTextCoinCancel()); return; }

        if (soundManager != null) { soundManager.playAudioClip(3); }

        if (currentCatItem == 0) { Save.Save.SaveBoughtHelmet(currentIdItem); }
        else if (currentCatItem == 1) { Save.Save.SaveBoughtBike(currentIdItem); }
        else if (currentCatItem == 2) { Save.Save.SaveBoughtKlaxon(currentIdItem); }
        else if (currentCatItem == 3) { Save.Save.SaveBoughtWheels(currentIdItem); }

        buttons[4].gameObject.SetActive(false);
        buttons[6].gameObject.SetActive(true);

        updateCoins(getPriceItem());

        currentValidateSign[currentIdItem].SetActive(true);
        StartCoroutine(IAnimSign(currentValidateSign[currentIdItem].transform));
    }

    private bool checkCoins()
    {
        return (currentCoinsPlayer >= getPriceItem());
    }

    private int getPriceItem()
    {
        int tempPrice = 0;

        if(currentCatItem == 0)
        {
            tempPrice = helmets[currentIdItem].price;
        }
        else if (currentCatItem == 1)
        {
            tempPrice = bikes[currentIdItem].price;
        }
        else if (currentCatItem == 2)
        {
            tempPrice = klaxons[currentIdItem].price;
        }
        else if (currentCatItem == 3)
        {
            tempPrice = wheels[currentIdItem].price;
        }

        return tempPrice;
    }

    private void updateCoins(int _price)
    {
        currentCoinsPlayer -= _price;

        StartCoroutine(IGrowTextCoin());
        StartCoroutine(IDecreaseTextCoin());
        StartCoroutine(IBlinkTextCoin());

        Save.Save.SaveCurrentCoins(currentCoinsPlayer);
    }

    private IEnumerator IGrowTextCoin()
    {
        float sizeStart = currentCoins.fontSize;

        while(currentCoins.fontSize < sizeStart * FACTOR_SCALE)
        {
            yield return new WaitForSeconds(0.01f);

            currentCoins.fontSize += 2f;
        }

        while (currentCoins.fontSize > sizeStart)
        {
            yield return new WaitForSeconds(0.01f);

            currentCoins.fontSize -= 1.2f;
        }

        currentCoins.fontSize = sizeStart;
    }

    private IEnumerator IDecreaseTextCoin()
    {
        while (int.Parse(currentCoins.text) > currentCoinsPlayer)
        {
            yield return new WaitForSeconds(0.01f);

            int coins = int.Parse(currentCoins.text);
            coins -= 2;
            currentCoins.text = coins.ToString();
        }

        currentCoins.text = currentCoinsPlayer.ToString();
    }

    private IEnumerator IBlinkTextCoin()
    {
        Color32 white = new Color32(255, 255, 255, 255);
        Color32 black = new Color32(0, 0, 0, 255);

        currentCoins.color = black;

        yield return new WaitForSeconds(DELAY_BLINK);

        currentCoins.color = white;

        yield return new WaitForSeconds(DELAY_BLINK);

        currentCoins.color = black;

        yield return new WaitForSeconds(DELAY_BLINK);

        currentCoins.color = white;
    }

    private IEnumerator IBlinkTextCoinCancel()
    {
        Color32 white = new Color32(255, 255, 255, 255);
        Color32 red = new Color32(255, 0, 0, 255);

        currentCoins.color = red;

        yield return new WaitForSeconds(DELAY_BLINK);

        currentCoins.color = white;

        yield return new WaitForSeconds(DELAY_BLINK);

        currentCoins.color = red;

        yield return new WaitForSeconds(DELAY_BLINK);

        currentCoins.color = white;
    }

    private IEnumerator IAnimSign(Transform _sign)
    {
        float startScale = _sign.localScale.x;

        while (_sign.localScale.x < startScale * FACTOR_SCALE)
        {
            yield return new WaitForSeconds(0.01f);

            _sign.localScale = new Vector2(_sign.localScale.x + 0.2f, _sign.localScale.y + 0.2f);
        }

        while (_sign.localScale.x > startScale)
        {
            yield return new WaitForSeconds(0.01f);

            _sign.localScale = new Vector2(_sign.localScale.x - 0.2f, _sign.localScale.y - 0.2f);
        }

        _sign.localScale = new Vector2(startScale, startScale);
    }

    public void equipItem()
    {
        if (soundManager != null) { soundManager.playAudioClip(4); }

        if (currentCatItem == 0) { Save.Save.SaveEquipedHelmet(currentIdItem); }
        else if (currentCatItem == 1) { Save.Save.SaveEquipedBike(currentIdItem); }
        else if (currentCatItem == 2) { Save.Save.SaveEquipedKlaxon(currentIdItem); }
        else if (currentCatItem == 3) { Save.Save.SaveEquipedWheels(currentIdItem); }

        buttons[6].gameObject.SetActive(false);
        equiped.SetActive(true);

        int length = 0;
        if (currentCatItem == 0) { length = helmets.Length; }
        else if (currentCatItem == 1) { length = bikes.Length; }
        else if (currentCatItem == 2) { length = klaxons.Length; }
        else if (currentCatItem == 3) { length = wheels.Length; }

        for(int i = 0; i < length; i++)
        {
            currentEquipSign[i].SetActive(false);
        }

        currentEquipSign[currentIdItem].SetActive(true);
        StartCoroutine(IAnimSign(currentEquipSign[currentIdItem].transform));
    }
}
