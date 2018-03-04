﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingString : TimingSystem
{
    public string[] ErrorSounds;
    public AudioSource AudioSource;

    public float MaxHealth = 5;
    public Image HealthImage;
    public Sprite StandardHeart;
    public Sprite BurningHeart;
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

    public int RequiredStreaksForHealth = 10;
    public int HealthGainedPerStreak = 5;
    public int RequiredStreaksForEffect = 10;

    public Animator StringAnimator;
    public Text StreakCounter;
    public Text HighestStreakCounter;
    public GameObject StreakEffect;
    public Bloodshot BloodShotEffect;
    public float MaxStreakEffectCounter;

    public NoteGenerator NoteGen;

    public static float AngstMultiplier = 1;
    public static float MetalMultiplier = 1;
    public static float NotesHit = 0;

    private float streakCounter = 0;
    private float streakHighScoreCounter = 0;
    private float perfectCounter = 0;
    private float health;

    void Start()
    {
        health = MaxHealth;
        HealthImage.fillAmount = MaxHealth;
        if (!GigBackgroundManager.GigSession)
            HealthImage.gameObject.SetActive(false);

        NotesHit = 0;
    }

    public override void FailTiming()
    {
            base.FailTiming();

            NoteGen.SwitchMusicSource(false);
            Destroy(Instantiate(MissPopupPrefab, new Vector2(transform.position.x, transform.position.y + 5), Quaternion.identity), 3);

            AddOrRemoveHealth(-1);
            UpdateStreakCounters(-1000000, null);
            perfectCounter = 0;


        AudioManager.instance.Play(ErrorSounds[Random.Range(0, ErrorSounds.Length)]);
        //StringAnimator.SetTrigger("StringStroked");
    }

    public override void SucceedTiming(GameObject note)
    {
        base.SucceedTiming(note);

        NoteGen.SwitchMusicSource(true);

        NotesHit++;
        UpdateStreakCounters(1, note);

        if (streakCounter % RequiredStreaksForHealth == 0 && streakCounter > 0)
        {
            AddOrRemoveHealth(HealthGainedPerStreak);
        }

        if (GigBackgroundManager.GigSession && perfectCounter % RequiredStreaksForEffect == 0 && perfectCounter > 0)
        {
            Destroy(Instantiate(StreakEffect), 3);
            AudioManager.instance.Play("StreakSound");
        }

        Destroy(Instantiate(GetNoteAccuracyPrefab(note), new Vector2(transform.position.x, transform.position.y + 5), Quaternion.identity), 3);

        //GameObject metalPopup = Instantiate(MetalPopupPrefab, target.transform.position, Quaternion.identity) as GameObject;
        //GameObject angstPopup = Instantiate(AngstPopupPrefab, target.transform.position, Quaternion.identity) as GameObject;
        GameObject noteHitEffect = Instantiate(NoteHitEffect, note.transform.position, Quaternion.identity) as GameObject;
        Vector3 tempPos = noteHitEffect.transform.position;
        tempPos.y = transform.position.y;
        noteHitEffect.transform.position = tempPos;

        //metalPopup.GetComponent<TransformAndRotate>().RotationZ *= Random.Range(0.2f, 1.4f);
        //angstPopup.GetComponent<TransformAndRotate>().RotationZ *= Random.Range(0.2f, 1.4f);

        //Destroy(metalPopup, 2);
        //Destroy(angstPopup, 2);
        Destroy(noteHitEffect, 5);

        StringAnimator.SetTrigger("StringStroked");

        note.name = "DEAD_NOTE";

        GameObject tempTarget = note;
        note.GetComponent<BoxCollider2D>().enabled = false;

        note.transform.GetComponentInChildren<CutoffLerp>().Lerp = true;

        Destroy(tempTarget, 1);

        TargetInRange = false;

    }

    private GameObject GetNoteAccuracyPrefab(GameObject note)
    {
        if (!TargetInRange)
            return new GameObject();

        float distance = transform.position.y - note.transform.position.y;
        if (distance < 0)
            distance *= -1;

        GameObject go;

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

    private void UpdateStreakCounters(int value, GameObject note)
    {
        streakCounter += value;

        if (GetNoteAccuracyPrefab(note).name.Contains("Perfect"))
            perfectCounter += value;
        else
            perfectCounter = 0;

        if (streakCounter < 0)
            streakCounter = 0;
        if (perfectCounter < 0)
            perfectCounter = 0;

        if (streakCounter > streakHighScoreCounter)
        {
            streakHighScoreCounter = streakCounter;
            HighestStreakCounter.text = "Highest Streak: " + streakCounter.ToString();
        }
        StreakCounter.text = streakCounter.ToString();

        if (GigBackgroundManager.GigSession && streakCounter >= 10 || streakCounter == 0)
            BloodShotEffect.Progress = streakCounter / MaxStreakEffectCounter;
    }

    private void AddOrRemoveHealth(int amount)
    {
        health += amount;

        HealthImage.sprite = StandardHeart;

        if (health > MaxHealth)
        {
            health = MaxHealth;
            HealthImage.sprite = BurningHeart;
        }
        else if (health <= 0 && GigBackgroundManager.GigSession)
        {
            FindObjectOfType<NoteGenerator>().EndGame(false);
        }
        HealthImage.fillAmount = health / MaxHealth;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name != "DEAD_NOTE")
        {
            FailTiming();
            //Remove notes in the set until there is only one left, then remove the whole set.
            if (NoteGenerator.NoteSets[0].Notes.Count == 1)
            {
                NoteGenerator.NoteSets.RemoveAt(0);
            }
            else
            {
                NoteGenerator.NoteSets[0].Notes.RemoveAt(0);
            }
        }
    }

    public static void Reset()
    {
        MetalMultiplier = 1;
        AngstMultiplier = 1;
    }
}