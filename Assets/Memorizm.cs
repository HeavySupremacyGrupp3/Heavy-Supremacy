using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memorizm : MonoBehaviour
{
    [Header("Cards")]
    public Image[] CardContents;
    public Sprite[] Images;
    public Sprite DefaultSprite;
    private List<Sprite> ShuffledCardSprites = new List<Sprite>();
    private Image PreviouslyCheckedCard = null;
    public float CardFlipHalfTime = 0.26f;

    [Header("Time Limit")]
    public float TimeLimit = 40f;
    private float InitialTimeLimit;
    public float TimeDecreasePerLevel = 2f;
    private float InitialTimeDecreasePerLevel;
    public float TimeDecreasePerLevelPerLevelDecrease = 1.2f;
    public float MinimumTimeLimit = 7f;
    public Slider TimeSlider;
    private float LevelStartTime;
    private bool StartTimer = false;

    [Header("Card Manipulation")]
    public int StartingCards = 3;
    public int LevelToIntroduceSixCards = 4;
    private float CurrentAmountOfCards;
    private int CardsTurnedCorrectly = 0;

    [Header("Pause Overlay & Etc")]
    public Animator PauseOverlay;
    private bool IsPaused = true;
    public float PauseTime = 3f;
    public Text LevelText;
    public Animator LoseOverlay;

    private int Level = 1;

    private void Awake()
    {
        InitialTimeLimit = TimeLimit;
        InitialTimeDecreasePerLevel = TimeDecreasePerLevel;
    }

    private void OnEnable()
    {
        NewGame();
        AudioManager.instance.Stop("HUBMusic");
        AudioManager.instance.Play("Memorizm_Music");
    }

    private void OnDisable()
    {
        AudioManager.instance.Stop("Memorizm_Music");
        AudioManager.instance.Play("HUBMusic");
    }

    private void NewGame()
    {
        TimeLimit = InitialTimeLimit;
        TimeDecreasePerLevel = InitialTimeDecreasePerLevel;
        CurrentAmountOfCards = StartingCards;
        CardsTurnedCorrectly = 0;
        Level = 1;
        LevelText.text = "level " + Level;

        foreach (Image Card in CardContents)
        {
            StartCoroutine(Flip(Card, DefaultSprite));
        }

        ShuffleCards();
        StartCoroutine(ShowPause());
    }

    private void NextLevel()
    {
        TimeDecreasePerLevel *= TimeDecreasePerLevelPerLevelDecrease;
        TimeLimit = TimeLimit - TimeDecreasePerLevel < MinimumTimeLimit ? MinimumTimeLimit : TimeLimit - TimeDecreasePerLevel;

        CardsTurnedCorrectly = 0;

        Level++;
        LevelText.text = "level " + Level;

        if (Level >= LevelToIntroduceSixCards)
        {
            CurrentAmountOfCards = 6;
        }

        foreach (Image Card in CardContents)
        {
            StartCoroutine(Flip(Card, DefaultSprite));
        }

        StartCoroutine(ShowPause());
        ShuffleCards();
    }

    private void LoseLevel()
    {
        StartCoroutine(ShowLossScreen());
    }

    private void Update()
    {
        if (!StartTimer)
            return;

        float curTime = Time.time - LevelStartTime;
        TimeSlider.value = curTime / TimeLimit;

        if (curTime >= TimeLimit)
        {
            LoseLevel();
        }
    }

    private IEnumerator ShowLossScreen()
    {
        StartTimer = false;
        TimeSlider.value = 0;
        LoseOverlay.gameObject.SetActive(true);
        LoseOverlay.SetTrigger("Animate");
        IsPaused = true;
        yield return new WaitForSeconds(PauseTime);
        IsPaused = false;
        LoseOverlay.gameObject.SetActive(false);
        NewGame();
    }

    private IEnumerator ShowPause()
    {
        StartTimer = false;
        TimeSlider.value = 0;
        PauseOverlay.gameObject.SetActive(true);
        PauseOverlay.SetTrigger("Animate");
        IsPaused = true;
        yield return new WaitForSeconds(PauseTime);
        IsPaused = false;
        PauseOverlay.gameObject.SetActive(false);
        LevelStartTime = Time.time;
        StartTimer = true;
    }

    private void ShuffleCards()
    {
        ShuffledCardSprites.Clear();

        // Get the sprites that will be used
        List<Sprite> thisRoundsSprites = new List<Sprite>();
        int curAmount = 0;
        for (int i = Random.Range(0, Images.Length); curAmount < (int)CurrentAmountOfCards; i++)
        {
            if (i == Images.Length)
                i = 0;

            thisRoundsSprites.Add(Images[i]);
            curAmount++;
        }

        // Populate the list to be shuffled
        int ofEach = CardContents.Length / thisRoundsSprites.Count;
        for (int i = 0; i < thisRoundsSprites.Count; i++)
        {
            for (int x = 0; x < ofEach; x++)
            {
                ShuffledCardSprites.Add(thisRoundsSprites[i]);
            }
        }

        // Shuffle the list (twice, for teh lulz)
        for (int n = 0; n < 2; n++)
        {
            for (int i = ShuffledCardSprites.Count - 1; i > 1; i--)
            {
                int x = Random.Range(0, i + 1);
                Sprite spr = ShuffledCardSprites[x];
                ShuffledCardSprites[x] = ShuffledCardSprites[i];
                ShuffledCardSprites[i] = spr;
            }
        }
    }

    public void TurnOverCard(int idx)
    {
        if (IsPaused)
            return;

        AudioManager.instance.Play("Memorizm_Flip");
        StartCoroutine(TurnOverCardCo(idx));
    }

    private IEnumerator TurnOverCardCo(int idx)
    {
        IsPaused = true;
        yield return Flip(CardContents[idx], ShuffledCardSprites[idx]);
        if (PreviouslyCheckedCard != null) // Check if match
        {
            if (PreviouslyCheckedCard.sprite == CardContents[idx].sprite && PreviouslyCheckedCard != CardContents[idx])
            {
                CardsTurnedCorrectly += 2;
            }
            else
            {
                StartCoroutine(Flip(CardContents[idx], DefaultSprite));
                yield return Flip(PreviouslyCheckedCard, DefaultSprite);
            }
            PreviouslyCheckedCard = null;

            if (CardsTurnedCorrectly == CardContents.Length) // All Correct. Win. Yay!
            {
                NextLevel();
            }
        }
        else
        {
            PreviouslyCheckedCard = CardContents[idx];
        }
        IsPaused = false;
    }

    private IEnumerator Flip(Image card, Sprite spr)
    {
        card.GetComponentInParent<Animator>().SetTrigger("Animate");
        yield return new WaitForSeconds(CardFlipHalfTime);
        card.sprite = spr;
    }


}
