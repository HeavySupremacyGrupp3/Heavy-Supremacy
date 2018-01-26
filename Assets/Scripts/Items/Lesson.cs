using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson : Item {

    public float MetalIncrease;

    public override void ActivatePurchase()
    {
        base.ActivatePurchase();
        Debug.Log("PURCHASED LESSON!");
        FindObjectOfType<metalStatScript>().addOrRemoveAmount(MetalIncrease);
    }
}
