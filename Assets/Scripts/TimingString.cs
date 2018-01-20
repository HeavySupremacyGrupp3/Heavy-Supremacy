using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingString : TimingSystem
{
    public AudioClip ErrorSound;
    public AudioSource AudioSource;

    public override void FailTiming()
    {
        base.FailTiming();
        AudioSource.PlayOneShot(ErrorSound);
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();
        Destroy(target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CanExitCollider && collision.transform.position.y < transform.position.y - 0.5f)
        {
            FailTiming();
        }
    }
}