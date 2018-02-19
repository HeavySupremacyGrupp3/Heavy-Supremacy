using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingString : TimingSystem
{
    public string[] ErrorSounds;
    public AudioSource AudioSource;

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

    public int RequiredStreaksForHealth = 10;
    public int HealthGainedPerStreak = 5;
    public int RequiredStreaksForEffect = 10;

    public Animator StringAnimator;
    public Text StreakCounter;
    public Text HighestStreakCounter;
    public GameObject StreakEffect;

    public NoteGenerator NoteGen;

    public static float AngstMultiplier = 1;
    public static float MetalMultiplier = 1;
    public static float NotesHit = 0;

    private int streakCounter = 0;
    private int streakHighScoreCounter = 0;
    private int health;

    void Start()
    {
        health = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
        if (!GigBackgroundManager.GigSession)
            HealthSlider.gameObject.SetActive(false);

        NotesHit = 0;
    }

    public override void FailTiming()
    {
        //Make sure note cannot fail if it has been hit.
        if (hitTargets.Count > 0 && targets.Count > 0 && hitTargets.Contains(targets[0].gameObject))
        {
            ClearHitTargetList();
            return;
        }
        else
        {
            base.FailTiming();

            NoteGen.SwitchMusicSource(true);
            Destroy(Instantiate(MissPopupPrefab, new Vector2(transform.position.x, transform.position.y + 5), Quaternion.identity), 3);

            AddOrRemoveHealth(-1);
            UpdateStreakCounter(-streakCounter);

            ClearHitTargetList();
        }

        AudioManager.instance.Play(ErrorSounds[Random.Range(0, ErrorSounds.Length)]);
        StringAnimator.SetTrigger("StringStroked");
    }

    void ClearHitTargetList()
    {
        foreach (GameObject go in hitTargets)
        {
            Destroy(go);
        }
        hitTargets.Clear();
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();

        NoteGen.SwitchMusicSource(false);

        NotesHit++;
        UpdateStreakCounter(1);

        if (streakCounter % RequiredStreaksForHealth == 0)
            AddOrRemoveHealth(HealthGainedPerStreak);

        if(streakCounter % RequiredStreaksForEffect == 0)
            Destroy(Instantiate(StreakEffect), 3);

        Destroy(Instantiate(GetNoteAccuracyPrefab(), new Vector2(transform.position.x, transform.position.y + 5), Quaternion.identity), 3);

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

        targets.Clear();
    }

    private GameObject GetNoteAccuracyPrefab()
    {
        float distance = transform.position.y - targets[0].transform.position.y;
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

    private void UpdateStreakCounter(int value)
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
            FindObjectOfType<NoteGenerator>().EndGame(false);
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

    public static void Reset()
    {
        MetalMultiplier = 1;
        AngstMultiplier = 1;
    }
}