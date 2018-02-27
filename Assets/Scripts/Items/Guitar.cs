using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : Item {

    public float MetalMultiplierIncrease = 0.1f;

    private void Start()
    {
        //base.Start();
    }

    public override void ActivatePurchase()
    {
        base.ActivatePurchase();
        Debug.Log("PURCHASED GUITAR!");
        NoteGenerator.NumberOfUniqueNotes++;
        TimingString.MetalMultiplier += MetalMultiplierIncrease;
    }
}
