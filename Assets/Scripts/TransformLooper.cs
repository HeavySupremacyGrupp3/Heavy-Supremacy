using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLooper : MonoBehaviour
{

   
    public float Speed = 1;
    public Vector2 TargetPosition;

    private float posTimer = 0;
    private Vector2 startPos;

    private bool looping;

    void OnEnable()
    {
        MiniGameManager.stopProducts += StopLoop;
    }

    void OnDisable()
    {
        MiniGameManager.stopProducts -= StopLoop;
    }

    private void Start()
    {
        looping = true;
        startPos = transform.localPosition;
        TargetPosition = new Vector2(transform.localPosition.x + TargetPosition.x, transform.localPosition.y + TargetPosition.y);
    }

    void Update()
    {
        if (looping)
        {
            posTimer += Time.deltaTime * Speed;
            if (posTimer >= 1)
                posTimer = 0;

            transform.localPosition = Vector2.Lerp(startPos, TargetPosition, posTimer);
        }
    }

    private void StopLoop()
    {
        looping = !looping;
    }
}
