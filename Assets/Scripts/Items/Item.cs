using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public float Price = 10;
    public bool OneTimePurchase = false;
    public string Name;
    public string Description;
    public Sprite ProductImage;
    public string PurchaseSound;

    public virtual void ActivatePurchase()
    {
        if (PurchaseSound != null && PurchaseSound != "")
        {
            AudioManager.instance.Play(PurchaseSound);
        }
    }

    public virtual void UpdateFurniture()
    {

    }
}
