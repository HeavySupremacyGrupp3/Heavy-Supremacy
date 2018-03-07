using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public enum ItemType { Item, Furniture };
    public ItemType Type;
    public float Price = 10;
    public bool OneTimePurchase = false;
    public string Name;
    public string Description;
    public Sprite ProductImage;

    public virtual void ActivatePurchase()
    {

    }

    public virtual void UpdateFurniture()
    {

    }
}
