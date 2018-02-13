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

        Text.rectTransform.position = new Vector3(mousePos.x, mousePos.y, -0.01f);
        //Text.rectTransform.localPosition = new Vector3(Text.rectTransform.position.x, Text.rectTransform.position.y, Text.rectTransform.position.z);
    }

    public void SetStatString(string stat)
    {
        UpdatePosition();

        if (stat == "metal")
        {
            Text.text = metal.getAmount().ToString();
            Text.color = MetalColor;
        }
        else if (stat == "fame")
        {
            Text.text = fame.getAmount().ToString();
            Text.color = FameColor;
        }
        else if (stat == "angst")
        {
            Text.text = angst.getAmount().ToString();
            Text.color = AngstColor;
        }
        else if (stat == "energy")
        {
            Text.text = energy.getAmount().ToString();
            Text.color = EnergyColor;
        }

        Text.gameObject.SetActive(true);
    }
}