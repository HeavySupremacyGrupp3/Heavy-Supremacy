using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : Item {

    private void Start()
    {
        //base.Start();
    }

    public override void ActivatePurchase()
    {
        base.ActivatePurchase();
        Debug.Log("PURCHASED GUITAR!");
        NoteGenerator.NoteMultiplier++;
    }
}
