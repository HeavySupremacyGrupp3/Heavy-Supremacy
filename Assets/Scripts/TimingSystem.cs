using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimingSystem : MonoBehaviour
{

    public KeyCode ActivasionKey1;
    public KeyCode ActivasionKey2;
    public KeyCode ActivasionKey3;

    public bool CanComboKey;
    public bool CanExitCollider;

    protected GameObject target;

    void Update()
    {
        if (Input.GetKeyDown(ActivasionKey1) || Input.GetKeyDown(ActivasionKey2) || Input.GetKeyDown(ActivasionKey3))
            ActivateMechanic();
    }

    void ActivateMechanic()
    {
        if (target == null)
        {
            FailTiming();
        }
        else if (target != null)
        {
            if (target.name.Contains("_1") && Input.GetKey(ActivasionKey1) ||
                target.name.Contains("_2") && Input.GetKey(ActivasionKey2) ||
                target.name.Contains("_3") && Input.GetKey(ActivasionKey3))
            {
                SucceedTiming();
            }
        }
    }

    public virtual void FailTiming()
    {
        Debug.Log("FAILED TIMING");
        target = null;
    }

    public virtual void SucceedTiming()
    {
        Debug.Log("SUCCEEDED TIMING");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "TimingObject")
        {
            target = collision.gameObject;
        }
    }
}
