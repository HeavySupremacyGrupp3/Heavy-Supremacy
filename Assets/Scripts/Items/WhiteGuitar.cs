﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WhiteGuitar : Item
{

    public float DoubleNoteChance = 0.1f;
    public float MetalMultiplierIncrease = 0.1f;
    public string HUBGuitar;
    public float ScaleMultiplier = 1;

    private Image hubImage;
    private bool instantiated;

    private void Start()
    {
        //base.Start();
    }

    public override void ActivatePurchase()
    {
        base.ActivatePurchase();
        NoteGenerator.DoubleNoteChance = DoubleNoteChance;
        TimingString.MetalMultiplier += MetalMultiplierIncrease;
    }

    public override void UpdateFurniture()
    {
        hubImage = GameObject.Find(HUBGuitar).GetComponent<Image>();
        hubImage.sprite = ProductImage;
        hubImage.GetComponent<RectTransform>().sizeDelta = new Vector2(ProductImage.rect.width, ProductImage.rect.height) * ScaleMultiplier;
    }
}
