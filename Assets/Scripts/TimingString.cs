using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingString : TimingSystem
{
    public AudioClip[] ErrorSounds;
    public AudioSource AudioSource;

    public float AngstRewardAmount = -10;
    public float MetalRewardAmount = 15;
    public int MaxHealth = 5;
    public Slider HealthSlider;
    public GameObject AngstPopupPrefab;
    public GameObject MetalPopupPrefab;
    public GameObject NoteHitEffect;

    public GameObject PerfectPopupPrefab;
    public GameObject GoodPopupPrefab;
    public GameObject BadPopupPrefab;
    public GameObject MissPopupPrefab;

    public float PerfectDistance;
    public float GoodDistance;
    public float BadDistance;

    public Animator StringAnimator;
    public Text StreakCounter;
    public Text HighestStreakCounter;

    public static float AngstMultiplier = 1;
    public static float MetalMultiplier = 1;

    private angstStatScript AngstStatScript;
    private metalStatScript MetalStatScript;

    private int streakCounter = 0;
    private int streakHighScoreCounter = 0;
    private int health;

    void Start()
    {
        health = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
        if (!GigBackgroundManager.GigSession)
            HealthSlider.gameObject.SetActive(false);
        
        AngstStatScript = FindObjectOfType<angstStatScript>();
        MetalStatScript = FindObjectOfType<metalStatScript>();
    }

    public override void FailTiming()
    {
        Destroy(Instantiate(MissPopupPrefab, new Vector2(transform.position.x + 5, transform.position.y), Quaternion.identity), 3);

        AddOrRemoveHealth(-1);
        UpdateStreakCounters(-streakCounter);

        AudioSource.PlayOneShot(ErrorSounds[Random.Range(0, ErrorSounds.Length)]);
        StringAnimator.SetTrigger("StringStroked");

        base.FailTiming();
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();

        AddOrRemoveHealth(1);
        UpdateStreakCounters(1);

        Destroy(Instantiate(GetNoteAccuracyPrefab(), new Vector2(transform.position.x + 5, transform.position.y), Quaternion.identity), 3);

        AngstStatScript.addOrRemoveAmount(AngstRewardAmount * AngstMultiplier);
        MetalStatScript.addOrRemoveAmount(MetalRewardAmount * MetalMultiplier);

        //GameObject metalPopup = Instantiate(MetalPopupPrefab, target.transform.position, Quaternion.identity) as GameObject;
        //GameObject angstPopup = Instantiate(AngstPopupPrefab, target.transform.position, Quaternion.identity) as GameObject;
        GameObject noteHitEffect = Instantiate(NoteHitEffect, targets[0].transform.position, Quaternion.identity) as GameObject;

        //metalPopup.GetComponent<TransformAndRotate>().RotationZ *= Random.Range(0.2f, 1.4f);
        //angstPopup.GetComponent<TransformAndRotate>().RotationZ *= Random.Range(0.2f, 1.4f);

        //Destroy(metalPopup, 2);
        //Destroy(angstPopup, 2);
        Destroy(noteHitEffect, 5);

        StringAnimator.SetTrigger("StringStroked");

        Destroy(targets[0]);
        //This line is the cause of index-errors when hitting notes.
        targets.RemoveAt(0);
    }

    private GameObject GetNoteAccuracyPrefab()
    {
        float distance = transform.position.y - targets[0].transform.position.y;
        if (distance < 0)
            distance *= -1;

        GameObject go = new GameObject();

        //Decending order: Perfect, Good, Bad, Miss.
        if (distance <= PerfectDistance)
            go = PerfectPopupPrefab;
        else if (distance <= GoodDistance)
            go = GoodPopupPrefab;
        else if (distance <= BadDistance)
            go = BadPopupPrefab;
        else
            go = MissPopupPrefab;

        return go;
    }

    private void UpdateStreakCounters(int value)
    {
        streakCounter += value;

        if (streakCounter > streakHighScoreCounter)
        {
            streakHighScoreCounter = streakCounter;
            HighestStreakCounter.text = "Highest Streak: " + streakCounter.ToString();
        }
        StreakCounter.text = streakCounter.ToString();
    }

    private void AddOrRemoveHealth(int amount)
    {
        health += amount;

        if (health > MaxHealth)
            health = MaxHealth;
        else if (health <= 0 && GigBackgroundManager.GigSession)
        {
            FindObjectOfType<NoteGenerator>().EndGame();
        }
        HealthSlider.value = MaxHealth - health;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CanExitCollider && collision.transform.position.y < transform.position.y - 0.5f)
        {
            FailTiming();
        }
    }
}