using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public FadeOutManager fadeScript;

    public delegate void mittEvent();
    public static event mittEvent sleep;

    void Start()
    {
        Initialize();
        fadeScript = GetComponent<FadeOutManager>();
    }

    void Initialize()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
            Destroy(FindObjectsOfType<GameManager>()[0]);
        else
            DontDestroyOnLoad(gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadWork(float energyCost)
    {
        if (CheckEnergy(energyCost))
            SceneManager.LoadScene("WorkScene");
    }

    public void LoadPractice(float energyCost)
    {
        if (CheckEnergy(energyCost))
            SceneManager.LoadScene("PracticeScene");
    }

    public void LoadGig(float energyCost)
    {
        if (CheckEnergy(energyCost))
            SceneManager.LoadScene("GigScene");
    }

    public void LoadHUB()
    {
        SceneManager.LoadScene("HUBScene");
    }

    public void LoadSleep()
    {
        fadeScript = FindObjectOfType<FadeOutManager>();
        fadeScript.FadeOut();
        sleep();
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
