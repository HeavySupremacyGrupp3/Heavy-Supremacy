using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingString : TimingSystem
{
    public AudioClip ErrorSound;
    public AudioSource AudioSource;
    public happinessStatScript HappinessStatScript;
    public metalStatScript MetalStatScript;
    public float HappinessRewardAmount = 10;
    public float MetalRewardAmount = 15;

    public GameObject HappinessPopupPrefab;
    public GameObject MetalPopupPrefab;
    public GameObject NoteHitEffect;

    public Animator StringAnimator;

    public override void FailTiming()
    {
        base.FailTiming();
        AudioSource.PlayOneShot(ErrorSound);

        StringAnimator.SetTrigger("StringStroked");
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();
        Destroy(target);
        HappinessStatScript.addOrRemoveAmount(HappinessRewardAmount);
        MetalStatScript.addOrRemoveAmount(MetalRewardAmount);

        GameObject metalPopup = Instantiate(MetalPopupPrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject happinessPopup = Instantiate(HappinessPopupPrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject noteHitEffect = Instantiate(NoteHitEffect, transform.position, Quaternion.identity) as GameObject;

        Destroy(metalPopup, 2);
        Destroy(happinessPopup, 2);
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