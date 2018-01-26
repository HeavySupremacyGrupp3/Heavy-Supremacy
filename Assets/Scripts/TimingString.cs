using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingString : TimingSystem
{
    public AudioClip ErrorSound;
    public AudioSource AudioSource;

    public float AngstRewardAmount = -10;
    public float MetalRewardAmount = 15;
    public float Health = 0;

    public GameObject AngstPopupPrefab;
    public GameObject MetalPopupPrefab;
    public GameObject NoteHitEffect;

    public Animator StringAnimator;

    private angstStatScript AngstStatScript;
    private metalStatScript MetalStatScript;

    void Start()
    {
        AngstStatScript = FindObjectOfType<angstStatScript>();
        MetalStatScript = FindObjectOfType<metalStatScript>();
    }

    public override void FailTiming()
    {
        base.FailTiming();
        AudioSource.PlayOneShot(ErrorSound);

        StringAnimator.SetTrigger("StringStroked");

        Health--;
        if (Health == 0)
            FindObjectOfType<GameManager>().LoadHUB();
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();
        Destroy(target);
        AngstStatScript.addOrRemoveAmount(AngstRewardAmount);
        MetalStatScript.addOrRemoveAmount(MetalRewardAmount);

        GameObject metalPopup = Instantiate(MetalPopupPrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject angstPopup = Instantiate(AngstPopupPrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject noteHitEffect = Instantiate(NoteHitEffect, transform.position, Quaternion.identity) as GameObject;

        Destroy(metalPopup, 2);
        Destroy(angstPopup, 2);
        Destroy(noteHitEffect, 5);

        StringAnimator.SetTrigger("StringStroked");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CanExitCollider && collision.transform.position.y < transform.position.y - 0.5f)
        {
            FailTiming();
        }
    }
}