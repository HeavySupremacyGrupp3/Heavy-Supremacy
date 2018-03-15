﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GigBackgroundManager : MonoBehaviour
{
    //Added in acending order, based on amount of fame.
    public Sprite[] Backgrounds;
    public GameObject[] Audiences;
    [Range(0,1)]
    public float[] AudienceHealthTresholds;
    public float[] FameRequirementLevels;
    public Image Background;
    public GameObject PracticeBackground;

    public static int BackgroundIndex = 0;
    public static bool GigSession = false;

    private TimingString timingString;

    private void Awake()
    {
        timingString = FindObjectOfType<TimingString>();

        if (GigSession)
        {
            Background.sprite = Backgrounds[GetBackgroundIndex()];
            PracticeBackground.SetActive(false);
        }
        else
        {
            timingString.MaxHealth = 0;
        }
    }

    private int GetBackgroundIndex()
    {
        int index = 0;

        for (int i = 0; i < FameRequirementLevels.Length; i++)
        {
            if (i != FameRequirementLevels.Length - 1 && FindObjectOfType<fameStatScript>().getAmount() >= FameRequirementLevels[i + 1])
                index++;
        }

        return index;
    }

    private void Update()
    {
        if (GigSession)
        {
            float healthPrct = timingString.health / timingString.MaxHealth;

            for (int i = 0; i < Audiences.Length; i++)
            {
                if (healthPrct >= AudienceHealthTresholds[i])
                    Audiences[i].SetActive(true);
                else
                    Audiences[i].SetActive(false);
            }
        }
    }
}
