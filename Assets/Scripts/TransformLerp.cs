using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLerp : MonoBehaviour
{
    public float Speed = 1;
    public Vector2 StartPosition;
    public Vector2 TargetPosition;

    private float posTimer = 0;

    public bool Lerp = true;
    public bool Reverse;

    private void Start()
    {
        if (StartPosition == Vector2.zero)
            StartPosition = transform.localPosition;

        TargetPosition = new Vector2(StartPosition.x + TargetPosition.x, StartPosition.y + TargetPosition.y);
    }

    void Update()
    {
        if (Lerp)
        {
            if (Reverse && posTimer >= 0)
                posTimer -= Time.deltaTime * Speed;
            else if(posTimer <= 1 && !Reverse)
                posTimer += Time.deltaTime * Speed;

            transform.localPosition = Vector2.Lerp(StartPosition, TargetPosition, posTimer);
        }
    }

    public void ReverseLerp()
    {
        Reverse = !Reverse;
    }

    public void StartLerp()
    {
        Lerp = true;
    }

    public void StopLerp()
    {
        Lerp = false;
    }
}
