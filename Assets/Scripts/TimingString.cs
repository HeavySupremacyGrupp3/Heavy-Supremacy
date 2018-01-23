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

    public override void FailTiming()
    {
        base.FailTiming();
        AudioSource.PlayOneShot(ErrorSound);
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();
        Destroy(target);
        HappinessStatScript.addOrRemoveAmount(HappinessRewardAmount);
        MetalStatScript.addOrRemoveAmount(MetalRewardAmount);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CanExitCollider && collision.transform.position.y < transform.position.y - 0.5f)
        {
            FailTiming();
        }
    }
}