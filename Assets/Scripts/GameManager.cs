﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public FadeOutManager fadeScript;
    public float Rent;
    public static int day = 1;
    public static int week = 1;
    public TextMesh WeekText;
    public GameObject EndGamePanel;
    public Text EndGameTitle;

    public delegate void mittEvent();
    public static event mittEvent sleep;

    public static string EndGameTitleText;
    public static bool ToEndGame;

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        if (ToEndGame)
            EndGame(EndGameTitleText);

        Initialize();
        fadeScript = GetComponent<FadeOutManager>();
        //WeekText = GetComponent<TextMesh>();
        WeekText.text = "Approximately week: ";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        FindObjectOfType<fameStatScript>().addOrRemoveAmount(10);
        //WeekText.text = "Approximately week: " + week;
    }

    void Initialize()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
            Destroy(FindObjectsOfType<GameManager>()[0].gameObject);
        
            DontDestroyOnLoad(gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void EndGame(string title)
    {
        ToEndGame = false;
        EndGamePanel.SetActive(true);
        EndGameTitle.text = title;
    }

    public void Restart()
    {
        ShopSystem.MyInventory.Clear();
        FindObjectOfType<metalStatScript>().ResetAmount();
        FindObjectOfType<moneyStatScript>().ResetAmount();
        FindObjectOfType<angstStatScript>().ResetAmount();
        FindObjectOfType<fameStatScript>().ResetAmount();
        FindObjectOfType<energyStatScript>().ResetAmount();

        NoteGenerator.Reset();
        TimingString.Reset();

        day = 1;
        week = 1;

        LoadHUB();
    }

    public void LoadWork(float energyCost)
    {
        if (CheckEnergy(energyCost))
            SceneManager.LoadScene("WorkScene");
    }

    public void LoadPractice(float energyCost)
    {
        if (CheckEnergy(energyCost))
        {
            GigBackgroundManager.GigSession = false;
            SceneManager.LoadScene("PracticeScene");
        }
    }

    public void LoadGig(float energyCost)
    {
        if (CheckEnergy(energyCost))
        {
            GigBackgroundManager.GigSession = true;
            SceneManager.LoadScene("PracticeScene");
        }
    }
    public void LoadHUB()
    {
        GigBackgroundManager.GigSession = false;
        SceneManager.LoadScene("HUBScene");
    }

    public void LoadSleep()
    {
        fadeScript = FindObjectOfType<FadeOutManager>();
        fadeScript.FadeOut();
        IncreaseDay();
        sleep();
    }

    public void IncreaseDay()
    {
        day++;
        IncreaseWeek();
    }

    public void IncreaseWeek()
    {
        if (day % 7 == 0)
        {
            week++;

            if ((FindObjectOfType<moneyStatScript>().getAmount() - Rent) < 0)
                EndGame("You're Broke!");
            else
                FindObjectOfType<moneyStatScript>().addOrRemoveAmount(-Rent);
        }
    }

    public void ToggleGameObject(GameObject target)
    {
        target.SetActive(!target.active);
    }

    bool CheckEnergy(float energyCost)
    {
        if (FindObjectOfType<energyStatScript>().getAmount() - energyCost >= 0)
        {
            FindObjectOfType<energyStatScript>().addOrRemoveAmount(-energyCost);
            return true;
        }
        else
            return false;
    }
}
