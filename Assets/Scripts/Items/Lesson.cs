using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson : Item {

    public float PracticeAngstMultiplierIncrease;
    public float PracticeMetalMultiplierIncrease;

    private void Start()
    {
        //base.Start();
    }

    public override void ActivatePurchase()
    {
        base.ActivatePurchase();
        Debug.Log("PURCHASED LESSON!");
        TimingString.AngstMultiplier += PracticeAngstMultiplierIncrease;
        TimingString.MetalMultiplier += PracticeMetalMultiplierIncrease;
    }
}
