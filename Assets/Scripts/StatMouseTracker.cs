﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatMouseTracker : MonoBehaviour
{

    public Text Text;
    public Color MetalColor = Color.white;
    public Color FameColor = Color.white;
    public Color AngstColor = Color.white;
    public Color EnergyColor = Color.white;


    private metalStatScript metal;
    private fameStatScript fame;
    private angstStatScript angst;
    private energyStatScript energy;

    void Start()
    {
        metal = FindObjectOfType<metalStatScript>();
        fame = FindObjectOfType<fameStatScript>();
        angst = FindObjectOfType<angstStatScript>();
        energy = FindObjectOfType<energyStatScript>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x + 40, Input.mousePosition.y + 40));

        Text.rectTransform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    public void SetStatString(string stat)
    {
        UpdatePosition();
        float amount = 0;

        if (stat == "metal")
        {
            amount = metal.getAmount();
            Text.color = MetalColor;
        }
        else if (stat == "fame")
        {
            amount = fame.getAmount();
            Text.color = FameColor;
        }
        else if (stat == "angst")
        {
            amount = angst.getAmount();
            Text.color = AngstColor;
        }
        else if (stat == "energy")
        {
            amount = energy.getAmount();
            Text.color = EnergyColor;
        }

        amount = Mathf.RoundToInt(amount);
        Text.text = amount.ToString();

        Text.gameObject.SetActive(true);
    }
}