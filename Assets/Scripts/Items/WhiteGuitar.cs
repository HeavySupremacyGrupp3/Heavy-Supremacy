using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteGuitar : Item {

    public float DoubleNoteChance = 0.1f;

    private void Start()
    {
        //base.Start();
    }

    public override void ActivatePurchase()
    {
        base.ActivatePurchase();
        Debug.Log("PURCHASED WHITE GUITAR!");
        NoteGenerator.DoubleNoteChance = DoubleNoteChance;
    }
}
