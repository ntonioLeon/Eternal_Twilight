using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string name;
    public int price;
    public string itemId;

    public ShopItem(string name, int price, string itemId)
    {
        this.name = name;
        this.price = price;
        this.itemId = itemId;
    }
}
