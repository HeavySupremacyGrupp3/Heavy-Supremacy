﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Object HUBScene;
    public Object WorkScene;
    public Object PracticeScene;
    public Object GigScene;
    public FadeOutManager fadeScript;

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

    public void LoadWork()
    {
        SceneManager.LoadScene(WorkScene.name);
    }

    public void LoadPractice()
    {
        SceneManager.LoadScene(PracticeScene.name);
    }

    public void LoadGig()
    {
        SceneManager.LoadScene(GigScene.name);
    }

    public void LoadHUB()
    {
        SceneManager.LoadScene(HUBScene.name);
    }

    public void LoadSleep()
    {
        fadeScript = FindObjectOfType<FadeOutManager>();
        fadeScript.FadeOut();
    }

    public void ToggleGameObject(GameObject target)
    {
        target.SetActive(!target.active);
    }

}
