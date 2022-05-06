using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItemShop : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [Header("Id of the button")]
    [SerializeField] private int id;
    [SerializeField] private ItemShop.Type type;

    private ShopManager shopManager;

    // =====================================================

    private void Awake()
    {
        shopManager = GameObject.Find("SceneManager").GetComponent<ShopManager>();
    }

    public void setId(int _id)
    {
        id = _id;
    }

    public int getId()
    {
        return id;
    }

    public void setType(ItemShop.Type _type)
    {
        type = _type;
    }

    public ItemShop.Type getType()
    {
        return type;
    }

    public void sendIdItemToManager()
    {
        shopManager.showItem(getId(), (int)getType());
    }
}
