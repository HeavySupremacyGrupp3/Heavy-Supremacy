using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int Price = 10;
    public string Name;
    public bool OneTimePurchase = false;

    public virtual void ActivatePurchase()
    {

    }
}
