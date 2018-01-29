using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingString : TimingSystem
{
    public AudioClip[] ErrorSounds;
    public AudioSource AudioSource;

    public float AngstRewardAmount = -10;
    public float MetalRewardAmount = 15;
    public float Health = 0;

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

    public static float AngstMultiplier = 1;
    public static float MetalMultiplier = 1;

    private angstStatScript AngstStatScript;
    private metalStatScript MetalStatScript;

    void Start()
    {
        AngstStatScript = FindObjectOfType<angstStatScript>();
        MetalStatScript = FindObjectOfType<metalStatScript>();
    }

    public override void FailTiming()
    {
        Destroy(Instantiate(MissPopupPrefab, transform.position, Quaternion.identity), 3);
        base.FailTiming();
        AudioSource.PlayOneShot(ErrorSounds[Random.Range(0, ErrorSounds.Length)]);
        StringAnimator.SetTrigger("StringStroked");

        Health--;
        if (Health == 0)
            FindObjectOfType<GameManager>().LoadHUB();
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();

        Destroy(Instantiate(GetNoteAccuracyPrefab(), transform.position, Quaternion.identity), 3);

        Destroy(target);
        AngstStatScript.addOrRemoveAmount(AngstRewardAmount * AngstMultiplier);
        MetalStatScript.addOrRemoveAmount(MetalRewardAmount * MetalMultiplier);

        GameObject metalPopup = Instantiate(MetalPopupPrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject angstPopup = Instantiate(AngstPopupPrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject noteHitEffect = Instantiate(NoteHitEffect, transform.position, Quaternion.identity) as GameObject;

        metalPopup.GetComponent<TransformAndRotate>().RotationZ *= Random.Range(0.2f, 1.4f);
        angstPopup.GetComponent<TransformAndRotate>().RotationZ *= Random.Range(0.2f, 1.4f);

        Destroy(metalPopup, 2);
        Destroy(angstPopup, 2);
        Destroy(noteHitEffect, 5);

        StringAnimator.SetTrigger("StringStroked");
    }

    private GameObject GetNoteAccuracyPrefab()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CanExitCollider && collision.transform.position.y < transform.position.y - 0.5f)
        {
            FailTiming();
        }
    }
}