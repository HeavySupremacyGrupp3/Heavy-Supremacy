using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public FadeOutManager fadeScript;
    public float Rent;
    public static int day = 1;
    public static int week = 1;
    public static bool IsFirstWorkRun = true;
    public static bool IsFirstHubRun = true;
    public GameObject IntroPanel;
    public GameObject EndGamePanel;
    public Text EndGameTitle;
    public SceneTransitionScript SceneTransition;
    public Animator EnergyAnimator;
    public Image[] StatPreviewCosts;

    public delegate void mittEvent();
    public static event mittEvent sleep;

    public static string EndGameTitleText;
    public static bool ToEndGame;
    public static bool RestartGame;
    public static bool ShowIntroPanel;

	private KeyCode key=KeyCode.Escape;

    void Start()
    {
        if (ToEndGame)
            EndGame(EndGameTitleText);
        if (RestartGame)
            Restart();

        if (ShowIntroPanel)
        {
            IntroPanel.SetActive(true);
        }

        Initialize();
        fadeScript = GetComponent<FadeOutManager>();

        AudioManager am = AudioManager.instance;
        am.Play("HUBMusic");
        Sound s = Array.Find(am.sounds, Sound => Sound.name == "HUBMusic");
        s.source.time = UnityEngine.Random.Range(0, s.clip.length);

        am.Play("HUBAmbience");

        if (IsFirstHubRun)
        {
            IsFirstHubRun = !IsFirstHubRun;
            GameObject.Find("HubTutorial").transform.GetChild(0).GetComponent<Tutorial>().Run(); // BECAUSE UNITY IS AWESOME, AND GAMEOBJECT.FIND(HUB_TUTORIAL).GETCOMPONENT<TUTORIAL>() DOESN'T WORK! YAAY! Not even mad.
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            FindObjectOfType<fameStatScript>().addOrRemoveAmount(10);
        if (Input.GetKeyDown(KeyCode.A))
            FindObjectOfType<angstStatScript>().addOrRemoveAmount(-10.2f);
        //WeekText.text = "Approximately week: " + week;
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        if (Input.GetKeyDown(KeyCode.G))
            LoadGig(10);
		//if(Input.GetKeyDown(key))
		//	Quit();
		
    }

    void Initialize()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
            Destroy(FindObjectsOfType<GameManager>()[0].gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
            Destroy(gameObject);

        if (level == 1)
        {
            Time.timeScale = 1;
            PauseMenu.paused = false;

            ShopSystem.UpdateHUBEnvironment();
        }
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
        RestartGame = false;

        ShopSystem.MyInventory.Clear();

        FindObjectOfType<metalStatScript>().ResetAmount();
        FindObjectOfType<moneyStatScript>().ResetAmount();
        FindObjectOfType<angstStatScript>().ResetAmount();
        FindObjectOfType<fameStatScript>().ResetAmount();
        FindObjectOfType<energyStatScript>().ResetAmount();

        NoteGenerator.Reset();
        TimingString.Reset();
        ToEndGame = false;

        day = 1;
        week = 1;

        LoadHUB();
    }

    public void LoadWork(float energyCost)
    {
        if (CheckEnergy(energyCost))
        {
            AudioManager.instance.Play("DoorClick");
            StopHUBLoops();
            SceneTransition.StartTransition("WorkScene");
        }
    }

    public void LoadPractice(float energyCost)
    {
        if (CheckEnergy(energyCost))
        {
            AudioManager.instance.Play("practiceClick");
            StopHUBLoops();
            GigBackgroundManager.GigSession = false;
            SceneTransition.StartTransition("PracticeScene");
        }
    }

    public void LoadGig(float energyCost)
    {
        if (CheckEnergy(energyCost))
        {
            StopHUBLoops();
            GigBackgroundManager.GigSession = true;
            SceneTransition.StartTransition("PracticeScene");
        }
    }

    public void LoadStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    public void LoadHUB()
    {
        StopHUBLoops();
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
        FindObjectOfType<GameEventManager>().CheckForStatEvents();
    }

    public void IncreaseWeek()
    {
        if (day % 5 == 0)
        {
            FindObjectOfType<GameEventManager>().TriggerRentReminder();
        }
        if (day % 6 == 0)
        {
            FindObjectOfType<GameEventManager>().TriggerUpcomingGig();
        }
        if (day % 7 == 0)
        {
            FindObjectOfType<GameEventManager>().TriggerGig();
        }

        if (day % 8 == 0)
        {
            week++;
            day = 1;

            if ((FindObjectOfType<moneyStatScript>().getAmount() - Rent) < 0)
                EndGame("You're Broke!");
            else
                FindObjectOfType<moneyStatScript>().addOrRemoveAmount(-Rent);
        }
    }

    void StopHUBLoops()
    {
        AudioManager.instance.Stop("HUBAmbience");
        AudioManager.instance.Stop("HUBMusic");
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
        {
            EnergyAnimator.SetTrigger("LerpEnergy");
            AudioManager.instance.Play("LowEnergy");
            return false;
        }
    }

    public void SetIntroPanelBool(bool active)
    {
        ShowIntroPanel = active;
    }

    public void UpdateStatPreviewFill(StatPreviewData data)
    {
        int statIndex = data.StatIndex;
        float value = data.Value;

        float tempValue = 1 - StatPreviewCosts[statIndex].fillAmount;

        StatPreviewCosts[statIndex].material.SetFloat("_EdgeWidth", tempValue + (value / 100));
    }

    public void ResetAllStatPreviews()
    {
        for (int i = 0; i < StatPreviewCosts.Length; i++)
        {
            StatPreviewCosts[i].material.SetFloat("_EdgeWidth", 0);
        }
    }
}
