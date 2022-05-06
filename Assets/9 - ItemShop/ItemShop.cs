using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemShop", menuName = "ScriptableObjects/ItemShop", order = 1)]
public class ItemShop : ScriptableObject
{
    public enum Type { Casque, Armature, Sonnette, Pneus };
    public enum Rarity { Commun, Rare, Epique, Legendaire };

    public Type type;
    public int id; // Par type
    public string nameItem;
    [TextArea] public string description;
    public Sprite imgItem;
    public Sprite imgThema;
    public int price;
    public Rarity rarity;
}
