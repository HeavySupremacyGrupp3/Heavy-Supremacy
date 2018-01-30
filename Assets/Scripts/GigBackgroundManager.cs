using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GigBackgroundManager : MonoBehaviour
{
    //Added in acending order, based on amount of fame.
    public Sprite[] Backgrounds;
    public float[] FameRequirementLevels;
    public Image Background;
    public static int BackgroundIndex = 0;
    public static bool GigSession = false;

    private void Awake()
    {
        if (GigSession)
            Background.sprite = Backgrounds[GetBackgroundIndex()];
        else
        {
            FindObjectOfType<TimingString>().MaxHealth = 0;
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
}
