using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingString : TimingSystem
{
    public override void FailTiming()
    {
        base.FailTiming();
    }

    public override void SucceedTiming()
    {
        base.SucceedTiming();
        Destroy(target);
    }
}