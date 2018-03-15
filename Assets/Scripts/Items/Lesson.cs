using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson : Item {

    public float MetalMultiplierIncrease;
    public float PriceIncreaseMultiplier = 2;
    public float StartPrice;

    public override void ActivatePurchase()
    { 
        base.ActivatePurchase();
        TimingString.MetalMultiplier += MetalMultiplierIncrease;

        Price = Mathf.Round(Price * PriceIncreaseMultiplier);
    }

    public void ResetPrice()
    {
        Price = StartPrice;
    }
}
